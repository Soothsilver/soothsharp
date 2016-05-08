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
    public static class ExtensionMethods
    {
        public static readonly SymbolDisplayFormat QUALIFIED_NAME_DISPLAY_FORMAT =
            new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

        public static string GetQualifiedName(this INamedTypeSymbol symbol)
        {
            return symbol.ToDisplayString(QUALIFIED_NAME_DISPLAY_FORMAT);            
        }
    }
}
