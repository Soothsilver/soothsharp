using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.Trees.CSharp
{
    public abstract class Sharpnode
    {
        /// <summary>
        /// Gets the Roslyn syntax node that corresponds to this sharpnode.
        /// </summary>
        public SyntaxNode OriginalNode { get; private set; }

        protected Sharpnode(SyntaxNode originalNode)
        {
            this.OriginalNode = originalNode;
        }

        public abstract TranslationResult Translate(TranslationContext context);

        /// <summary>
        /// Collects all C# types from all children and adds them to the collection in the <see cref="TranslationProcess"/>. If this node
        /// is a type declaration, this node is collected as well. If this node is a class declaration, its instance fields are also processed.
        /// </summary>
        /// <param name="translationProcess">The process to add the types into.</param>
        public virtual void CollectTypesInto(TranslationProcess translationProcess)
        {
            // For most sharpnodes, do nothing.
        }
    }
}
