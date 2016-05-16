using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sharpsilver.Translation
{
    static class TypeTranslator
    {
        internal static string TranslateTypeToString(ITypeSymbol typeSymbol, TypeSyntax where, out Error error)
        {
            return TypeTranslator.SilverTypeToString(TranslateType(typeSymbol, where, out error));
        }
        /// <summary>
        /// Translates a C# type into a Silver type.
        /// </summary>
        /// <param name="typeSymbol">A C# type.</param>
        /// <param name="where">A syntaxnode for error diagnostic if the type is not supported in Silver.</param>
        /// <param name="error">This error is triggered if the type is not supported in Silver.</param>
        /// <returns>Silver name for the type.</returns>
        public static SilverType TranslateType(ITypeSymbol typeSymbol, SyntaxNode where, out Error error)
        {
            error = null;
            switch(typeSymbol.GetQualifiedName())
            {
                case "System.Int32":
                    return SilverType.Int;
                case "System.Boolean":
                    return SilverType.Bool;
                case "System.Single":
                case "System.Double":
                case "System.Int64":
                    // TODO others
                    // throw error
                    error =  new Error(Diagnostics.SSIL106_TypeNotSupported, where, typeSymbol.GetQualifiedName());
                    return SilverType.Error;
                case "System.Void":
                    return SilverType.Void;
                default:
                    return SilverType.Ref;
            }
        }

        internal static string SilverTypeToString(SilverType silverType)
        {
            switch (silverType)
            {
                case SilverType.Bool: return "Bool";
                case SilverType.Int: return "Int";
                case SilverType.Void: return Constants.SILVER_ERROR_STRING;
                default:
                    return Constants.SILVER_ERROR_STRING;
            }
        }

       
    }
}
