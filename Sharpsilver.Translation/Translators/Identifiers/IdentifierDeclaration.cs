using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation
{
    public class IdentifierDeclaration : Identifier
    {
         
        public IdentifierDeclaration(TaggedSymbol symbol, IdentifierTranslator identifierTranslator)
        {
            this.Symbol = symbol;
            this.identifierTranslator = identifierTranslator;
        }
        

        public override string ToString()
        {
            return Silvername;
        }
    }
}
