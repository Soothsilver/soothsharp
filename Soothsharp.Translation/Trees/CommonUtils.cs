using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees
{
    /// <summary>
    /// These utility functions are useful when constructing abstract syntax tree nodes.
    /// </summary>
    static class CommonUtils
    {
        /// <summary>
        /// Gets a silvernode that combines the silvernodes of all elements of <paramref name="results"/>. They are separated by
        /// newlines. Errors are combined as well.
        /// </summary>
        /// <param name="results">The results to merge, separated by newlines.</param>
        /// <param name="parent">The Roslyn node that represents this grouping of results.</param>
        public static TranslationResult GetHighlevelSequence(IEnumerable<TranslationResult> results, SyntaxNode parent = null)
        {
            TranslationResult result = new TranslationResult();
            HighlevelSequenceSilvernode sequence = new HighlevelSequenceSilvernode(parent);
            foreach(var incoming in results)
            {
                result.Errors.AddRange(incoming.Errors);
                if (incoming.Silvernode != null)
                {
                    sequence.List.Add(incoming.Silvernode);
                }
            }
            result.Silvernode = sequence;
            return result;
        }

        /// <summary>
        /// Combines the errors from all the arguments and returns them.
        /// </summary>
        /// <param name="results">TranslationResult instances that may contain errors.</param>
        internal static IEnumerable<Error> CombineErrors(params TranslationResult[] results)
        {
           return results.SelectMany((result) => result.Errors);
        }
    }
}
