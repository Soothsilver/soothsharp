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
        public TranslationResult TranslateNode(SyntaxNode node, TranslationContext context)
        {
            switch(node.Kind())
            {
                case SyntaxKind.SwitchStatement:

                    return
                        TranslationResult.Error("CSSIL501", "The 'switch' statement is not supported.", node);

                case SyntaxKind.UsingDirective:
                    Console.WriteLine("Using Directive.");
                    return new TranslationResult(new SilverSourceCode("// Using directive used: " +
                        (node as UsingDirectiveSyntax).GetText(), node));
                default:
                    List<TranslationResult> results = new List<TranslationResult>();
                    foreach(var child in node.ChildNodes())
                    {
                        Console.WriteLine("Entering " + child.Kind());
                        TranslationResult result = TranslateNode(child, new TranslationContext());
                        results.Add(result);

                    }
                    return TranslationResult.Combine(results);
            }
        }
        public TranslationResult Translate()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(Code);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var compilation = CSharpCompilation.Create("translated_assembly")
                                    .AddSyntaxTrees(tree)
                                    ;
            return TranslateNode(root, new TranslationContext());
        }
    }
}
