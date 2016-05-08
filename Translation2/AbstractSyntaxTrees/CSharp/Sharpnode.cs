using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    public abstract class Sharpnode
    {
        public SyntaxNode OriginalNode;

        protected Sharpnode(SyntaxNode originalNode)
        {
            OriginalNode = originalNode;
        }

        public abstract TranslationResult Translate(TranslationContext translationContext);
    }
}
