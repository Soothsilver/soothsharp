using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    class CompilationUnitSharpnode : Sharpnode
    {
        private List<Sharpnode> Children = new List<Sharpnode>();

        public CompilationUnitSharpnode(CompilationUnitSyntax node) : base(node)
        {
            Children.AddRange(node.Usings.Select(uds => new UsingDirectiveSharpnode(uds)));
            Children.AddRange(node.Members.Select<MemberDeclarationSyntax, Sharpnode>(
                mbr => 
                (mbr is NamespaceDeclarationSyntax 
                ? 
                (Sharpnode)new NamespaceSharpnode(mbr as NamespaceDeclarationSyntax) 
                :
                (Sharpnode)new UnexpectedSharpnode(mbr)
                )
                ));
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var results = Children.Select(child => child.Translate(context));
            return CommonUtils.CombineResults(results, OriginalNode);
        }
    }
}
