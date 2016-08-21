using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation
{
    public class IdentifierDeclaration : Identifier
    {
        private IdentifierTranslator identifierTranslator;
        public TaggedSymbol Symbol;
         
        public IdentifierDeclaration(TaggedSymbol symbol, IdentifierTranslator identifierTranslator)
        {
            this.Symbol = symbol;
            this.identifierTranslator = identifierTranslator;
        }

        public string Silvername { get; set; }

        public override string ToString()
        {
            return Silvername;
        }
    }
}
