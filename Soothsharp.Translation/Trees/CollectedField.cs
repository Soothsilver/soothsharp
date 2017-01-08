namespace Soothsharp.Translation
{
    public class CollectedField
    {
        public Identifier Name;
        public SilverType SilverType;
        
        public CollectedField(Identifier name, SilverType silverType)
        {
            this.Name = name;
            this.SilverType = silverType;
        }
    }
}
