using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Highlevel
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
      
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var classSymbol = context.Semantics.GetDeclaredSymbol(this.OriginalNode as ClassDeclarationSyntax);
            var attributes = classSymbol.GetAttributes();            
            switch (VerificationSettings.ShouldVerify(attributes, context.VerifyUnmarkedItems))
            {
                case VerificationSetting.DoNotVerify:
                     return TranslationResult.FromSilvernode(new SinglelineCommentSilvernode($"Class {classSymbol.GetQualifiedName()} skipped.", this.OriginalNode));
                case VerificationSetting.Contradiction:
                    return TranslationResult.Error(this.OriginalNode,
                        Diagnostics.SSIL113_VerificationSettingsContradiction);
            }
            return CommonUtils.GetHighlevelSequence(this.children.Select(child => child.Translate(context)));
        }

        public override void CollectTypesInto(TranslationProcess translationProcess, SemanticModel semantics)
        {


            var classSymbol = semantics.GetDeclaredSymbol(this.OriginalNode as ClassDeclarationSyntax);
            var attributes = classSymbol.GetAttributes();
            switch (VerificationSettings.ShouldVerify(attributes, translationProcess.Configuration.VerifyUnmarkedItems))
            {
                case VerificationSetting.DoNotVerify:
                    return;
                case VerificationSetting.Contradiction:
                    return;

            }

            this.TypeIfCollected = translationProcess.AddToCollectedTypes(this, semantics);
            foreach (Sharpnode node in children)
            {
                if (node is FieldDeclarationSharpnode)
                {
                    FieldDeclarationSharpnode fieldDeclaration = (FieldDeclarationSharpnode)node;
                    var fieldSymbol = fieldDeclaration.GetSymbol(semantics);
                    if (fieldSymbol.IsConst) continue; // Constants are inlined.
                    var typeSymbol = fieldSymbol.Type;
                    Error error;
                    this.TypeIfCollected.InstanceFields.Add(
                        new CollectedField(
                        translationProcess.IdentifierTranslator.RegisterAndGetIdentifier(((FieldDeclarationSharpnode)node).GetSymbol(semantics)),
                        TypeTranslator.TranslateType(typeSymbol, node.OriginalNode, out error)
                        ));
                    if (error != null)
                    {
                        translationProcess.AddError(error);
                    }
                }
            }
        }
    }
}