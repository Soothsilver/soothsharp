using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Highlevel;
using Sharpsilver.Translation.Translators;
using System.Collections.Generic;
using Sharpsilver.Translation.AbstractSyntaxTrees.Intermediate;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation
{
    public class TranslationProcess
    {
        public CSharpCompilation Compilation;
        public SemanticModel SemanticModel;
        public IdentifierTranslator IdentifierTranslator = new IdentifierTranslator();
        public ContractsTranslator ContractsTranslator;
        private List<CollectedType> collectedTypes = new List<CollectedType>();
        public CollectedType AddToCollectedTypes(ClassSharpnode classSharpnode)
        {
            var name = IdentifierTranslator.RegisterAndGetIdentifier(
                SemanticModel.GetDeclaredSymbol(classSharpnode.DeclarationSyntax));
            var superclassObject = IdentifierTranslator.SystemObject;
            CollectedType type = new CollectedType(name, superclassObject);
            collectedTypes.Add(type);
            return type;
        }
        public TranslationProcess()
        {
            this.ContractsTranslator =   new ContractsTranslator(this);
        }
        public TranslationResult TranslateCode(string csharpCode, bool writeProgressToConsole = false)
        {
            if (writeProgressToConsole) Console.WriteLine("- Syntax analysis begins.");
            SyntaxTree tree = CSharpSyntaxTree.ParseText(csharpCode);
            return TranslateTree(tree, writeProgressToConsole);
        }


        public TranslationResult TranslateTree(SyntaxTree tree, bool writeProgressToConsole = false)
        {
            var mscorlib = MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location);

            // 1. Prepare compilation object.
            if (writeProgressToConsole) Console.WriteLine("- Compiling.");
            this.Compilation = CSharpCompilation.Create("translated_assembly")
                                    .AddSyntaxTrees(tree)
                                    .AddReferences(mscorlib)
                                    .AddReferences(MetadataReference.CreateFromFile("Sharpsilver.Contracts.dll"))
                                    ;

            // 2. Prepare semantic analysis.
            if (writeProgressToConsole) Console.WriteLine("- Creating semantic model.");
            this.SemanticModel = this.Compilation.GetSemanticModel(tree, true);

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
            cSharpTree.CollectTypesInto(this);

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

            // 5a. Add types and common code
            HighlevelSequenceSilvernode rootSilvernode = new AbstractSyntaxTrees.Silver.HighlevelSequenceSilvernode(
                null, translationResult.Silvernode);

            HighlevelSequenceSilvernode axioms = new HighlevelSequenceSilvernode(null);

            CSharpTypeDomainSilvernode domain = new CSharpTypeDomainSilvernode(axioms);

            rootSilvernode.List.Add(domain);
            foreach (var collectedType in collectedTypes)
            {
                rootSilvernode.List.Add(collectedType.GenerateGlobalSilvernode());
                axioms.List.Add(collectedType.GenerateSilvernodeInsideCSharpType());
            }

            
            // 6. Assign names to identifiers
            this.IdentifierTranslator.AssignTrueNames();

            // 7. Postprocessing.
            rootSilvernode.Postprocess();
            translationResult.Silvernode = rootSilvernode;
            return translationResult;
        }

     
    }
}
