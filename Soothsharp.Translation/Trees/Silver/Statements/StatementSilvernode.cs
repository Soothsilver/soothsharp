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
            this.HowManyTabsAfterEachNewline = tablevel;
            foreach (var child in this.Children)
            {
                (child as StatementSilvernode)?.Postprocess(tablevel);
            }
        }


        /// <summary>
        /// Creates the proper number of spaces that should be added before this silvernode so that
        /// the resulting Viper code is properly indented.
        /// </summary>
        protected string Tabs(int add = 0)
        {
            string tabs = "";
            for (int i = 0; i < ((this.HowManyTabsAfterEachNewline + add) * Constants.SpacesPerIndentLevel); i++)
            {
                tabs += " ";
            }
            return tabs;
        }

        /// <summary>
        /// Unless this is a <see cref="BlockSilvernode"/>, returns a Viper block that contains this statement.
        /// If this is already a block, it returns itself.
        /// </summary>
        public virtual BlockSilvernode EncloseInBlockIfNotAlready()
        {
            return new BlockSilvernode(null, new List<StatementSilvernode> {this});
        }
    }
}
