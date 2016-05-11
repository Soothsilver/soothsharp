using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    internal class BlockSilvernode : Silvernode
    {
        private BlockSyntax blockSyntax;
        private List<Silvernode> Statements;
        public void Add(Silvernode statement)
        {
            Statements.Add(statement);
        }

        public BlockSilvernode(BlockSyntax blockSyntax, List<Silvernode> statements) : base(blockSyntax)
        {
            this.blockSyntax = blockSyntax;
            this.Statements = statements;
        }

        public override string ToString()
        {
            return "{\n"
                + string.Join("\n", Statements.Select(stmt => stmt.ToString().AscendTab()))
                + "\n}"
                ;
        }
    }
}