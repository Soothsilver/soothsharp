using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation
{
    public class IdentifierReference : Identifier
    {
        private IdentifierTranslator identifierTranslator;
        private TaggedSymbol symbol;

        public IdentifierReference(TaggedSymbol symbol, IdentifierTranslator identifierTranslator)
        {
            this.symbol = symbol;
            this.identifierTranslator = identifierTranslator;
        }

        /// <summary>
        /// Creates a reference directly from a Silver name.
        /// </summary>
        public IdentifierReference(string silvername)
        {
            this.Silvername = silvername;
        }

        public string Silvername { get; set; }

        public override string ToString()
        {
            return Silvername;
        }
    }
}
