using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    class InvocationPermissionCreate : InvocationTranslation
    {

        public InvocationPermissionCreate() 
        {
        }

        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            var expressions = ConvertToSilver(arguments, context);
            Silvernode = new BinaryExpressionSilvernode(
                expressions[0],
                "/",
                expressions[1],
                originalNode);
        }
    }
}
