using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver.Statements
{
    class LocalVariableDeclarationSilvernode : StatementSilvernode
    {
        public string Identifier { get;  }
        public SilverType Type { get; }

        public LocalVariableDeclarationSilvernode(string identifier, SilverType type, SyntaxNode originalNode) : base(originalNode)
        {
            Identifier = identifier;
            Type = type;
        }

        public override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return "var ";
                yield return Identifier;
                yield return " : ";
                yield return TypeTranslator.SilverTypeToString(Type);
                yield return ";";
            }
        }
    }
}
