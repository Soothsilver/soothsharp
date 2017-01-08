using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    public class DirectSilvercodeExpressionSharpnode : ExpressionSharpnode
    {
        private string Code;
        public DirectSilvercodeExpressionSharpnode(string code, ExpressionSyntax originalNode) : base(originalNode)
        {
            this.Code = code;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            return TranslationResult.FromSilvernode(this.Code);
        }
    }
}