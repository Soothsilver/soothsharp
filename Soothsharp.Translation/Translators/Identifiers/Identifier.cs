namespace Soothsharp.Translation
{
    public class Identifier
    {
        public TaggedSymbol Symbol { get; private set; }


        public Identifier(TaggedSymbol symbol)
        {
            this.Symbol = symbol;
        }
        public Identifier(string silvername)
        {
            this.Silvername = silvername;
        }

        public string Silvername { get; set; } = SILVERNAME_NOT_YET_ASSIGNED;

        private const string SILVERNAME_NOT_YET_ASSIGNED = "!silvername-not-yet-assigned!";

        public override string ToString()
        {
            return this.Silvername;
        }
    }
}
