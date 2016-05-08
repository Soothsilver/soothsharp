using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Sharpsilver.Translation;

namespace Sharpsilver.VisualStudioPlugin
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SharpsilverVisualStudioPluginAnalyzer : DiagnosticAnalyzer
    {
        private Dictionary<string, DiagnosticDescriptor> Rules = null;
        private IEnumerable<DiagnosticDescriptor> GetRules()
        {
            var diagnostics = Sharpsilver.Translation.Diagnostics.GetAllDiagnostics().ToList();
            bool firstTime = Rules == null;
            if (firstTime) Rules = new Dictionary<string, DiagnosticDescriptor>();
            foreach (var diagnostic in diagnostics)
            {
                DiagnosticDescriptor dd = new DiagnosticDescriptor(
                    diagnostic.ErrorCode,
                    diagnostic.Caption,
                    diagnostic.Caption,
                    "Sharpsilver.Translation",
                    transformSeverity(diagnostic.Severity),
                    true,
                    description: diagnostic.Details);
                if (firstTime)
                Rules.Add(diagnostic.ErrorCode, dd);
                yield return dd;
            }
        }

        private Microsoft.CodeAnalysis.DiagnosticSeverity transformSeverity(Translation.DiagnosticSeverity severity)
        {
            switch (severity)
            {
                case Translation.DiagnosticSeverity.UnrecoverableError: return Microsoft.CodeAnalysis.DiagnosticSeverity.Error;
                case Translation.DiagnosticSeverity.Error: return Microsoft.CodeAnalysis.DiagnosticSeverity.Error;
                case Translation.DiagnosticSeverity.Warning: return Microsoft.CodeAnalysis.DiagnosticSeverity.Warning;
                default: throw new NotImplementedException("This diagnostic severity could not be handled.");
            }
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get {
                return ImmutableArray.Create(GetRules().ToArray());
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information

            context.RegisterSyntaxTreeAction(wholeTreeAnalysis);

            context.RegisterSyntaxNodeAction(CC, ImmutableArray.Create(SyntaxKind.SwitchStatement));
            /*    context.RegisterSyntaxNodeAction((snac) => {

                }, ImmutableArray.Create(SyntaxKind.SwitchKeyword));
                */
        }

        private void wholeTreeAnalysis(SyntaxTreeAnalysisContext treeContext)
        {
            var translationProcess = new TranslationProcess();
            var result = translationProcess.TranslateTree(treeContext.Tree);
            foreach(var diagnostic in result.ReportedDiagnostics)
            {
                treeContext.ReportDiagnostic(Diagnostic.Create(
                    Rules[diagnostic.Diagnostic.ErrorCode], diagnostic.Node.GetLocation(), diagnostic.DiagnosticArguments)
                    );
            }

        }

        private void CC(SyntaxNodeAnalysisContext obj)
        {
           obj.ReportDiagnostic(Diagnostic.Create(SupportedDiagnostics[0], obj.Node.GetLocation()));
        }
        /*
        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            // Find just those named type symbols with names containing lowercase letters.
            if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }*/
    }
}
