using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    class InvocationSeqContains : InvocationSeq
    {
        public InvocationSeqContains(ExpressionSyntax methodGroup, ExpressionSharpnode methodGroupSharpnode)
        {
            this._methodGroup = methodGroup;
            this._methodGroupSharpnode = methodGroupSharpnode;
        }

        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            AddReceiverToList(arguments);
            var expressions = ConvertToSilver(arguments, context);
            Silvernode = new SimpleSequenceSilvernode(originalNode,
                expressions[1],
                " in ",
                expressions[0]);
        }
    }
}
