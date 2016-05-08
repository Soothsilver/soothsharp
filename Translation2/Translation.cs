using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Translators;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;

namespace Sharpsilver.Translation
{
    public class TranslationProcess
    {
        public CSharpCompilation Compilation;
        public SemanticModel SemanticModel;

        public TranslationProcess()
        {
        }
        public TranslationResult TranslateCode(string csharpCode)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(csharpCode);
            return TranslateTree(tree);
        }


        public TranslationResult TranslateTree(SyntaxTree tree)
        {
            var root = (CompilationUnitSyntax)tree.GetRoot();
            Compilation = CSharpCompilation.Create("translated_assembly")
                                    .AddSyntaxTrees(tree)
                                    ;
            SemanticModel = Compilation.GetSemanticModel(tree, true);

            Sharpnode cSharpTree = RoslynToSharpnode.Map(root);
            TranslationResult translationResult = cSharpTree.Translate(TranslationContext.StartNew(this));
            return translationResult;
        }
    }
}
