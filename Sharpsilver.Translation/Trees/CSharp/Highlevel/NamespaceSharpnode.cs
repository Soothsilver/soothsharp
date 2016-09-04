using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sharpsilver.Translation.Trees.CSharp.Highlevel
{
    internal class NamespaceSharpnode : Sharpnode
    {
        private List<Sharpnode> children = new List<Sharpnode>();

        public NamespaceSharpnode(NamespaceDeclarationSyntax node) : base(node)
        {
            this.children.AddRange(node.Members.Select<MemberDeclarationSyntax, Sharpnode>(
              mbr =>
              {
                  // ReSharper disable once SwitchStatementMissingSomeCases
                  switch (mbr.Kind())
                  {
                      case Microsoft.CodeAnalysis.CSharp.SyntaxKind.NamespaceDeclaration:
                          return new NamespaceSharpnode(mbr as NamespaceDeclarationSyntax);
                      case Microsoft.CodeAnalysis.CSharp.SyntaxKind.ClassDeclaration:
                          return new ClassSharpnode(mbr as ClassDeclarationSyntax);
                      case Microsoft.CodeAnalysis.CSharp.SyntaxKind.StructDeclaration:
                          return new DiagnosticSharpnode(mbr, Diagnostics.SSIL108_FeatureNotSupported, "structs");
                      case Microsoft.CodeAnalysis.CSharp.SyntaxKind.EnumDeclaration:
                          return new DiagnosticSharpnode(mbr, Diagnostics.SSIL105_FeatureNotYetSupported, "enums");
                      default:
                          return new UnexpectedSharpnode(mbr);
                  }
              }));
        }

        public override TranslationResult Translate(TranslationContext translationContext)
        {
            return CommonUtils.GetHighlevelSequence(this.children.Select(child => child.Translate(translationContext)), this.OriginalNode);
        }

        public override void CollectTypesInto(TranslationProcess translationProcess, SemanticModel semantics)
        {
            foreach (Sharpnode sharpnode in this.children)
            {
                sharpnode.CollectTypesInto(translationProcess, semantics);
            }
        }
    }
}