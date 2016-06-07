using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;
using System.Threading.Tasks;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver.Statements;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    public abstract class Silvernode
    {
        public SyntaxNode OriginalNode;
        public SyntaxToken OriginalToken;
        public SyntaxTrivia OriginalTrivia;
        protected int ColumnOffset;
        protected virtual IEnumerable<Silvernode> Children => new Silvernode[0];

        public Silvernode GetSilvernodeFromOffset(int offset)
        {
            int curoffset = offset;
            foreach (var child in Children)
            {
                if (child.Size > curoffset)
                {
                    return child.GetSilvernodeFromOffset(curoffset);
                }
                curoffset -= child.Size;
            }
            return this;
        }

        protected int Size => ToString().Length;

        public virtual bool IsVerificationCondition()
        {
            return false;
        }

        protected Silvernode(SyntaxNode originalNode)
        {
            OriginalNode = originalNode;
        }
        protected Silvernode(SyntaxToken originalToken)
        {
            OriginalToken = originalToken;
        }
        protected Silvernode(SyntaxTrivia originalTrivia)
        {
            OriginalTrivia = originalTrivia;
        }

        public abstract override string ToString();

   

        public virtual void Postprocess()
        {
            foreach (var child in Children)
            {
                child.Postprocess();
            }
        }

        public static implicit operator Silvernode(string s)
        {
            return new TextSilvernode(s);
        }
    }
}
