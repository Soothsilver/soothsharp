using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cs2Sil.Translation
{
    public class TranslationProcess
    {
        string Code;

        public TranslationProcess(string csharpCode)
        {
            Code = csharpCode;
        }
        public TranslationResult Translate()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(Code);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var compilation = CSharpCompilation.Create("translated_assembly")
                                    .AddSyntaxTrees(tree)
                                    ;
            return new TranslationResult();
        }
    }
}
