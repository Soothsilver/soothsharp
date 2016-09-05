using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sharpsilver.Translation.Trees.Silver.Statements
{
    public class BlockSilvernode : StatementSilvernode
    {
        private BlockSyntax blockSyntax;
        private List<StatementSilvernode> Statements;
        public void Add(StatementSilvernode statement)
        {
            Statements.Add(statement);
        }
        public void Prepend(StatementSilvernode statement)
        {
            Statements.Insert(0, statement);
        }

        public BlockSilvernode(BlockSyntax blockSyntax, List<StatementSilvernode> statements) : base(blockSyntax)
        {
            this.blockSyntax = blockSyntax;
            this.Statements = statements;
        }

        public override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return "{\n";
                yield return Tabs() + "\t";

                foreach (var a in Statements.WithSeparator<Silvernode>(new TextSilvernode("\n" + Tabs() + "\t")))
                {
                    yield return a;
                }

                yield return "\n";
                yield return Tabs();
                yield return "}";
            }
        }

        public override BlockSilvernode EncloseInBlockIfNotAlready()
        {
            return this;
        }

        public override void Postprocess(int level)
        {
            HowManyTabsAfterEachNewline = level;
            foreach (StatementSilvernode s in Statements)
            {
                s.Postprocess(level + 1);
            }
        }
    }
}