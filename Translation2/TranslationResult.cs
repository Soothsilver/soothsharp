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
        public Silvernode SilverSourceTree;
        public bool WasTranslationSuccessful = true;
        public List<Error> ReportedDiagnostics = new List<Error>();

        public static TranslationResult Error(SyntaxNode node, SharpsilverDiagnostic diagnostic, params Object[] diagnosticArguments)
        {
            TranslationResult r = new TranslationResult();
            r.WasTranslationSuccessful = false;
            r.ReportedDiagnostics.Add(new Translation.Error(diagnostic, node,diagnosticArguments));
            return r;
        }
        public static TranslationResult Silvernode(Silvernode node)
        {
            TranslationResult result = new TranslationResult();
            result.SilverSourceTree = node;
            return result;
        }

        public string GetSilverCodeAsString()
        {
            return SilverSourceTree.ToString();
        }
    }
}
