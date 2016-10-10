using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.CSharp
{
    public abstract class StatementSharpnode : Sharpnode
    {

        protected StatementSharpnode(SyntaxNode stmt) : base(stmt)
        {

        }

    }
}