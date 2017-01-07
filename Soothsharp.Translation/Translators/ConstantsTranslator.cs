using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    public class ConstantsTranslator
    {
        public TranslationResult TranslateIdentifierAsConstant(ISymbol symbol, SyntaxNode syntaxNode, TranslationContext context)
        {
            if (symbol is IFieldSymbol)
            {
                IFieldSymbol field = (IFieldSymbol) symbol;
                if (field.IsConst && field.HasConstantValue)
                {
                    return
                        TranslationResult.FromSilvernode(new TextSilvernode(constantToString(field.ConstantValue), syntaxNode));
                }
            }
            return null;
        }

        private string constantToString(object constantValue)
        {
            if (constantValue is int)
            {
                return constantValue.ToString();
            }
            else if (constantValue is bool)
            {
                if ((bool) constantValue) return "true";
                else return "false";
            }
            else if (constantValue == null)
            {
                return "null";
            }
            else
            {
                return Constants.SilverErrorString;
            }
        }
    }
}
