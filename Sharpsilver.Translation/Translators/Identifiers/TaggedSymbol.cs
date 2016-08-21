using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Translators
{
    /// <summary>
    /// For the purposes of <see cref="IdentifierTranslator"/>. 
    /// This class puts together a Roslyn symbol and a custom string tag that may possibly be empty. When silvernames are
    /// determined, it is guaranteed that all tagged symbols with the same symbol and tag will evaluate into the same
    /// silvername but that no other item will evaluate into that silvername.
    /// </summary>
    public class TaggedSymbol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaggedSymbol"/> class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="tag">The tag.</param>
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


        /// <summary>
        /// Gets the source Roslyn symbol.
        /// </summary>
        public ISymbol Symbol { get; }
        /// <summary>
        /// Gets the additional tag that may be an empty string.
        /// </summary>
        public string Tag { get; }
    }
}