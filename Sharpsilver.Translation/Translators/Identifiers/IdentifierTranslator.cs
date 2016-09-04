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
        private readonly Dictionary<TaggedSymbol, IdentifierDeclaration> registeredGlobalSymbols = new Dictionary<TaggedSymbol, IdentifierDeclaration>();
        private readonly Dictionary<TaggedSymbol, IdentifierReference> references = new Dictionary<TaggedSymbol, IdentifierReference>();
        public readonly IdentifierReference SystemObject = new IdentifierReference(Constants.SilverSystemObject);

        private readonly List<string> usedSilverIdentifiers = new List<string>
        {
            Constants.SilverMethodEndLabel,
            Constants.SilverReturnVariableName,
            Constants.SilverSystemObject,
            Constants.TypeOfFunction,
            Constants.IsSubTypeFunction,
            Constants.CSharpTypeDomain,
            Constants.SilverThis,
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
            return RegisterAndGetIdentifierWithTag(method, "");
        }
        public IdentifierDeclaration RegisterAndGetIdentifierWithTag(ISymbol classSymbol, string init)
        {
            var taggedSymbol = new TaggedSymbol(classSymbol, init);
            IdentifierDeclaration identifier = new IdentifierDeclaration(taggedSymbol, this);
            registeredGlobalSymbols.Add(taggedSymbol, identifier);
            return identifier;
        }

        public IdentifierReference GetIdentifierReference(ISymbol method)
        {
            return GetIdentifierReferenceWithTag(method, "");
        }
        public IdentifierReference GetIdentifierReferenceWithTag(ISymbol method, string tag)
        {
            var taggedSymbol = new TaggedSymbol(method, tag);
            if (references.ContainsKey(taggedSymbol))
            {
                return references[taggedSymbol];
            }
            IdentifierReference reference = new IdentifierReference(taggedSymbol, this);
            references.Add(taggedSymbol, reference);
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
                ISymbol symbol = kvp.Value.Symbol.Symbol;
                string baseSilverName = silverize(symbol.GetNameWithoutNamespaces());
                if (kvp.Key.Symbol is IParameterSymbol)
                {
                    baseSilverName = symbol.GetSimpleName();
                }
                if (kvp.Key.Tag != "")
                {
                    baseSilverName += "_" + kvp.Key.Tag;
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
