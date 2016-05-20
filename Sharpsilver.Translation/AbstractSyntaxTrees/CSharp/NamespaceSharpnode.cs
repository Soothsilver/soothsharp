using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System.Collections.Generic;
using System.Linq;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    internal class NamespaceSharpnode : Sharpnode
    {
        private List<Sharpnode> Children = new List<Sharpnode>();
        private NamespaceSharpnode ParentNamespace;

        public NamespaceSharpnode(NamespaceDeclarationSyntax node, NamespaceSharpnode parent = null) : base(node)
        {
            ParentNamespace = parent;
            Children.AddRange(node.Usings.Select(usng => new UsingDirectiveSharpnode(usng)));
            Children.AddRange(node.Members.Select<MemberDeclarationSyntax, Sharpnode>(
              mbr =>
              {
                  switch (mbr.Kind())
                  {
                      case Microsoft.CodeAnalysis.CSharp.SyntaxKind.NamespaceDeclaration:
                          return new NamespaceSharpnode(mbr as NamespaceDeclarationSyntax, this);
                      case Microsoft.CodeAnalysis.CSharp.SyntaxKind.ClassDeclaration:
                          return new ClassSharpnode(mbr as ClassDeclarationSyntax, this);
                      case Microsoft.CodeAnalysis.CSharp.SyntaxKind.StructDeclaration:
                          return new UnknownSharpnode(mbr);
                      default:
                          return new UnexpectedSharpnode(mbr);
                  }
              }));
        }

        public override TranslationResult Translate(TranslationContext translationContext)
        {
            return CommonUtils.CombineResults(Children.Select(child =>
            {
                return child.Translate(translationContext);
            }
            ), OriginalNode);
        }
    }
}