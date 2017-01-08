using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    class InvocationFoldingUnfolding : InvocationTranslation
    {
        private bool _isFolding;

        public InvocationFoldingUnfolding(bool isFolding)
        {
            this._isFolding = isFolding;
        }

        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            var expressions = ConvertToSilver(arguments, context);
            Silvernode = new SimpleSequenceSilvernode(originalNode,
                _isFolding ? "folding" : "unfolding",
                " ",
                expressions[0],
                " in ",
                expressions[1]
                );
        }
    }
}
