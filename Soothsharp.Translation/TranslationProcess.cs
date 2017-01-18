using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Soothsharp.Translation.Trees.CSharp;
using Soothsharp.Translation.Trees.CSharp.Highlevel;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    /// <summary>
    /// The translation process contains all information about verifying a C# solution and encapsulates all procedures necessary for verification.
    /// The process is started by either the Visual Studio plugin frontend or the standalone csverify.exe frontend.
    /// </summary>
    public class TranslationProcess
    {
        internal IdentifierTranslator IdentifierTranslator;
        public ContractsTranslator ContractsTranslator;
        public ConstantsTranslator ConstantsTranslator;
        internal TranslationConfiguration Configuration;
        private List<CollectedType> collectedTypes = new List<CollectedType>();
        private List<CompilationUnit> compilationUnits = new List<CompilationUnit>();
        private List<string> referencedAssemblies = new List<string>();
        internal CollectedType AddToCollectedTypes(ClassSharpnode classSharpnode, SemanticModel semanticModel)
        {
            this.IdentifierTranslator.RegisterAndGetIdentifier(
                semanticModel.GetDeclaredSymbol(classSharpnode.DeclarationSyntax));
            CollectedType type = new CollectedType(semanticModel.GetDeclaredSymbol(classSharpnode.DeclarationSyntax), classSharpnode.IsStatic);
            this.collectedTypes.Add(type);
            return type;
        }
        private TranslationProcess()
        {
            this.ContractsTranslator = new ContractsTranslator();
            this.ConstantsTranslator = new ConstantsTranslator();
            this.IdentifierTranslator = new IdentifierTranslator(this);
        }
        List<Error> masterErrorList = new List<Error>();
        private bool executed;


        /// <summary>
        /// Creates a new <see cref="TranslationProcess"/> from a C# syntax tree supplied by Visual Studio. 
        /// </summary>
        /// <param name="syntaxTree">The tree to translate.</param>
        // ReSharper disable once UnusedMember.Global - used by plugin
        public static TranslationProcess CreateFromSyntaxTree(
          SyntaxTree syntaxTree)
        {

            TranslationProcess process = new TranslationProcess();
            process.Configuration = new TranslationConfiguration()
            {
                Verbose = false,
                VerifyUnmarkedItems = true
            };
            process.compilationUnits.Add(CompilationUnit.CreateFromTree((CSharpSyntaxTree)syntaxTree, CompilationUnitVerificationStyle.FullVerification));
            return process;
        }
        public static TranslationProcess Create(
            List<string> verifiedFiles, 
            List<string> assumedFiles, 
            List<string> references, 
            TranslationConfiguration translationConfiguration)
        {
            TranslationProcess process = new TranslationProcess();
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
            this.masterErrorList.Add(error);
        }
        public TranslationProcessResult Execute()
        {
            if (this.executed)
            {
                throw new InvalidOperationException("The process was already executed once.");
            }
            this.executed = true;
            VerboseLog("Loading mscorlib and Soothsharp.Contracts...");
            var mscorlib = MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location);
            var contractsLibrary = MetadataReference.CreateFromFile(typeof(Contracts.Contract).Assembly.Location);
            var systemCore = MetadataReference.CreateFromFile(typeof(System.Linq.ParallelQuery).Assembly.Location);
            VerboseLog("Initializing compilation...");
            CSharpCompilation compilation;
            try
            {
                 compilation = CSharpCompilation.Create("translated_assembly", this.compilationUnits.Select(unit => unit.RoslynTree),
                    (new[] { mscorlib, contractsLibrary, systemCore }).Union(this.referencedAssemblies.Select(filename => MetadataReference.CreateFromFile(filename)))
                    );
            } catch (System.IO.IOException exception)
            {
                return TranslationProcessResult.Error(null, Diagnostics.SSIL112_FileNotFound, exception.Message);
            }

            VerboseLog("Processing trees...");
            HighlevelSequenceSilvernode masterTree = new HighlevelSequenceSilvernode(null);
            foreach (CompilationUnit compilationUnit in this.compilationUnits)
            {
                VerboseLog("Processing " + compilationUnit.RoslynTree.FilePath + "...");
                VerboseLog("- Semantic analysis...");
                SemanticModel semanticModel = compilation.GetSemanticModel(compilationUnit.RoslynTree, false);

                VerboseLog("- CONVERSION PHASE begins...");
                Sharpnode cSharpTree;
#if !DEBUG
                try
                {
#endif
                    cSharpTree = new CompilationUnitSharpnode(compilationUnit.RoslynTree.GetRoot() as CompilationUnitSyntax);
#if !DEBUG
            }
                catch (Exception ex)
                {
                    this.masterErrorList.Add(new Error(Diagnostics.SSIL103_ExceptionConstructingCSharp, compilationUnit.RoslynTree.GetRoot(), ex.ToString()));
                    continue;
                }
#endif

                VerboseLog("- COLLECTION PHASE begins...");
                cSharpTree.CollectTypesInto(this, semanticModel);

                VerboseLog("- MAIN PHASE begins...");
                TranslationResult translationResult;
                var diags = semanticModel.GetDiagnostics().ToList();
                bool skipTranslatingThisTree = false;
                foreach(var diag in diags)
                {
                    if (diag.Severity != Microsoft.CodeAnalysis.DiagnosticSeverity.Error) continue;
                    masterErrorList.Add(new Error(Diagnostics.SSIL123_ThereIsThisCSharpError, null, diag.ToString()));
                    skipTranslatingThisTree = true;
                }
                if (!skipTranslatingThisTree)
                {
#if !DEBUG
                try
                {
#endif
                    translationResult =
                        cSharpTree.Translate(TranslationContext.StartNew(this, semanticModel,
                            this.Configuration.VerifyUnmarkedItems, compilationUnit.Style));
#if !DEBUG
                }
                catch (Exception ex)
                {
                    this.masterErrorList.Add(new Error(Diagnostics.SSIL104_ExceptionConstructingSilver, compilationUnit.RoslynTree.GetRoot(), ex.ToString()));
                    continue;
                }
#endif
                    masterTree.List.Add(translationResult.Silvernode);
                    this.masterErrorList.AddRange(translationResult.Errors);
                }
            }

            VerboseLog("GLOBAL ADDITION PHASE begins...");
            foreach (var collectedType in this.collectedTypes)
            {
                masterTree.List.Add(collectedType.GenerateGlobalSilvernode(this));
            }
            

            VerboseLog("OPTIMIZATION PHASE begins...");
            masterTree.OptimizeRecursively();

            VerboseLog("NAME ASSIGNMENT PHASE begins...");
            this.IdentifierTranslator.AssignTrueNames();

            VerboseLog("POSTPROCESSING PHASE begins...");
            masterTree.Postprocess();

            return new TranslationProcessResult(masterTree, this.masterErrorList);
        }

        private void VerboseLog(string logline)
        {
            if (this.Configuration.Verbose)
            {
                Console.WriteLine("- " + logline);
            }
        }
    }
}
