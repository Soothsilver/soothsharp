using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation
{
    static class TypeTranslator
    {
        public static string TranslateType(ITypeSymbol typeSymbol, SyntaxNode where, out Error error)
        {
            error = null;
            switch(typeSymbol.GetQualifiedName())
            {
                case "System.Int32":
                    return "Int";
                case "System.Boolean":
                    return "Bool";

                case "System.Single":
                case "System.Double":
                case "System.Int64":
                    // TODO others
                    // throw error
                    error =  new Error(Diagnostics.SSIL106_TypeNotSupported, where, typeSymbol.GetQualifiedName());
                    return Constants.SILVER_ERROR_STRING;
                default:
                    return "Ref";
            }
        }
    }
}
