using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using Soothsharp.Translation.Translators;

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
        // These words are Viper keywords, and so they can't be used as identifiers directly.
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
            "Ref",
            "res",
            "this",
            ArraysTranslator.IntegerArrayContents,
            ArraysTranslator.IntegerArrayRead,
            ArraysTranslator.IntegerArrayWrite,
            ArraysTranslator.IntegerArrayAccess
        };
        private readonly Dictionary<TaggedSymbol, Identifier> registeredGlobalSymbols = new Dictionary<TaggedSymbol, Identifier>();
        private readonly Dictionary<TaggedSymbol, Identifier> references = new Dictionary<TaggedSymbol, Identifier>();

        private readonly List<string> usedSilverIdentifiers = new List<string>
        {
            Constants.SilverMethodEndLabel,
            Constants.SilverReturnVariableName,
            Constants.SilverSystemObject,
            Constants.SilverThis,
            ""
        }.Union(silverKeywords).ToList();

        /// <summary>
        /// Silverizes the specified identifier (see thesis) by removing characters invalid in Viper.
        /// </summary>
        /// <param name="identifier">The identifier to remove invalid characters from.</param>
        private static string Silverize(string identifier)
        {
            char[] charArray = identifier.Replace('.', '_').ToCharArray();
            char[] updatedArray =
                charArray.Where(
                    ch => (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9') || ch == '_')
                    .ToArray();
            return new string(updatedArray);
        }

        /// <summary>
        /// Adds a new identifier to the identifier database that corresponds to a C# symbol, and returns it.
        /// It is guaranteed to be distinct from other identifiers.
        /// </summary>
        /// <param name="symbol">The symbol to register as a Viper identifier.</param>
        public Identifier RegisterAndGetIdentifier(ISymbol symbol)
        {
            return RegisterAndGetIdentifierWithTag(symbol, "");
        }
        /// <summary>
        /// Adds a new identifier to the identifier database that corresponds to a C# symbol,
        /// adds a tag, and returns the identifier.
        /// It is guaranteed to be distinct from other identifiers.
        /// </summary>
        /// <param name="classSymbol">The class symbol.</param>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public Identifier RegisterAndGetIdentifierWithTag(ISymbol classSymbol, string tag)
        {
            var taggedSymbol = new TaggedSymbol(classSymbol, tag);
            Identifier identifier = new Identifier(taggedSymbol);
            this.registeredGlobalSymbols.Add(taggedSymbol, identifier);
            return identifier;
        }

        /// <summary>
        /// Gets the identifier that corresponds to a C# symbol. If it doesn't exist, it is created.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public Identifier GetIdentifierReference(ISymbol symbol)
        {
            return GetIdentifierReferenceWithTag(symbol, "");
        }

        /// <summary>
        /// Gets the identifier that corresponds to a C# symbol and tag. If it doesn't exist, it is created.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="tag">The tag.</param>
        public Identifier GetIdentifierReferenceWithTag(ISymbol symbol, string tag)
        {
            var taggedSymbol = new TaggedSymbol(symbol, tag);
            if (this.references.ContainsKey(taggedSymbol))
            {
                return this.references[taggedSymbol];
            }
            Identifier reference = new Identifier(taggedSymbol);
            this.references.Add(taggedSymbol, reference); 
            return reference;
        }


        private int temporaryIdentifiersRegisteredCount;

        /// <summary>
        /// Registers a new unique identifier, used for temporary variables needed by Viper. Guaranteed to 
        /// be distinct from other identifiers.
        /// </summary>
        public Identifier RegisterNewUniqueIdentifier()
        {
            this.temporaryIdentifiersRegisteredCount++;
            return RegisterAndGetIdentifierWithTag(null, "tmp" + this.temporaryIdentifiersRegisteredCount);
        }

        /// <summary>
        /// Performs the name assignment phase, as per the thesis.
        /// </summary>
        public void AssignTrueNames()
        {
            // First, for declarations:
            foreach (var kvp in this.registeredGlobalSymbols)
            {
                // Determine the base silvername.
                ISymbol symbol = kvp.Value.Symbol.Symbol;
                string baseSilverName = "";
                if (symbol != null)
                {
                    baseSilverName = Silverize(symbol.GetNameWithoutNamespaces());
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

                // Add a number to the end to maintain uniqueness of identifiers.
                int i = 2;
                while (this.usedSilverIdentifiers.Contains(silverName))
                {
                    silverName = baseSilverName + i;
                    i++;
                }
                this.usedSilverIdentifiers.Add(silverName);
                kvp.Value.Silvername = silverName;
            }

            // Second, use declarations in references:
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
