namespace Soothsharp.Translation
{
    /// <summary>
    /// Represents a field of a C# class that is to be translated as a Viper field.
    /// </summary>
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
