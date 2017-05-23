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

        public ContractsRemover(SemanticModel model)
        {
            SemanticModel = model;
        }

        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var symbol = SemanticModel.GetSymbolInfo(node.Expression).Symbol;
            var name = symbol.GetQualifiedName();

            if (name == ContractsTranslator.Unfolding || name == ContractsTranslator.Folding)
            {
                return node.ArgumentList.Arguments[1].Expression;
            }

            return base.VisitInvocationExpression(node);
        }
        public override SyntaxNode VisitExpressionStatement(ExpressionStatementSyntax node)
        {
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
