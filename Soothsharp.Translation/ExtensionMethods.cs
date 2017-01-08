using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
namespace Soothsharp.Translation
{
    /// <summary>
    /// Containts various useful extension methods used in this assembly.
    /// </summary>
    public static class ExtensionMethods
    {
        private static readonly SymbolDisplayFormat qualifiedNameDisplayFormat =
            new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                memberOptions: SymbolDisplayMemberOptions.IncludeContainingType);

        private static readonly SymbolDisplayFormat withoutNamespacesNameDisplayFormat =
            new SymbolDisplayFormat(
               
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes,
                memberOptions: SymbolDisplayMemberOptions.IncludeContainingType);

        private static readonly SymbolDisplayFormat simpleSymbolFormat =
            new SymbolDisplayFormat(
                parameterOptions: SymbolDisplayParameterOptions.IncludeName
                );

        /// <summary>
        /// Gets the fully qualified name of the C# symbol that includes namespaces (for example, System.Int32) .
        /// </summary>
        /// <param name="symbol">The symbol to get the name of.</param>
        /// <returns>The fully qualified name of the symbol.</returns>
        public static string GetQualifiedName(this ISymbol symbol)
        {
            return symbol.ToDisplayString(ExtensionMethods.qualifiedNameDisplayFormat);
        } 
        /// <summary>
        /// Gets a partially qualified name of the C# symbol that does not include namespaces (for example, Int32) .
        /// </summary>
        /// <param name="symbol">The symbol to get the name of.</param>
        /// <returns>The fully qualified name of the symbol.</returns>
        public static string GetNameWithoutNamespaces(this ISymbol symbol)
        {
            return symbol.ToDisplayString(ExtensionMethods.withoutNamespacesNameDisplayFormat);
        }
        public static string GetSimpleName(this ISymbol symbol)
        {
            return symbol.ToDisplayString(ExtensionMethods.simpleSymbolFormat);
        }

        /// <summary>
        /// Creates a new <see cref="IEnumerable{T}"/> from the existing one, but puts a separator element between each two elements. This method does not use ToString() at all.
        /// </summary>
        /// <param name="sequence">The original <see cref="IEnumerable{T}"/>.</param>
        /// <param name="separator">The separator element.</param>
        /// <returns></returns>
        public static IEnumerable<T> WithSeparator<T>(this IEnumerable<T> sequence, T separator)
        {
            int i = 0;
            int count = sequence.Count();
            foreach (var t in sequence)
            {
                yield return t;
                if (i != count - 1)
                {
                    yield return separator;
                }
                i++;
            }
        }
    }
}
