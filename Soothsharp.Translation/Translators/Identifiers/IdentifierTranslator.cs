using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Soothsharp.Translation
{
    /// <summary>
    /// Allows declarations to register new identifiers, and variables/method calls to get existing identifiers.
    /// </summary>
    public class IdentifierTranslator
    {
        private TranslationProcess process;
        public IdentifierTranslator(TranslationProcess process)
        {
            this.process = process;
        }
        private static string[] silverKeywords = new[]
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
        private readonly Dictionary<TaggedSymbol, Identifier> registeredGlobalSymbols = new Dictionary<TaggedSymbol, Identifier>();
        private readonly Dictionary<TaggedSymbol, Identifier> references = new Dictionary<TaggedSymbol, Identifier>();

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
        }.Union(IdentifierTranslator.silverKeywords).ToList();

        // TODO register local symbols, later on, when import mechanisms and local syntax in Silver are more clear to me

        private static string Silverize(string identifier)
        {
            char[] charArray = identifier.Replace('.', '_').ToCharArray();
            char[] updatedArray =
                charArray.Where(
                    ch => (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9') || ch == '_')
                    .ToArray();
            return new string(updatedArray);
        }

        public Identifier RegisterAndGetIdentifier(ISymbol method)
        {
            return RegisterAndGetIdentifierWithTag(method, "");
        }
        public Identifier RegisterAndGetIdentifierWithTag(ISymbol classSymbol, string tag)
        {
            var taggedSymbol = new TaggedSymbol(classSymbol, tag);
            Identifier identifier = new Identifier(taggedSymbol);
            this.registeredGlobalSymbols.Add(taggedSymbol, identifier);
            return identifier;
        }

        public Identifier GetIdentifierReference(ISymbol method)
        {
            return GetIdentifierReferenceWithTag(method, "");
        }
        public Identifier GetIdentifierReferenceWithTag(ISymbol method, string tag)
        {
            var taggedSymbol = new TaggedSymbol(method, tag);
            if (this.references.ContainsKey(taggedSymbol))
            {
                return this.references[taggedSymbol];
            }
            Identifier reference = new Identifier(taggedSymbol);
            this.references.Add(taggedSymbol, reference);
            return reference;
        }


        private int temporaryIdentifiersRegisteredCount;
        public Identifier RegisterNewUniqueIdentifier()
        {
            this.temporaryIdentifiersRegisteredCount++;
            return RegisterAndGetIdentifierWithTag(null, "tmp" + this.temporaryIdentifiersRegisteredCount);
        }

        public void AssignTrueNames()
        {
            foreach (var kvp in this.registeredGlobalSymbols)
            {
                ISymbol symbol = kvp.Value.Symbol.Symbol;
                string baseSilverName = "";
                if (symbol != null)
                {
                    baseSilverName = IdentifierTranslator.Silverize(symbol.GetNameWithoutNamespaces());
                    if (kvp.Key.Symbol is IParameterSymbol)
                    {
                        baseSilverName = symbol.GetSimpleName();
                    }
                }
                if (kvp.Key.Tag != "")
                {
                    baseSilverName += "_" + kvp.Key.Tag;
                }
                string silverName = baseSilverName;
                int i = 2;
                while (this.usedSilverIdentifiers.Contains(silverName))
                {
                    silverName = baseSilverName + i;
                    i++;
                }
                this.usedSilverIdentifiers.Add(silverName);
                kvp.Value.Silvername = silverName;
            }
            foreach (var kvp in this.references)
            {
                if (this.registeredGlobalSymbols.ContainsKey(kvp.Key))
                {
                    kvp.Value.Silvername = this.registeredGlobalSymbols[kvp.Key].Silvername;
                }
                else
                {
                    this.process.AddError(new Error(Diagnostics.SSIL120_UndeclaredNameReferenced, null, kvp.Key.Symbol +
                        (String.IsNullOrEmpty(kvp.Key.Tag) ? "" : "_" + kvp.Key.Tag)));
                }
            }
        }

     
    }
}
