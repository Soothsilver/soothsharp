using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sharpsilver.Translation.Trees.CSharp
{
    public abstract class ExpressionSharpnode : Sharpnode
    {
        protected ExpressionSharpnode(ExpressionSyntax syntax) : base(syntax)
        {

        }
    }
}