using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    class LocalVariableDeclarationSilvernode : Silvernode
    {
        public string Identifier { get; private set; }
        public SilverType Type { get; private set; }

        public LocalVariableDeclarationSilvernode(string identifier, SilverType type, SyntaxNode originalNode) : base(originalNode)
        {
            Identifier = identifier;
            Type = type;
        }

        public override string ToString()
        {
            return "var " + Identifier + " : " + TypeTranslator.SilverTypeToString(Type) + ";";
        }
    }
}
