using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Soothsharp.Translation.Trees.Silver
{
    public class BlockSilvernode : StatementSilvernode
    {
        public List<StatementSilvernode> Statements;
        public void Add(StatementSilvernode statement)
        {
            this.Statements.Add(statement);
        }
        public void Prepend(StatementSilvernode statement)
        {
            this.Statements.Insert(0, statement);
        }

        public BlockSilvernode(BlockSyntax blockSyntax, List<StatementSilvernode> statements) : base(blockSyntax)
        {
            this.Statements = statements;
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                if (this.Statements.Count != 0)
                {
                    yield return "{\n";
                    yield return Tabs(1);

                    foreach (var a in this.Statements.WithSeparator<Silvernode>(new TextSilvernode("\n" + Tabs(1))))
                    {
                        yield return a;
                    }

                    yield return "\n";
                    yield return Tabs();
                    yield return "}";
                } else
                {
                    yield return "{}";
                }
            }
        }


        protected override void OptimizePre()
        {
            for(int i = 0; i < this.Statements.Count; i++)
            {
                StatementSilvernode thisStatement = this.Statements[i];

                // Blocks cannot happen with blocks. Remove nested blocks (deeply)!
                if (thisStatement is BlockSilvernode)
                {
                    BlockSilvernode block = (BlockSilvernode)thisStatement;
                    this.Statements.RemoveAt(i);
                    this.Statements.InsertRange(i, block.Statements);
                    i--;
                }
                else if (thisStatement is StatementsSequenceSilvernode)
                {
                    StatementsSequenceSilvernode sequence = (StatementsSequenceSilvernode)thisStatement;
                    sequence.OptimizeRecursively();
                    List<StatementSilvernode> points = sequence.List;
                    this.Statements.RemoveAt(i);
                    this.Statements.InsertRange(i, points);
                    i--;
                }
            }
        }

        public override BlockSilvernode EncloseInBlockIfNotAlready()
        {
            return this;
        }

        public override void Postprocess(int level)
        {
            this.HowManyTabsAfterEachNewline = level;
            foreach (StatementSilvernode s in this.Statements)
            {
                s.Postprocess(level + 1);
            }
        }
    }
}