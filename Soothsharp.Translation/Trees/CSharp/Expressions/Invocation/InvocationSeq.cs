using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Contracts;
using Soothsharp.Translation.Trees.CSharp.Expressions;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    /// <summary>
    /// Base class for invocation styles based on <see cref="Seq{T}"/>.
    /// </summary>
    /// <seealso cref="Soothsharp.Translation.Trees.CSharp.Invocation.InvocationTranslation" />
    abstract class InvocationSeq : InvocationTranslation
    {
        protected ExpressionSyntax MethodGroup;
        protected ExpressionSharpnode MethodGroupSharpnode;

        /// <summary>
        /// Adds, as the zeroeth element, the method call's receiver to the list of arguments in the parameter.
        /// </summary>
        /// <param name="arguments">The arguments of the method call.</param>
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
        }
    }
}
