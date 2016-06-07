using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Highlevel
{
    class CompilationUnitSharpnode : Sharpnode
    {
        private List<Sharpnode> children = new List<Sharpnode>();

        public CompilationUnitSharpnode(CompilationUnitSyntax node) : base(node)
        {
            // Using directives ignored.
            children.AddRange(node.Members.Select(
                mbr => mbr is NamespaceDeclarationSyntax
                    ? new NamespaceSharpnode((NamespaceDeclarationSyntax) mbr) as Sharpnode
                    : new UnexpectedSharpnode(mbr) as Sharpnode));
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var results = children.Select(child => child.Translate(context));
            return CommonUtils.GetHighlevelSequence(results, OriginalNode);
        }
    }
}
