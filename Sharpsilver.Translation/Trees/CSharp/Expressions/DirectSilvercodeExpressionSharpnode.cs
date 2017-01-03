using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    public class DirectSilvercodeExpressionSharpnode : ExpressionSharpnode
    {
        public string Code;
        public DirectSilvercodeExpressionSharpnode(string code, ExpressionSyntax originalNode) : base(originalNode)
        {
            this.Code = code;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            return TranslationResult.FromSilvernode(Code);
        }
    }
}