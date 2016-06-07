using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.Translators
{
    /// <summary>
    /// Allows declarations to register new identifiers, and variables/method calls to get existing identifiers.
    /// </summary>
    public class IdentifierTranslator
    {
        private static string[] SilverKeywords = new[]
        {
            "import",
            "domain",
            "unique",
            "function",
            "axiom",
            "field",
            "requires",
            "ensures",
            "invariant",
            "predicate",
            "returns",
            "new",
            "var",
            "assert",
            "assume",
            "inhale",
            "exhale",
            "fold",
            "unfold",
            "goto",
            "if",
            "elsif",
            "else",
            "fresh",
            "constraining",
            "union",
            "intersection",
            "setminus",
            "in",
            "subset",
            "true",
            "false",
            "null",
            "result",
            "old",
            "none",
            "write",
            "epsilon",
            "wildcard",
            "perm",
            "unfolding",
            "forall",
            "exists",
            "Seq",
            "Set",
            "Multiset",
            "acc",
            "Int",
            "Bool",
            "Perm",
            "Ref"
        };
        private readonly Dictionary<ISymbol, IdentifierDeclaration> registeredGlobalSymbols =
            new Dictionary<ISymbol, IdentifierDeclaration>();
        private readonly Dictionary<ISymbol, IdentifierReference> references = new Dictionary<ISymbol, IdentifierReference>();

        private readonly List<string> usedSilverIdentifiers = new List<string>
        {
            Constants.SilverMethodEndLabel,
            Constants.SilverReturnVariableName,
            ""
        }.Union(SilverKeywords).ToList();

        // TODO register local symbols, later on, when import mechanisms and local syntax in Silver are more clear to me

        private string silverize(string identifier)
        {
            char[] charArray = identifier.Replace('.', '_').ToCharArray();
            char[] updatedArray =
                charArray.Where(
                    ch => (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9') || ch == '_')
                    .ToArray();
            return new string(updatedArray);
        }

        public IdentifierDeclaration RegisterAndGetIdentifier(ISymbol method)
        {
            IdentifierDeclaration identifier = new IdentifierDeclaration(method, this);
            registeredGlobalSymbols.Add(method, identifier);
            return identifier;
        }

        public IdentifierReference GetIdentifierReference(ISymbol method)
        {
            if (references.ContainsKey(method))
            {
                return references[method];
            }
            IdentifierReference reference = new IdentifierReference(method, this);
            references.Add(method, reference);
            return reference;
        }

        public string RegisterNewUniqueIdentifier()
        {
            return "_id";
        }

        public void AssignTrueNames()
        {
            foreach (var kvp in registeredGlobalSymbols)
            {
                ISymbol symbol = kvp.Value.Symbol;
                string baseSilverName = silverize(symbol.GetNameWithoutNamespaces());
                if (kvp.Key is IParameterSymbol)
                {
                    baseSilverName = symbol.GetSimpleName();
                }
                string silverName = baseSilverName;
                int i = 2;
                while (usedSilverIdentifiers.Contains(silverName))
                {
                    silverName = baseSilverName + i;
                    i++;
                }
                usedSilverIdentifiers.Add(silverName);
                kvp.Value.Silvername = silverName;
            }
            foreach (var kvp in references)
            {
                if (registeredGlobalSymbols.ContainsKey(kvp.Key))
                {
                    kvp.Value.Silvername = registeredGlobalSymbols[kvp.Key].Silvername;
                }
                else
                {
                    // TODO report the error (if it can occur at all? ...probably only in syntax-problematic code or in otherwise unsupported code)
                    kvp.Value.Silvername = Constants.SilverErrorString;
                }
            }
        }
    }
}
