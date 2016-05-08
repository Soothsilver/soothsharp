using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees
{
    class CommonUtils
    {
        public static TranslationResult CombineResults(IEnumerable<TranslationResult> results, SyntaxNode parent = null)
        {
            TranslationResult result = new TranslationResult();
            SequenceSilvernode sequence = new SequenceSilvernode(parent);
            foreach(var incoming in results)
            {
                result.ReportedDiagnostics.AddRange(incoming.ReportedDiagnostics);
                if (incoming.SilverSourceTree != null)
                {
                    sequence.List.Add(incoming.SilverSourceTree);
                }
            }
            result.SilverSourceTree = sequence;
            result.WasTranslationSuccessful = results.All(rs => rs.WasTranslationSuccessful);
            return result;
        }
    }
}
