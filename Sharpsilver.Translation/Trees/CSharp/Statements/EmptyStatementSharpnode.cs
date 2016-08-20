using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Translation.Trees.Silver;

namespace Sharpsilver.Translation.Trees.CSharp.Statements
{
    class EmptyStatementSharpnode : StatementSharpnode
    {
        public EmptyStatementSharpnode(EmptyStatementSyntax syntax) : base(syntax)
        {
            
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            return TranslationResult.FromSilvernode(new TextSilvernode("", OriginalNode));
        }
    }
}
