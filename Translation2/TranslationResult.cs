using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation
{
    public class TranslationResult
    {
        public List<SilverSourceCode> SilverCode = new List<SilverSourceCode>();
        public bool WasTranslationSuccessful;
        public List<Error> Errors = new List<Error>();

        public TranslationResult(SilverSourceCode result)
        {
            SilverCode.Add(result);
            WasTranslationSuccessful = true;
        }
        private TranslationResult()
        {

        }

        internal static TranslationResult Combine(List<TranslationResult> results)
        {
            TranslationResult result = new TranslationResult();
            foreach(TranslationResult sub in results)
            {
                result.Errors.AddRange(sub.Errors);
                result.SilverCode.AddRange(sub.SilverCode);
            }
            result.WasTranslationSuccessful = results.TrueForAll(sub => sub.WasTranslationSuccessful);
            return result;
        }

        internal static TranslationResult Error(SyntaxNode node, SharpsilverDiagnostic diagnostic, params Object[] diagnosticArguments)
        {
            TranslationResult r = new TranslationResult();
            r.Errors.Add(new Translation.Error(diagnostic, node,diagnosticArguments));
            return r;
        }

        public string GetSilverCodeAsString()
        {
            return String.Join("", SilverCode.Select(scc => scc.GetSilverCode()));
        }
    }
}
