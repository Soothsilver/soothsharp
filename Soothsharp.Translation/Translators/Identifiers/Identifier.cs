using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    /// <summary>
    /// Represents an identifier that has to be translated into a Viper identifier. Identifiers should
    /// never be in Silver text directly, but as <see cref="IdentifierSilvernode"/> that refer to this
    /// class, so that the name assignment phase can work correctly.
    /// </summary>
    public class Identifier
    {
        /// <summary>
        /// Gets the information used to translate the identifier, i.e. the Roslyn symbol and a tag.
        /// </summary>
        public TaggedSymbol Symbol { get; private set; }


        public Identifier(TaggedSymbol symbol)
        {
            this.Symbol = symbol;
        }
        /// <summary>
        /// Creates an identifier from Silver text directly, without going through the name assignment phase.
        /// </summary>
        /// <param name="silvername">The Silver text that replaces the identifier.</param>
        public Identifier(string silvername)
        {
            this.Silvername = silvername;
        }

        // If this remains unassigned, then there is an error in name assignment.
        public string Silvername { get; set; } = SILVERNAME_NOT_YET_ASSIGNED;

        private const string SILVERNAME_NOT_YET_ASSIGNED = "!silvername-not-yet-assigned!";

        public override string ToString()
        {
            return this.Silvername;
        }
    }
}
