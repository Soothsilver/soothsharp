namespace Sharpsilver.Translation
{
    public class CollectedField
    {
        public Identifier Name;
        public SilverType SilverType;
        
        public CollectedField(Identifier name, SilverType silverType)
        {
            Name = name;
            SilverType = silverType;
        }
    }
}
