using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Translators;
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
            this.eaes = syntax;
            this.Container = RoslynToSharpnode.MapExpression(syntax.Expression);
            // Only single-dimensional arrays are supported.
            this.Index = RoslynToSharpnode.MapExpression(syntax.ArgumentList.Arguments[0].Expression);

        }

        public override TranslationResult Translate(TranslationContext context)
        {
            SymbolInfo symbolInfo = context.Semantics.GetSymbolInfo(this.eaes);
            ISymbol symbol = symbolInfo.Symbol;
            string accessorName = symbol?.GetQualifiedName();
            var errors = new List<Error>();
            var container = this.Container.Translate(context);
            var index = this.Index.Translate(context.ChangePurityContext(PurityContext.Purifiable));
            errors.AddRange(container.Errors);
            errors.AddRange(index.Errors);
            // TODO purifiable
            if (accessorName == SeqTranslator.SeqAccess)
            {
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
                var typeInfo = context.Semantics.GetTypeInfo(Container.OriginalNode);
                var t = typeInfo.Type;
                if (t.Kind == SymbolKind.ArrayType)
                {
                    // ASSUME READ
                    var readsilvernode = context.Process.ArraysTranslator.ArrayRead(this.OriginalNode, container.Silvernode,
                        index.Silvernode); 
                    TranslationResult read = TranslationResult.FromSilvernode(readsilvernode, errors);
                    read.Arrays_Container = container;
                    read.Arrays_Index = index;
                    return read; 
                }
                else
                {
                    //      if (ssContainer as IExpressionSym
                    return TranslationResult.Error(this.OriginalNode,
                        Diagnostics.SSIL128_IndexersAreOnlyForSeqsAndArrays);
                }
            }
        }
    }
}
