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
            this.MethodGroup = methodGroup;
            this.MethodGroupSharpnode = methodGroupSharpnode;
        }

        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            AddReceiverToList(arguments);
            var expressions = ConvertToSilver(arguments, context);
            this.Silvernode = new SimpleSequenceSilvernode(originalNode,
                expressions[1], // the element of the sequence
                " in ",
                expressions[0]); // the sequence (previously the receiver)
        }
    }
}
