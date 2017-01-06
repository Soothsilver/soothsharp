using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp
{
    class ElementAccessSharpnode : ExpressionSharpnode
    {
        private ElementAccessExpressionSyntax eaes;
        private ExpressionSharpnode Container;
        private ExpressionSharpnode Index;
        public ElementAccessSharpnode(ElementAccessExpressionSyntax syntax) : base(syntax)
        {
            eaes = syntax;
            Container = RoslynToSharpnode.MapExpression(syntax.Expression);
            // TODO multiple indicies
            Index = RoslynToSharpnode.MapExpression(syntax.ArgumentList.Arguments[0].Expression);

        }

        public override TranslationResult Translate(TranslationContext context)
        {
            SymbolInfo symbolInfo = context.Semantics.GetSymbolInfo(this.eaes);
            ISymbol symbol = symbolInfo.Symbol;
            string accessorName = symbol.GetQualifiedName();
            var errors = new List<Error>();
            var container = Container.Translate(context);
            var index = Index.Translate(context.ChangePurityContext(PurityContext.Purifiable));
            // TODO purifiable
            if (accessorName == SeqTranslator.SeqAccess)
            {
                errors.AddRange(container.Errors);
                errors.AddRange(index.Errors);
                return TranslationResult.FromSilvernode(
                    new SimpleSequenceSilvernode(this.OriginalNode,
                        container.Silvernode,
                        "[",
                        index.Silvernode,
                        "]"
                      ), errors
                    );
            }
            else
            {
                return TranslationResult.Error(this.OriginalNode,
                    Diagnostics.SSIL128_IndexersAreOnlyForSeqsAndArrays);
            }
        }
    }
}
