using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Sharpsilver.Translation.Trees.Silver
{
    /// <summary>
    /// Represents a node in the Silver syntax tree.
    /// </summary>
    public abstract class Silvernode
    {
        /// <summary>
        /// The C# syntax node this silvernode comes from, or null, if this silvernode is not associated to a C# node. This is used to 
        /// connect potential errors in this silvernode back to the C# source.
        /// </summary>
        public SyntaxNode OriginalNode;
        /// <summary>
        /// Gets the children of this silvernode, if any. All <see cref="ComplexSilvernode"/>s have children, but other silvernodes cannot.
        /// </summary>
        public virtual IEnumerable<Silvernode> Children => new Silvernode[0];

        /// <summary>
        /// Gets the C# node that was translated into the deepmost silvernode that is present at the specified character offset off the 
        /// beginning of this silvernode.
        /// <para>
        /// For example, if the offset is 5, this method will find the silvernode that has some text at the sixth character of this silvernode.
        /// The deepmost silvernode may not have an associated C# node. In that case, we ascend the tree and the deepmost silvernode that actually
        /// has an associated C# node is selected.
        /// </para>
        /// <para>
        /// It is an error to call this method with an offset that is not present in this silvernode.
        /// </para>
        /// </summary>
        /// <param name="offset">The character offset at which we should recover the C# node</param>
        /// <returns>The C# node associated with the silvernode at the given offset</returns>
        public SyntaxNode GetSyntaxNodeFromOffset(int offset)
        {
            if (offset >= this.Size) throw new ArgumentException("The offset must be within this node.", nameof(offset));
            int curoffset = offset;
            foreach (var child in Children)
            {
                if (child.Size > curoffset)
                {
                    SyntaxNode childNode =  child.GetSyntaxNodeFromOffset(curoffset);
                    if (childNode == null)
                    {
                        return this.OriginalNode;
                    }
                    else
                    {
                        return childNode;
                    }
                }
                curoffset -= child.Size;
            }
            return this.OriginalNode;
        }

        /// <summary>
        /// Gets the length, in characters, of this silvernode.
        /// </summary>
        protected int Size => ToString().Length;

        /// <summary>
        /// Returns a value that indicates whether this silvernode represents a contract ("requires", "ensures" or "invariant").
        /// </summary>
        public virtual bool IsVerificationCondition()
        {
            return false;
        }

        /// <summary>
        /// Initializes a new <see cref="Silvernode"/>. 
        /// </summary>
        /// <param name="originalNode">The associated C# node, or null.</param>
        protected Silvernode(SyntaxNode originalNode)
        {
            OriginalNode = originalNode;
        }

        /// <summary>
        /// Returns the Silver code text that silvernode evaluates to.
        /// </summary>
        public abstract override string ToString();   

        /// <summary>
        /// Postprocessing may modify silvernodes. Postprocessing includes procedures such as indenting code blocks.
        /// </summary>
        public virtual void Postprocess()
        {
            foreach (var child in Children)
            {
                child.Postprocess();
            }
        }

        /// <summary>
        /// This implicit operator converts a string to a simple text silvernode. This is a useful shortcut in complex translations.
        /// </summary>
        /// <param name="s">The string that will be output into the Silver source.</param>
        public static implicit operator Silvernode(string s)
        {
            return new TextSilvernode(s);
        }

        /// <summary>
        /// Runs the OPTIMIZATION PHASE for this silvernode, BEFORE optimization on child nodes is run.
        /// </summary>
        protected virtual void OptimizePre()
        {
        }
        /// <summary>
        /// Runs the OPTIMIZATION PHASE for this silvernode, AFTER optimization on child nodes is run.
        /// </summary>
        protected virtual void OptimizePost()
        {

        }

        /// <summary>
        /// Runs the OPTIMIZATION PHASE for this silvernode, then calls itself on its children.
        /// </summary>
        public void OptimizeRecursively()
        {
            this.OptimizePre();
            foreach (var child in this.Children)
            {
                child.OptimizeRecursively();
            }
            this.OptimizePost();
        }

        /// <summary>
        /// Performs the specified actions on this node and all descendant nodes recursively.
        /// </summary>
        /// <param name="action">The action to perform on this node and each descendant.</param>
        public void Recurse(Action<Silvernode> action)
        {
            action(this);
            foreach (var child in this.Children)
            {
                child.Recurse(action);
            }
        }
        /// <summary>
        /// Gets all descedants, recursively, of this silvernode, excluding this node itself.
        /// </summary>
        public IEnumerable<Silvernode> Descendants
        {
            get {
                foreach (var child in this.Children)
                {
                    yield return child;
                    foreach (var descendant in child.Descendants)
                    {
                        yield return descendant;
                    }
                }
            }
        }
    }
}
