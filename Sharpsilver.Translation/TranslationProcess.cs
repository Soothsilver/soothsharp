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
    /// <summary>
    /// The translation process contains all information about verifying a C# solution and encapsulates all procedures necessary for verification.
    /// The process is started by either the Visual Studio plugin frontend or the standalone csverify.exe frontend.
    /// </summary>
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
            this.ContractsTranslator = new ContractsTranslator(this);
        }
        /// <summary>
        /// Translates C# code into a Silver syntax tree or produces errors.
        /// </summary>
        /// <param name="csharpCode">The C# code.</param>
        /// <param name="writeProgressToConsole">If true, translation progress will be written to the console.</param>
        public TranslationResult TranslateCode(string csharpCode, bool writeProgressToConsole = false)
        {
            if (writeProgressToConsole) Console.WriteLine("- Syntax analysis begins.");
            SyntaxTree tree = CSharpSyntaxTree.ParseText(csharpCode);
            return TranslateTree(tree, writeProgressToConsole);
        }
        /// <summary>
        /// Translates a C# syntax tree into a Silver syntax tree or produces error.
        /// </summary>
        /// <param name="tree">The C# syntax tree.</param>
        /// <param name="writeProgressToConsole">If true, translation progress will be written to the console.</param>
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
            if (writeProgressToConsole) Console.WriteLine("- Collecting types.");
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

            // 6. Add types and common code
            HighlevelSequenceSilvernode rootSilvernode = new HighlevelSequenceSilvernode(
                null, 
                translationResult.Silvernode);

            HighlevelSequenceSilvernode axioms = new HighlevelSequenceSilvernode(null);
            CSharpTypeDomainSilvernode domain = new CSharpTypeDomainSilvernode(axioms);

            rootSilvernode.List.Add(domain);
            foreach (var collectedType in collectedTypes)
            {
                rootSilvernode.List.Add(collectedType.GenerateGlobalSilvernode());
                axioms.List.Add(collectedType.GenerateSilvernodeInsideCSharpType());
            }

            
            // 7. Assign names to identifiers
            this.IdentifierTranslator.AssignTrueNames();

            // 8. Postprocessing.
            rootSilvernode.Postprocess();
            translationResult.Silvernode = rootSilvernode;
            return translationResult;
        }
    }
}
