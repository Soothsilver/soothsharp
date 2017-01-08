using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.CSharp.Expressions;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    abstract class InvocationSeq : InvocationTranslation
    {
        protected ExpressionSyntax MethodGroup;
        protected ExpressionSharpnode MethodGroupSharpnode;

        protected void AddReceiverToList(List<ExpressionSharpnode> arguments)
        {
            if (this.MethodGroupSharpnode is IdentifierExpressionSharpnode)
            {
                arguments.Insert(0, new DirectSilvercodeExpressionSharpnode(Constants.SilverThis, this.MethodGroup));
            }
            else if (this.MethodGroupSharpnode is MemberAccessExpressionSharpnode)
            {
                arguments.Insert(0, ((MemberAccessExpressionSharpnode) this.MethodGroupSharpnode).Container);
            }
            else
            {
                this.Errors.Add(new Error(Diagnostics.SSIL102_UnexpectedNode, this.MethodGroup, this.MethodGroup.Kind()));
            }

            Error error = null;
            if (error != null)
            {
                this.Errors.Add(error);
            }
        }
    }
}
