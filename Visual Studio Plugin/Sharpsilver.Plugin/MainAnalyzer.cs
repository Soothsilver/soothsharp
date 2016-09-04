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
using Sharpsilver.Translation.BackendInterface;

namespace Sharpsilver.Plugin
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SharpsilverVisualStudioPluginAnalyzer : DiagnosticAnalyzer
    {
        private Dictionary<string, DiagnosticDescriptor> rules;
        private IEnumerable<DiagnosticDescriptor> GetRules()
        {
            var diagnostics = Sharpsilver.Translation.Diagnostics.GetAllDiagnostics().ToList();
            bool firstTime = rules == null;
            if (firstTime) rules = new Dictionary<string, DiagnosticDescriptor>();
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
                rules.Add(diagnostic.ErrorCode, dd);
                yield return dd;
            }
        }

        private Microsoft.CodeAnalysis.DiagnosticSeverity transformSeverity(Translation.DiagnosticSeverity severity)
        {
            switch (severity)
            {
                case Translation.DiagnosticSeverity.Error: return Microsoft.CodeAnalysis.DiagnosticSeverity.Error;
                case Translation.DiagnosticSeverity.Warning: return Microsoft.CodeAnalysis.DiagnosticSeverity.Warning;
                default: throw new Exception("This diagnostic severity could not be handled.");
            }
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(GetRules().ToArray());

        public override void Initialize(AnalysisContext context)
        {
            // Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more

            context.RegisterSyntaxTreeAction(WholeTreeTranslationAnalysis);
            context.RegisterSyntaxTreeAction(WholeTreeVerificationAnalysis);

            /*    context.RegisterSyntaxNodeAction((snac) => {

                }, ImmutableArray.Create(SyntaxKind.SwitchKeyword));
                */
        }

        private void WholeTreeVerificationAnalysis(SyntaxTreeAnalysisContext treeContext)
        {
            /*
            if (treeContext.Tree.GetText().ToString().Length < 10)
            {
                return;
            }
            var translationProcess = TranslationProcess.Create()
            var result = translationProcess.TranslateTree(treeContext.Tree);
            if (!result.WasTranslationSuccessful) return; // translation errors are handled by the other method
            if (result.Silvernode.ToString().Trim() == "") return;
            var verifier = new SiliconBackend();
            var verificationResult = verifier.Verify(result.Silvernode);
            foreach (var diagnostic in verificationResult.Errors)
            {
                if (diagnostic.Node != null)
                {
                    treeContext.ReportDiagnostic(Diagnostic.Create(
                        rules[diagnostic.Diagnostic.ErrorCode], diagnostic.Node.GetLocation(), diagnostic.DiagnosticArguments)
                        );
                }
                else
                {
                    treeContext.ReportDiagnostic(Diagnostic.Create(
                        rules[diagnostic.Diagnostic.ErrorCode], null, diagnostic.DiagnosticArguments)
                        );
                }
            }*/
        }

        private void WholeTreeTranslationAnalysis(SyntaxTreeAnalysisContext treeContext)
        {
            /*
            var translationProcess = new TranslationProcess();
            var result = translationProcess.TranslateTree(treeContext.Tree);
            foreach(var diagnostic in result.Errors)
            {

                if (diagnostic.Node != null)
                {
                    treeContext.ReportDiagnostic(Diagnostic.Create(
                        rules[diagnostic.Diagnostic.ErrorCode], diagnostic.Node.GetLocation(), diagnostic.DiagnosticArguments)
                        );
                }
                else
                {
                    treeContext.ReportDiagnostic(Diagnostic.Create(
                        rules[diagnostic.Diagnostic.ErrorCode], null, diagnostic.DiagnosticArguments)
                        );
                }
            }*/

        }
    }
}
