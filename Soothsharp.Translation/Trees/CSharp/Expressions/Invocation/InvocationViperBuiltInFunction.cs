using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    class InvocationViperBuiltInFunction : InvocationSubroutine
    {
        private string _keyword;

        public InvocationViperBuiltInFunction(string keyword, bool isImpure)
        {
            this._keyword = keyword;
            Impure = isImpure;
        }

        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            var expressions = ConvertToSilver(arguments, context);
            var identifier = new Identifier(_keyword);
            SilverType = SilverType.Bool;

            Silvernode = new CallSilvernode(
                       identifier,
                       expressions,
                       SilverType,
                       originalNode
                   );
        }
    }
}
