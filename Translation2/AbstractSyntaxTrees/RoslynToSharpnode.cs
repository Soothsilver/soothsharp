using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;

namespace Sharpsilver.Translation.Translators
{
    public class RoslynToSharpnode
    {
        public static Sharpnode Map(SyntaxNode node)
        {
            switch(node.Kind())
            {
                case SyntaxKind.CompilationUnit:
                    return new CompilationUnitSharpnode(node as CompilationUnitSyntax);
                default:
                    return new UnknownSharpnode(node);
            }
        }
        
        /*
        public static TranslationResult TranslateSyntaxNode(SyntaxNode node, TranslationContext context, TranslationProcess process)
        {
            switch (node.Kind())
            {
                case SyntaxKind.CompilationUnit:
                    return ContainerTranslator.TranslateCompilationUnit(node as CompilationUnitSyntax, context, process);
                case SyntaxKind.NamespaceDeclaration:
                    return ContainerTranslator.TranslateNamespace(node as NamespaceDeclarationSyntax, context, process);
                case SyntaxKind.ClassDeclaration:
                    return ContainerTranslator.TranslateClassDeclaration(node as ClassDeclarationSyntax, context, process);
                case SyntaxKind.MethodDeclaration:
                    return ContainerTranslator.TranslateMethodDeclaration(node as MethodDeclarationSyntax, context, process);
                case SyntaxKind.Block:
                    return StatementTranslator.TranslateBlock(node as BlockSyntax, context, process);
                default:
                    return TranslationResult.Error(node, Diagnostics.SSIL101, node.Kind());
            }
        }*/
    }
}
