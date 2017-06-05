using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    /// <summary>
    /// Translates methods into statements such as 'assert [something]' or 'inhale [something]'.
    /// </summary>
    /// <remarks>
    /// This class is named "PhpStaement" because these statements remind me of PHP's echo and print, in that they
    /// don't use parentheses.
    /// </remarks>
    /// <seealso cref="Soothsharp.Translation.Trees.CSharp.Invocation.InvocationTranslation" />
    class InvocationPhpStatement : InvocationTranslation
    {
        private string _keyword;

        public InvocationPhpStatement(string keyword) 
        {
            this._keyword = keyword;
        }

        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            var expressions = ConvertToSilver(arguments, context);
            this.Silvernode = new AssertionLikeSilvernode(this._keyword, expressions[0], originalNode);
        }
    }
}
