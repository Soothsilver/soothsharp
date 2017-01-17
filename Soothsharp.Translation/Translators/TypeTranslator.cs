using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation
{
    static class TypeTranslator
    {
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
            if (typeSymbol == null)
            {
                error = new Error(Diagnostics.SSIL204_OtherLocalizedError, where, "Unknown type (is the type declared in a file submitted to Soothsharp?)");
                return SilverType.Error;
            }
            switch(typeSymbol.GetQualifiedName())
            {
                case "System.Int32":
                    return SilverType.Int;
                case "System.Boolean":
                    return SilverType.Bool;
                case "System.Single":
                case "System.Double":
                case "System.Decimal":
                case "System.Char":
                case "System.Byte":
                    error = new Error(Diagnostics.SSIL106_TypeNotSupported, where, typeSymbol.GetQualifiedName());
                    return SilverType.Error;
                case "System.Int16":
                case "System.UInt16":
                case "System.UInt32":
                case "System.Int64":
                case "System.UInt64":
                    error = new Error(Diagnostics.SSIL115_ThisIntegerSizeNotSupported, where, typeSymbol.GetQualifiedName());
                    return SilverType.Error;
                case ContractsTranslator.PermissionType:
                    return SilverType.Perm;
                case "System.Void":
                    return SilverType.Void;
                    
                case SeqTranslator.SeqClassWithoutEndDot:
                    INamedTypeSymbol namedType = (INamedTypeSymbol)typeSymbol;
                    var firstTypeArgument = namedType.TypeArguments[0];
                    return SilverType.Seq(TypeTranslator.TranslateType(firstTypeArgument, where, out error));
                default:
                    if (typeSymbol.TypeKind == TypeKind.Enum)
                    {
                        return SilverType.Int;
                    }
                    return SilverType.Ref;
            }
        }
    }
}
