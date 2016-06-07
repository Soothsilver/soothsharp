using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Highlevel
{
    internal class NamespaceSharpnode : Sharpnode
    {
        private List<Sharpnode> children = new List<Sharpnode>();

        public NamespaceSharpnode(NamespaceDeclarationSyntax node) : base(node)
        {
            children.AddRange(node.Members.Select<MemberDeclarationSyntax, Sharpnode>(
              mbr =>
              {
                  switch (mbr.Kind())
                  {
                      case Microsoft.CodeAnalysis.CSharp.SyntaxKind.NamespaceDeclaration:
                          return new NamespaceSharpnode(mbr as NamespaceDeclarationSyntax);
                      case Microsoft.CodeAnalysis.CSharp.SyntaxKind.ClassDeclaration:
                          return new ClassSharpnode(mbr as ClassDeclarationSyntax);
                      case Microsoft.CodeAnalysis.CSharp.SyntaxKind.StructDeclaration:
                          return new DiagnosticSharpnode(mbr, Diagnostics.SSIL108_FeatureNotSupported, "structs");
                      default:
                          return new UnexpectedSharpnode(mbr);
                  }
              }));
        }

        public override TranslationResult Translate(TranslationContext translationContext)
        {
            return CommonUtils.GetHighlevelSequence(children.Select(child => child.Translate(translationContext)), OriginalNode);
        }
    }
}