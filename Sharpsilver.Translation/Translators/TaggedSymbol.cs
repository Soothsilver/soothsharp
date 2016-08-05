using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Translators
{
    public class TaggedSymbol
    {
        public TaggedSymbol(ISymbol symbol, string tag)
        {
            this.Symbol = symbol;
            this.Tag = tag;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TaggedSymbol)) return false;
            TaggedSymbol other = (TaggedSymbol)obj;
            return this.Symbol == other.Symbol &&
                   this.Tag == other.Tag;
        }

        protected bool Equals(TaggedSymbol other)
        {
            return Equals(this.Symbol, other.Symbol) && string.Equals(this.Tag, other.Tag);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Symbol?.GetHashCode() ?? 0)*397) ^ (this.Tag?.GetHashCode() ?? 0);
            }
        }


        public ISymbol Symbol { get; }
        public string Tag { get; }
    }
}