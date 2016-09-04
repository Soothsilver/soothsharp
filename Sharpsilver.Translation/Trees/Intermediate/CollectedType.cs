using System.Collections.Generic;
using Sharpsilver.Translation.Trees.Silver;

namespace Sharpsilver.Translation
{
    public class CollectedType
    {
        public Identifier Name;
        public Identifier Supertype;
        public List<CollectedField> InstanceFields = new List<CollectedField>();

        public CollectedType(Identifier name, Identifier supertype)
        {
            Name = name;
            Supertype = supertype;
        }

        public Silvernode GenerateGlobalSilvernode()
        {
            
            var node = new SimpleSequenceSilvernode(null);

            foreach (CollectedField field in InstanceFields)
            {
                node.List.Add("field ");
                node.List.Add(new IdentifierSilvernode(field.Name));
                node.List.Add(": ");
                node.List.Add(new TypeSilvernode(null, field.SilverType));
                if (InstanceFields[InstanceFields.Count - 1] != field)
                {
                    node.List.Add("\n");
                }
            }
            return node;
        }

        public Silvernode GenerateSilvernodeInsideCSharpType()
        {
            return new SimpleSequenceSilvernode(null,
                "\tunique function ",
                new IdentifierSilvernode(Name),
                "() : ",
                Constants.CSharpTypeDomain
                );
        }
    }
}
