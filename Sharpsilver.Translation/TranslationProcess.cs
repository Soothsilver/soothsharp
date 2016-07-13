using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Highlevel;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation
{
    public class TranslationProcess
    {
        public CSharpCompilation Compilation;
        public SemanticModel SemanticModel;
        public IdentifierTranslator IdentifierTranslator = new IdentifierTranslator();
        public ContractsTranslator ContractsTranslator;

        public TranslationProcess()
        {
            ContractsTranslator =   new ContractsTranslator(this);
        }
        public TranslationResult TranslateCode(string csharpCode, bool writeProgressToConsole = false)
        {
            if (writeProgressToConsole) Console.WriteLine("- Syntax analysis begins.");
            SyntaxTree tree = CSharpSyntaxTree.ParseText(csharpCode);
            return TranslateTree(tree, writeProgressToConsole);
        }


        public TranslationResult TranslateTree(SyntaxTree tree, bool writeProgressToConsole = false)
        {
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var mscorlib = MetadataReference.CreateFromFile(typeof(System.Attribute).Assembly.Location);

            // 1. Prepare compilation object.
            if (writeProgressToConsole) Console.WriteLine("- Compiling.");
            Compilation = CSharpCompilation.Create("translated_assembly")
                                    .AddSyntaxTrees(tree)
                                    .AddReferences(mscorlib)
                                    .AddReferences(MetadataReference.CreateFromFile("Sharpsilver.Contracts.dll"))
                                    ;

            // 2. Prepare semantic analysis.
            if (writeProgressToConsole) Console.WriteLine("- Creating semantic model.");
            SemanticModel = Compilation.GetSemanticModel(tree, true);

            // 3. Convert to Sharpnode intermediate representation.
            Sharpnode cSharpTree;
            if (writeProgressToConsole) Console.WriteLine("- Mapping to sharpnodes.");
            try
            {
                cSharpTree = new CompilationUnitSharpnode(tree.GetRoot() as CompilationUnitSyntax);
            }
            catch (Exception ex)
            {
                return TranslationResult.Error(null, Diagnostics.SSIL103_ExceptionConstructingCSharp, ex.ToString());
            }

            // 4. Collect types.

            // 5. Convert to Silver intermediate representation.
            if (writeProgressToConsole) Console.WriteLine("- Translating.");
            TranslationResult translationResult;
            try
            {
                translationResult = cSharpTree.Translate(TranslationContext.StartNew(this));
            }
            catch (Exception ex)
            {

                return TranslationResult.Error(null, Diagnostics.SSIL104_ExceptionConstructingSilver, ex.ToString());
            }

            // 6. Assign names to identifiers
            IdentifierTranslator.AssignTrueNames();

            // 7. Postprocessing.
            translationResult.Silvernode.Postprocess();
            return translationResult;
        }
        
    }
}
