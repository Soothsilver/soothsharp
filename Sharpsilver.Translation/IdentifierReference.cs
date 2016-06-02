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
        private ISymbol method;

        public IdentifierReference(ISymbol method, IdentifierTranslator identifierTranslator)
        {
            this.method = method;
            this.identifierTranslator = identifierTranslator;
        }

        public string Silvername { get; set; }

        public override string ToString()
        {
            return Silvername;
        }
    }
}
