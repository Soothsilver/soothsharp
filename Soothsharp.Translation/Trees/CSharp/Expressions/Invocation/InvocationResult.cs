using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    class InvocationResult : InvocationTranslation
    {
        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            string text = context.IsFunctionOrPredicateBlock ? "result" : Constants.SilverReturnVariableName;
            Silvernode = new TextSilvernode(text, originalNode);
        }
    }
}
