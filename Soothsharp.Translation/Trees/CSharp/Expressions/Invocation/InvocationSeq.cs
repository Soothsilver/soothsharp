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
        protected ExpressionSyntax _methodGroup;
        protected ExpressionSharpnode _methodGroupSharpnode;

        protected void AddReceiverToList(List<ExpressionSharpnode> arguments)
        {
            if (this._methodGroupSharpnode is IdentifierExpressionSharpnode)
            {
                arguments.Insert(0, new DirectSilvercodeExpressionSharpnode(Constants.SilverThis, this._methodGroup));
            }
            else if (this._methodGroupSharpnode is MemberAccessExpressionSharpnode)
            {
                arguments.Insert(0, ((MemberAccessExpressionSharpnode) this._methodGroupSharpnode).Container);
            }
            else
            {
                this.Errors.Add(new Error(Diagnostics.SSIL102_UnexpectedNode, this._methodGroup, this._methodGroup.Kind()));
            }

            Error error = null;
            if (error != null)
            {
                this.Errors.Add(error);
            }
        }
    }
}
