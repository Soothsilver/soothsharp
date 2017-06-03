using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Soothsharp.Rewriter
{
    /// <summary>
    /// This rewriter program removes all contracts from C# code.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.Error.WriteLine("The rewriter must have one or zero arguments.");
            }
            if (args.Length == 1)
            {
                try
                {
                    RewriteCode(File.ReadAllText(args[0]));
                }
                catch (IOException)
                {
                    Console.Error.WriteLine("The file '" + args[0] + "' could not be opened.");
                }
            }
            else
            {
                string line;
                string input = "";
                while ((line = Console.ReadLine()) != null)
                {
                    input += line + Environment.NewLine;
                }
                RewriteCode(input);
            }
        }

        private static void RewriteCode(string input)
        {
            // Inspiration:
            // https://github.com/dotnet/roslyn/wiki/Getting-Started-C%23-Syntax-Transformation

            CSharpSyntaxTree tree = (CSharpSyntaxTree) CSharpSyntaxTree.ParseText(input);
            var mscorlib = MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location);
            var contractsLibrary = MetadataReference.CreateFromFile(typeof(Contracts.Contract).Assembly.Location);
            var systemCore = MetadataReference.CreateFromFile(typeof(ParallelQuery).Assembly.Location);
            CSharpCompilation compilation = CSharpCompilation.Create("translated_assembly", new[] {tree},
                    new[] {mscorlib, contractsLibrary, systemCore});
            SemanticModel semanticModel = compilation.GetSemanticModel(tree, true);

            ContractsRemover remover = new ContractsRemover(semanticModel);

            SyntaxNode newSource = remover.Visit(tree.GetRoot());

            Console.Write(newSource.ToFullString());
        }
    }
    
    /// <summary>
    /// This rewriter removes contracts (requires, ensures, invariant) completely and modifies
    /// "folding" and "unfolding" so as to remove their permission part but keep the executable part.
    /// </summary>
    /// <seealso cref="Microsoft.CodeAnalysis.CSharp.CSharpSyntaxRewriter" />
    public class ContractsRemover : CSharpSyntaxRewriter
    {
        private readonly SemanticModel SemanticModel;

        private readonly string[] ForbiddenStatements = new[]
        {
             ContractsTranslator.ContractAssume,
             ContractsTranslator.ContractAssert,
             ContractsTranslator.ContractEnsures,
             ContractsTranslator.ContractExhale,
             ContractsTranslator.ContractInhale,
             ContractsTranslator.ContractInvariant,
             ContractsTranslator.ContractRequires,
             ContractsTranslator.Fold,
             ContractsTranslator.Unfold
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractsRemover"/> class.
        /// </summary>
        public ContractsRemover(SemanticModel model)
        {
            SemanticModel = model;
        }

        /// <inheritdoc />
        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            // The first argument of "unfolding" and "folding" is only relevant in Viper code, not in C#, 
            // the second argument, however, is executable code, so we leave that.

            var symbol = SemanticModel.GetSymbolInfo(node.Expression).Symbol;
            var name = symbol.GetQualifiedName();

            if (name == ContractsTranslator.Unfolding || name == ContractsTranslator.Folding)
            {
                return node.ArgumentList.Arguments[1].Expression;
            }

            return base.VisitInvocationExpression(node);
        }

        /// <inheritdoc />
        public override SyntaxNode VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            // Contracts are removed from the syntax tree.

            var symbol = SemanticModel.GetSymbolInfo(node.Expression).Symbol;
            var name = symbol.GetQualifiedName();
            if (ForbiddenStatements.Contains(name))
            {
                return null;
            }
            return base.VisitExpressionStatement(node);
        }
    }
}
