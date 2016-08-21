using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.Silver;
using System.Collections.Generic;
using Sharpsilver.Translation;
using Sharpsilver.Translation.Trees.CSharp.Expressions;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation.Trees.CSharp
{
    public class ObjectCreationExpressionSharpnode : ExpressionSharpnode
    {
        public List<ExpressionSharpnode> Arguments = new List<ExpressionSharpnode>();

        public ObjectCreationExpressionSharpnode(ObjectCreationExpressionSyntax syntax) : base(syntax)
        {
            foreach (var argument in syntax.ArgumentList.Arguments)
            {
                this.Arguments.Add(RoslynToSharpnode.MapExpression(argument.Expression));
                // TODO name:colon, ref/out...
            }
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            return TranslationResult.FromSilvernode(
                new NewSilvernode(this.OriginalNode)
                );
        }

    }
}