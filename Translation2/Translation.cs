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
            var mscorlib = MetadataReference.CreateFromFile(typeof(System.Attribute).Assembly.Location);
            Compilation = CSharpCompilation.Create("translated_assembly")
                                    .AddSyntaxTrees(tree)
                                    .AddReferences(mscorlib)
                                    .AddReferences(MetadataReference.CreateFromFile("Sharpsilver.Contracts.dll"))
                                    ;
            SemanticModel = Compilation.GetSemanticModel(tree, true);

            Sharpnode cSharpTree;
            try
            {
                cSharpTree = RoslynToSharpnode.Map(root);
            }
            catch (Exception ex)
            {
                return TranslationResult.Error(null, Diagnostics.SSIL103_ExceptionConstructingCSharp, ex.GetType().ToString());
            }
            try
            {
                TranslationResult translationResult = cSharpTree.Translate(TranslationContext.StartNew(this));
                return translationResult;
            }
            catch (Exception ex)
            {

                return TranslationResult.Error(null, Diagnostics.SSIL104_ExceptionConstructingSilver, ex.GetType().ToString());
            }
        }
    }
}
