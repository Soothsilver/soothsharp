using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System.Collections.Generic;
using System.Linq;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    internal class ClassSharpnode : Sharpnode
    {
        private List<Sharpnode> Children = new List<Sharpnode>();
        private NamespaceSharpnode ParentNamespace;

        public ClassSharpnode(ClassDeclarationSyntax node, NamespaceSharpnode parent) : base(node)
        {
            ParentNamespace = parent;
        }

        public override TranslationResult Translate(TranslationContext translationContext)
        {
            return TranslationResult.Silvernode(new SinglelineCommentSilvernode("Class.", OriginalNode));
        }
    }
}