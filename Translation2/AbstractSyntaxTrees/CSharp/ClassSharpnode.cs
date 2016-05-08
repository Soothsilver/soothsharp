﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System.Collections.Generic;
using System.Linq;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    internal class ClassSharpnode : Sharpnode
    {
        private List<Sharpnode> Children = new List<Sharpnode>();
        private NamespaceSharpnode ParentNamespace;
        

        public ClassSharpnode(ClassDeclarationSyntax node, NamespaceSharpnode parent) : base(node)
        {
            ParentNamespace = parent;
            Children.AddRange(node.Members.Select(mbr => RoslynToSharpnode.Map(mbr, this)));
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var ClassSymbol = context.Process.SemanticModel.GetDeclaredSymbol(OriginalNode as ClassDeclarationSyntax);

            var attributes = ClassSymbol.GetAttributes();
            
            if (attributes.Any(attribute => attribute.AttributeClass.GetQualifiedName() == ContractsLinks.UNVERIFIED_ATTRIBUTE))
            {
                return TranslationResult.Silvernode(new SinglelineCommentSilvernode($"Class {ClassSymbol.Name} skipped because it was marked [Unverified].", OriginalNode));
            }

            return CommonUtils.CombineResults(Children.Select(child => child.Translate(context)));

        }
    }
}