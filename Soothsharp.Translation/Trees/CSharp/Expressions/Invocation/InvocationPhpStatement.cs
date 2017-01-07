using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
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
            Silvernode = new AssertionLikeSilvernode(_keyword, expressions[0], originalNode);
        }
    }
}
