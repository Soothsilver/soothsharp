using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation
{
    public class IdentifierTranslator
    {
        private List<string> RegisteredIdentifiers = new List<string>
        {
            Constants.SILVER_RETURN_VARIABLE_NAME
        };  

        private string Silverize(string identifier)
        {
            return identifier
                .Replace('.', '_')
                ;
        }
        public string RegisterAndGetIdentifier(IMethodSymbol method)
        {
            string newIdentifier = Silverize(method.ContainingType.GetQualifiedName() + "." +  method.Name);
            // TODO this won't do for locals
            if (RegisteredIdentifiers.Contains(newIdentifier))
            {
                int i = 2;
                while (RegisteredIdentifiers.Contains(newIdentifier + i.ToString()))
                {
                    i++;
                }
                newIdentifier = newIdentifier + i.ToString();
            }
            RegisteredIdentifiers.Add(newIdentifier);
            return newIdentifier;
        }
    }
}
