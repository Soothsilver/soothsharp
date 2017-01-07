using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Soothsharp.Translation.Trees.Silver
{
    public class BlockSilvernode : StatementSilvernode
    {
        public List<StatementSilvernode> Statements;
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
            this.Statements = statements;
        }

        public override IEnumerable<Silvernode> Children
        {
            get
            {
                if (Statements.Count != 0)
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
                } else
                {
                    yield return "{}";
                }
            }
        }


        protected override void OptimizePre()
        {
            for(int i = 0; i < Statements.Count; i++)
            {
                StatementSilvernode thisStatement = Statements[i];

                // Blocks cannot happen with blocks. Remove nested blocks (deeply)!
                if (thisStatement is BlockSilvernode)
                {
                    BlockSilvernode block = (BlockSilvernode)thisStatement;
                    Statements.RemoveAt(i);
                    Statements.InsertRange(i, block.Statements);
                    i--;
                }
                else if (thisStatement is StatementsSequenceSilvernode)
                {
                    StatementsSequenceSilvernode sequence = (StatementsSequenceSilvernode)thisStatement;
                    sequence.OptimizeRecursively();
                    List<StatementSilvernode> points = sequence.List;
                    Statements.RemoveAt(i);
                    Statements.InsertRange(i, points);
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
            HowManyTabsAfterEachNewline = level;
            foreach (StatementSilvernode s in Statements)
            {
                s.Postprocess(level + 1);
            }
        }
    }
}