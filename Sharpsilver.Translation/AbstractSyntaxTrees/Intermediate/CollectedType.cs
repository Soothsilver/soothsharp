using System.Collections.Generic;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Intermediate
{
    class CollectedType
    {
        public Identifier Name;
        public Identifier Supertype;
        public List<Identifier> InstanceFields = new List<Identifier>();
    }
}
