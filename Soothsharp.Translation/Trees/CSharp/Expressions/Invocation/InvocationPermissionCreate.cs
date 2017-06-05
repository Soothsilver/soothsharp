using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Contracts;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    /// <summary>
    /// Translates <see cref="Permission.Create(int, int)"/> to "num/denom". 
    /// </summary>
    /// <seealso cref="Soothsharp.Translation.Trees.CSharp.Invocation.InvocationTranslation" />
    class InvocationPermissionCreate : InvocationTranslation
    {
        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            var expressions = ConvertToSilver(arguments, context);
            this.Silvernode = new BinaryExpressionSilvernode(
                expressions[0],
                "/",
                expressions[1],
                originalNode);
        }
    }
}
