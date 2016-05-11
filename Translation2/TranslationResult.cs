using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation
{
    public class TranslationResult
    {
        // Universal
        public Silvernode SilverSourceTree;
        public bool WasTranslationSuccessful
        {
            get
            {
                return !ReportedDiagnostics.Any(err => err.Diagnostic.Severity == DiagnosticSeverity.Error);
            }
        }
        public List<Error> ReportedDiagnostics = new List<Error>();

        // For methods and loops.
        public List<Silvernode> VerificationConditions = new List<Silvernode>();

        public static TranslationResult Error(SyntaxNode node, SharpsilverDiagnostic diagnostic, params Object[] diagnosticArguments)
        {
            TranslationResult r = new TranslationResult();
            r.SilverSourceTree = new ErrorSilvernode(node);
            r.ReportedDiagnostics.Add(new Translation.Error(diagnostic, node,diagnosticArguments));
            return r;
        }
        public static TranslationResult Silvernode(Silvernode node, IEnumerable<Error> errors = null)
        {
            TranslationResult result = new TranslationResult();
            result.SilverSourceTree = node;
            if (errors != null)
            {
                result.ReportedDiagnostics.AddRange(errors);
            }
            return result;
        }

        public string GetSilverCodeAsString()
        {
            if (SilverSourceTree == null)
            {
                return "/*[! ERROR !]*/";
            }
            return SilverSourceTree.ToString();
        }
    }
}
