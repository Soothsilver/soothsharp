using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Highlevel
{
    internal class ClassSharpnode : Sharpnode
    {
        private List<Sharpnode> children = new List<Sharpnode>();
        

        public ClassSharpnode(ClassDeclarationSyntax node) : base(node)
        {
            children.AddRange(node.Members.Select(mbr => RoslynToSharpnode.MapClassMember(mbr)));
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var classSymbol = context.Process.SemanticModel.GetDeclaredSymbol(OriginalNode as ClassDeclarationSyntax);

            var attributes = classSymbol.GetAttributes();
            
            if (attributes.Any(attribute => attribute.AttributeClass.GetQualifiedName() == ContractsTranslator.UnverifiedAttribute))
            {
                return TranslationResult.FromSilvernode(new SinglelineCommentSilvernode($"Class {classSymbol.GetQualifiedName()} skipped because it was marked [Unverified].", OriginalNode));
            }
            return CommonUtils.GetHighlevelSequence(children.Select(child => child.Translate(context)));
        }
    }
}