using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.CSharp;
using Sharpsilver.Translation.Trees.CSharp.Highlevel;
using System.Collections.Generic;
using Sharpsilver.Translation.Trees.Silver;
using System.Linq;

namespace Sharpsilver.Translation
{
    /// <summary>
    /// The translation process contains all information about verifying a C# solution and encapsulates all procedures necessary for verification.
    /// The process is started by either the Visual Studio plugin frontend or the standalone csverify.exe frontend.
    /// </summary>
    public class TranslationProcess
    {
        internal IdentifierTranslator IdentifierTranslator = new IdentifierTranslator();
        private ContractsTranslator ContractsTranslator;
        internal TranslationConfiguration Configuration;
        private List<CollectedType> collectedTypes = new List<CollectedType>();
        private List<CompilationUnit> compilationUnits = new List<CompilationUnit>();
        private List<string> referencedAssemblies = new List<string>();
        internal CollectedType AddToCollectedTypes(ClassSharpnode classSharpnode, SemanticModel semanticModel)
        {
            var name = IdentifierTranslator.RegisterAndGetIdentifier(
                semanticModel.GetDeclaredSymbol(classSharpnode.DeclarationSyntax));
            var superclassObject = IdentifierTranslator.SystemObject;
            CollectedType type = new CollectedType(semanticModel.GetDeclaredSymbol(classSharpnode.DeclarationSyntax), name, superclassObject);
            collectedTypes.Add(type);
            return type;
        }
        private TranslationProcess()
        {
            this.ContractsTranslator = new ContractsTranslator(this);
        }
        List<Error> masterErrorList = new List<Error>();
        private bool executed = false;

        public static TranslationProcess Create(
            List<string> verifiedFiles, 
            List<string> assumedFiles, 
            List<string> references, 
            TranslationConfiguration translationConfiguration)
        {
            TranslationProcess process = new Translation.TranslationProcess();
            process.Configuration = translationConfiguration;
            foreach(string name in verifiedFiles)
            {
                if (translationConfiguration.Verbose)
                {
                    Console.WriteLine($"- Adding '{name}' to files marked for full verification.");
                }
                process.compilationUnits.Add(CompilationUnit.CreateFromFile(name, CompilationUnitVerificationStyle.FullVerification));
            }
            foreach (string name in assumedFiles)
            {
                if (translationConfiguration.Verbose)
                {
                    Console.WriteLine($"- Adding '{name}' to files marked for signature extraction only.");
                }
                process.compilationUnits.Add(CompilationUnit.CreateFromFile(name, CompilationUnitVerificationStyle.ContractsOnly));
            }
            foreach (string name in references)
            {
                if (translationConfiguration.Verbose)
                {
                    Console.WriteLine($"- Adding '{name}' to referenced assemblies.");
                }
                process.referencedAssemblies.Add(name);
            }


            return process;
        }

        internal void AddError(Error error)
        {
            masterErrorList.Add(error);
        }
        public TranslationProcessResult Execute()
        {
            if (executed)
            {
                throw new InvalidOperationException("The process was already executed once.");
            }
            executed = true;
            VerboseLog("Loading mscorlib and Sharpsilver.Contracts...");
            var mscorlib = MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location);
            var contractsLibrary = MetadataReference.CreateFromFile("Sharpsilver.Contracts.dll");
            VerboseLog("Initializing compilation...");
            CSharpCompilation compilation;
            try
            {
                 compilation = CSharpCompilation.Create("translated_assembly",
                    compilationUnits.Select(unit => unit.RoslynTree),
                    (new[] { mscorlib, contractsLibrary }).Union(referencedAssemblies.Select(filename => MetadataReference.CreateFromFile(filename)))
                    );
            } catch (System.IO.IOException exception)
            {
                return TranslationProcessResult.Error(null, Diagnostics.SSIL112_FileNotFound, exception.Message);
            }

            VerboseLog("Processing trees...");
            HighlevelSequenceSilvernode masterTree = new HighlevelSequenceSilvernode(null);
            foreach (CompilationUnit compilationUnit in compilationUnits)
            {
                VerboseLog("Processing " + compilationUnit.RoslynTree.FilePath + "...");
                VerboseLog("- Semantic analysis...");
                SemanticModel semanticModel = compilation.GetSemanticModel(compilationUnit.RoslynTree, false);

                VerboseLog("- Mapping to sharpnodes...");
                Sharpnode cSharpTree;
                try
                {
                    cSharpTree = new CompilationUnitSharpnode(compilationUnit.RoslynTree.GetRoot() as CompilationUnitSyntax);
                }
                catch (Exception ex)
                {
                    masterErrorList.Add(new Error(Diagnostics.SSIL103_ExceptionConstructingCSharp, compilationUnit.RoslynTree.GetRoot(), ex.ToString()));
                    continue;
                }

                VerboseLog("- COLLECTION PHASE begins...");
                cSharpTree.CollectTypesInto(this, semanticModel);

                VerboseLog("- MAIN PHASE begins...");
                TranslationResult translationResult;
                try
                {
                    translationResult = cSharpTree.Translate(TranslationContext.StartNew(this, semanticModel, Configuration.VerifyUnmarkedItems));
                }
                catch (Exception ex)
                {
                    masterErrorList.Add(new Error(Diagnostics.SSIL104_ExceptionConstructingSilver, compilationUnit.RoslynTree.GetRoot(), ex.ToString()));
                    continue;
                }
                masterTree.List.Add(translationResult.Silvernode);
                masterErrorList.AddRange(translationResult.Errors);
            }

            VerboseLog("GLOBAL ADDITION PHASE begins...");
            // Axioms and domains are not necessary now.
            
            //HighlevelSequenceSilvernode axioms = new HighlevelSequenceSilvernode(null);
            //CSharpTypeDomainSilvernode domain = new CSharpTypeDomainSilvernode(axioms);

            // masterTree.List.Add(domain);
            foreach (var collectedType in collectedTypes)
            {
                masterTree.List.Add(collectedType.GenerateGlobalSilvernode(this));
                //axioms.List.Add(collectedType.GenerateSilvernodeInsideCSharpType());
            }
            
            VerboseLog("REFERENCED ASSEMBLIES ADDITION PHASE is not yet implemented.");

            VerboseLog("OPTIMIZATION PHASE begins...");
            masterTree.OptimizeRecursively();

            VerboseLog("Assigning silvernames to identifiers...");
            this.IdentifierTranslator.AssignTrueNames();

            VerboseLog("Postprocessing...");
            masterTree.Postprocess();

            return new TranslationProcessResult(masterTree, masterErrorList);
        }

        private void VerboseLog(string logline)
        {
            if (Configuration.Verbose)
            {
                Console.WriteLine("- " + logline);
            }
        }
    }
}
