using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    public abstract class StatementSilvernode : ComplexSilvernode
    {
        protected int HowManyTabsAfterEachNewline;

        protected StatementSilvernode(SyntaxNode statement) : base(statement)
        {
        }

        public override void Postprocess()
        {
            Postprocess(0);
        }

        public virtual void Postprocess(int tablevel)
        {
            HowManyTabsAfterEachNewline = tablevel;
            foreach (var child in Children)
            {
                (child as StatementSilvernode)?.Postprocess(tablevel);
            }
        }

        protected string Tabs()
        {
            string tabs = "";
            for (int i = 0; i < HowManyTabsAfterEachNewline; i++)
            {
                tabs += "\t";
            }
            return tabs;
        }

        public virtual BlockSilvernode EncloseInBlockIfNotAlready()
        {
            return new BlockSilvernode(null, new List<StatementSilvernode> {this});
        }
    }
}
