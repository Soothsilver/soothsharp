using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.Silver;
using Sharpsilver.Translation.Translators;
using Sharpsilver.Translation.Trees.Intermediate;

namespace Sharpsilver.Translation.Trees.CSharp.Highlevel
{
    /// <summary>
    /// Represents a class declaration in the C# abstract syntax tree.
    /// </summary>
    public class ClassSharpnode : Sharpnode
    {
        public ClassDeclarationSyntax DeclarationSyntax { get; }
        public bool IsStatic;
        private List<Sharpnode> children = new List<Sharpnode>();
        /// <summary>
        /// If this sharpnode's type was already collected by the translation process, then this contains a reference to the type. Use this
        /// to fill in instance fields, for example.
        /// </summary>
        public CollectedType TypeIfCollected;
        

        public ClassSharpnode(ClassDeclarationSyntax node) : base(node)
        {
            this.DeclarationSyntax = node;
            this.IsStatic = node.Modifiers.Any(syntaxToken => syntaxToken.Kind() == SyntaxKind.StaticKeyword);
            this.children.AddRange(node.Members.Select(RoslynToSharpnode.MapClassMember));
            if (node.Members.All(mds => mds.Kind() != SyntaxKind.ConstructorDeclaration) && !this.IsStatic)
            {
                this.children.Add(new ConstructorSharpnode(this));
            }
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var classSymbol = context.Semantics.GetDeclaredSymbol(this.OriginalNode as ClassDeclarationSyntax);

            var attributes = classSymbol.GetAttributes();
            
            if (attributes.Any(attribute => attribute.AttributeClass.GetQualifiedName() == ContractsTranslator.UnverifiedAttribute))
            {
                return TranslationResult.FromSilvernode(new SinglelineCommentSilvernode($"Class {classSymbol.GetQualifiedName()} skipped because it was marked [Unverified].", this.OriginalNode));
            }
            return CommonUtils.GetHighlevelSequence(this.children.Select(child => child.Translate(context)));
        }

        public override void CollectTypesInto(TranslationProcess translationProcess, SemanticModel semantics)
        {
            this.TypeIfCollected = translationProcess.AddToCollectedTypes(this, semantics);
            foreach (Sharpnode node in children)
            {
                if (node is FieldDeclarationSharpnode)
                {
                    this.TypeIfCollected.InstanceFields.Add(
                        translationProcess.IdentifierTranslator.RegisterAndGetIdentifier(((FieldDeclarationSharpnode)node).GetSymbol(semantics)));
                }
            }
        }
    }
}