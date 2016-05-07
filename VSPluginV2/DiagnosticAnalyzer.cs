using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Sharpsilver.VisualStudioPlugin
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SharpsilverVisualStudioPluginAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SharpsilverVisualStudioPlugin";
        
    
        private const string Category = "Naming";
        
        private static DiagnosticDescriptor Rule2 = new DiagnosticDescriptor("CSSIL502", "The 'switch' keyword is not permitted.", "DESTROY ALL SWITCHES!", "Sharpsilver Translation", DiagnosticSeverity.Error, true);

        private IEnumerable<DiagnosticDescriptor> GetRules()
        {
            yield return Rule2;
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(GetRules().ToArray()); } }

        public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information

            context.RegisterSyntaxNodeAction(CC, ImmutableArray.Create(SyntaxKind.SwitchStatement));
        /*    context.RegisterSyntaxNodeAction((snac) => {
               
            }, ImmutableArray.Create(SyntaxKind.SwitchKeyword));
            */
        }

        private void CC(SyntaxNodeAnalysisContext obj)
        {
            obj.ReportDiagnostic(Diagnostic.Create(Rule2, obj.Node.GetLocation()));
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
