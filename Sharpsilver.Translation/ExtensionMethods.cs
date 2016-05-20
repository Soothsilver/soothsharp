using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
namespace Sharpsilver.Translation
{
    /// <summary>
    /// Containts various useful extension methods used in this assembly.
    /// </summary>
    public static class ExtensionMethods
    {
        public static readonly SymbolDisplayFormat QualifiedNameDisplayFormat =
            new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces, 
                memberOptions: SymbolDisplayMemberOptions.IncludeContainingType);

        /// <summary>
        /// Gets the fully qualified name of the C# symbol (for example, System.Int32).
        /// </summary>
        /// <param name="symbol">The symbol to get the name of.</param>
        /// <returns>The fully qualified name of the symbol.</returns>
        public static string GetQualifiedName(this ISymbol symbol)
        {
            return symbol.ToDisplayString(QualifiedNameDisplayFormat);            
        }
        /// <summary>
        /// Increases line indent by one tab to the right for all lines in the input.
        /// </summary>
        /// <param name="input">Text that should be tabified to the right.</param>
        /// <returns>The input, with each line indented one more tab to the right.</returns>
        public static string AscendTab(this string input)
        {
            string[] lines = input.Split('\n'); 
            return String.Join("\n", lines.Select(line => "\t" + line));
        }
    }
}
