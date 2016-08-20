using System.Collections.Generic;
using Sharpsilver.Translation.Trees.Silver;

namespace Sharpsilver.Translation.Trees.Intermediate
{
    public class CollectedType
    {
        public Identifier Name;
        public Identifier Supertype;
        public List<Identifier> InstanceFields = new List<Identifier>();

        public CollectedType(Identifier name, Identifier supertype)
        {
            Name = name;
            Supertype = supertype;
        }

        public Silvernode GenerateGlobalSilvernode()
        {
            var node = new SimpleSequenceSilvernode(null,
                "function ",
                new IdentifierSilvernode(Name),
                "(): ",
                Constants.CSharpTypeDomain,
                "\n");
            foreach (Identifier identifier in InstanceFields)
            {
                node.List.Add("field ");
                node.List.Add(new IdentifierSilvernode(identifier));
                node.List.Add(": ");
                node.List.Add("Int\n");
            }
            return node;
        }

        public Silvernode GenerateSilvernodeInsideCSharpType()
        {
            return new SimpleSequenceSilvernode(null,
                ""
                );
        }
    }
}
