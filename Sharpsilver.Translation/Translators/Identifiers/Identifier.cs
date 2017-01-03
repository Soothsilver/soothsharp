namespace Soothsharp.Translation
{
    public class Identifier
    {
        public IdentifierTranslator IdentifierTranslator { get; set; }
        public TaggedSymbol Symbol { get; set; }


        public Identifier(TaggedSymbol symbol, IdentifierTranslator identifierTranslator)
        {
            this.Symbol = symbol;
            this.IdentifierTranslator = identifierTranslator;
        }
        public Identifier(string silvername)
        {
            this.Silvername = silvername;
        }

        public string Silvername { get; set; } = SILVERNAME_NOT_YET_ASSIGNED;

        public const string SILVERNAME_NOT_YET_ASSIGNED = "!silvername-not-yet-assigned!";

        public override string ToString()
        {
            return Silvername;
        }
    }
}
