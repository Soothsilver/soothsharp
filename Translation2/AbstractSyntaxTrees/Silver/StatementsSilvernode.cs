using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    class StatementsSilvernode : Silvernode
    {
        private List<Silvernode> Statements = new List<Silvernode>();

        public void Add(Silvernode statement)
        {
            Statements.Add(statement);
        }

        public StatementsSilvernode(SyntaxNode original) : base(original)
        {

        }

        public override string ToString()
        {
            return String.Join("\n", Statements);
        }
    }
}
