using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sharpsilver.Translation
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
                case SyntaxKind.UsingDirective:
                    return new TranslationResult(new SilverSourceCode("// Using directive used.", node));
                case SyntaxKind.CompilationUnit:
                case SyntaxKind.NamespaceDeclaration:
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.MethodDeclaration:
                case SyntaxKind.Block:
                    List<TranslationResult> results = new List<TranslationResult>();
                    foreach (var child in node.ChildNodes())
                    {
                        Console.WriteLine("Entering " + child.Kind());
                        TranslationResult result = TranslateNode(child, new TranslationContext());
                        results.Add(result);

                    }
                    return TranslationResult.Combine(results);
                default:
                    return TranslationResult.Error(node, Diagnostics.SSIL101, node.Kind());
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
