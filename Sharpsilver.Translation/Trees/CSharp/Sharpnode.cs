using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.CSharp
{
    /// <summary>
    /// Represents a node in the C# abstract tree.
    /// </summary>
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

        /// <summary>
        /// Converts this sharpnode into a silvernode and a list of errors.
        /// </summary>
        /// <param name="context">Translation context (such as whether we're in a pure context).</param>
        /// <returns></returns>
        public abstract TranslationResult Translate(TranslationContext context);

        /// <summary>
        /// Collects all C# types from all children and adds them to the collection in the <see cref="TranslationProcess"/>. If this node
        /// is a type declaration, this node is collected as well. If this node is a class declaration, its instance fields are also processed.
        /// </summary>
        /// <param name="translationProcess">The process to add the types into.</param>
        /// <param name="semantics">Semantic model of the current tree that is being translated.</param>
        public virtual void CollectTypesInto(TranslationProcess translationProcess, SemanticModel semantics)
        {
            // For most sharpnodes, do nothing.
        }
    }
}
