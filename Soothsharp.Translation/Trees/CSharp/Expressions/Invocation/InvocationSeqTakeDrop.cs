using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.CSharp.Expressions;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    class InvocationSeqTakeDrop : InvocationSeq
    {
        private bool _doTake;
        private bool _doDrop;


        public InvocationSeqTakeDrop(bool doTake, bool doDrop, ExpressionSyntax methodGroup, ExpressionSharpnode methodGroupSharpnode)
        {
            this._doTake = doTake;
            this._doDrop = doDrop;
            this.MethodGroup = methodGroup;
            this.MethodGroupSharpnode = methodGroupSharpnode;
        }

        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            AddReceiverToList(arguments);
            var expressions = ConvertToSilver(arguments, context);

            // Determine what argument corresponds to what action - there are three scenarios
            Silvernode dropAmount = "";
            Silvernode takeAmount = "";
            if (this._doTake && this._doDrop)
            {
                takeAmount = expressions[2];
                dropAmount = expressions[1];
            }
            else if (this._doTake)
            {
                takeAmount = expressions[1];
            }
            else if (this._doDrop)
            {
                dropAmount = expressions[1];
            }
            else
            {
                throw new Exception("Never happens.");
            }
            this.Silvernode = new SimpleSequenceSilvernode(originalNode, 
                expressions[0],
                "[",
                dropAmount,
                "..",
                takeAmount,
                "]"
                );
        }
    }
}
