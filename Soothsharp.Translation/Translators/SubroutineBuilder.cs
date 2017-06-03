using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.CSharp;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    class SubroutineBuilder
    {
        private readonly BlockSharpnode BodySharpnode;
        private readonly bool IsConstructor;
        private readonly IMethodSymbol MethodSymbol;
        private readonly List<ParameterSharpnode> Parameters;
        private readonly TranslationContext Context;
        private readonly INamedTypeSymbol ConstructorClass;
        private readonly SyntaxNode OriginalNode;

        public SubroutineBuilder(
            IMethodSymbol symbol,
            bool isConstructor,
            INamedTypeSymbol constructorClass,
            List<ParameterSharpnode> parameters,
            BlockSharpnode body,
            TranslationContext context,
            SyntaxNode originalNode
            )
        {
            this.ConstructorClass = constructorClass;
            this.MethodSymbol = symbol;
            this.IsConstructor = isConstructor;
            this.Parameters = parameters;
            this.BodySharpnode = body;
            this.Context = context;
            this.OriginalNode = originalNode;
        }


        public TranslationResult TranslateSelf()
        {
            Identifier identifier = GetSubroutineIdentifier();
            SilverKind silverKind = SilverKind.Method;
            TranslationResult result = new TranslationResult();
            bool isAbstract = false;

            var attributes = this.MethodSymbol.GetAttributes();
            switch (VerificationSettings.ShouldVerify(attributes, this.Context.VerifyUnmarkedItems)) {
                case VerificationSetting.DoNotVerify:
                    return TranslationResult.FromSilvernode(new SinglelineCommentSilvernode($"Method {this.MethodSymbol.GetQualifiedName()} skipped because it was marked [Unverified].", this.OriginalNode));
                case VerificationSetting.Contradiction:
                    return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL113_VerificationSettingsContradiction);
                case VerificationSetting.Verify:
                    break;
                default:
                    throw new InvalidOperationException("Nonexistent verification settings.");
            }
            foreach (var attribute in attributes)
            {
                switch (attribute.AttributeClass.GetQualifiedName())
                {
                    case ContractsTranslator.PredicateAttribute:
                        if (silverKind == SilverKind.Method && !this.IsConstructor)
                        {
                            silverKind = SilverKind.Predicate;
                        }
                        else
                        {
                            return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL116_MethodAttributeContradiction);
                        }
                        break;
                    case ContractsTranslator.PureAttribute:
                        if (silverKind == SilverKind.Method && !this.IsConstructor)
                        {
                            silverKind = SilverKind.Function;
                        }
                        else
                        {
                            return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL116_MethodAttributeContradiction);
                        }
                        break;
                    case ContractsTranslator.AbstractAttribute:
                    case ContractsTranslator.SignatureOnlyAttribute:
                        if (this.IsConstructor)
                        {
                            return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL117_ConstructorMustNotBeAbstract);
                        }
                        isAbstract = true;
                        break;

                        // Ignore other attributes.
                }
            }
            if (Context.MarkEverythingAbstract)
            {
                isAbstract = true;
            }
            TranslationContext bodyContext = this.Context;
            if (silverKind == SilverKind.Function || silverKind == SilverKind.Predicate)
            {
                bodyContext = new TranslationContext(this.Context)
                {
                    IsFunctionOrPredicateBlock = true,
                    PurityContext = PurityContext.PureOrFail
                };
            }
            TranslationResult body = this.BodySharpnode.Translate(bodyContext);
            result.Errors.AddRange(body.Errors);

            var silverParameters = new List<ParameterSilvernode>();
            if (!this.IsConstructor && !this.MethodSymbol.IsStatic)
            {
                silverParameters.Add(new ParameterSilvernode(new Identifier(Constants.SilverThis), new TypeSilvernode(null, SilverType.Ref), null));
            }
            for (int i = 0; i < this.Parameters.Count; i++)
            {
                ParameterSharpnode sharpnode = this.Parameters[i];
                var symbol = this.MethodSymbol.Parameters[i];
                var rrs = sharpnode.Translate(this.Context, symbol);
                silverParameters.Add(rrs.Silvernode as ParameterSilvernode);
                result.Errors.AddRange(rrs.Errors);
            }

            // Prepare silvernodes before composing them
            var silName = new IdentifierSilvernode(identifier);
            var silOriginalnode = this.OriginalNode;
            var silParameters = silverParameters;
            string silReturnValueName = Constants.SilverReturnVariableName;
            if (this.IsConstructor) silReturnValueName = Constants.SilverThis;
            if (silverKind != SilverKind.Method)
            {
                silReturnValueName = "result"; // "result" is a Silver keyword
            }

            Error diagnostic;
            SilverType silverReturnType = TypeTranslator.TranslateType(this.IsConstructor ? this.ConstructorClass : this.MethodSymbol.ReturnType, null, out diagnostic);
            if (diagnostic != null) result.Errors.Add(diagnostic);
            var silTypeSilvernode = new TypeSilvernode(null, silverReturnType);
            var silVerificationConditions = body.Contracts;
            var silBlock = body.Silvernode as BlockSilvernode;
            if (silverReturnType == SilverType.Void && silverKind == SilverKind.Function)
            {
                return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL118_FunctionsMustHaveAReturnType);
            }
            if (silverReturnType != SilverType.Bool && silverKind == SilverKind.Predicate)
            {
                return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL119_PredicateMustBeBool);
            }
            if (this.IsConstructor)
            {
                silBlock.Prepend(new AssignmentSilvernode(Constants.SilverThis, new CallSilvernode(this.Context.Process.IdentifierTranslator.GetIdentifierReferenceWithTag(this.ConstructorClass, Constants.InitializerTag), new List<Silvernode>(), SilverType.Ref, null), null));
            }
            if (silverKind == SilverKind.Method)
            {
                silBlock.Add(new LabelSilvernode(Constants.SilverMethodEndLabel, null));
            }
            switch (silverKind)
            {
                case SilverKind.Method:
                    result.Silvernode = new MethodSilvernode(silOriginalnode, silName, silParameters, silReturnValueName, silTypeSilvernode, silVerificationConditions, isAbstract ? null : silBlock);
                    break;
                case SilverKind.Function:
                    result.Silvernode = new FunctionSilvernode(silOriginalnode, silName, silParameters, silReturnValueName, silTypeSilvernode, silVerificationConditions, isAbstract ? null : silBlock);
                    break;
                case SilverKind.Predicate:
                    result.Silvernode = new PredicateSilvernode(silOriginalnode, silName, silParameters, silReturnValueName, silTypeSilvernode, silVerificationConditions, isAbstract ? null : silBlock);
                    break;
                default:
                    throw new InvalidOperationException("Nonexistent silverkind.");
            }

            return result;
        }

        private Identifier GetSubroutineIdentifier()
        {
            if (this.IsConstructor)
            {
                return this.Context.Process.IdentifierTranslator.RegisterAndGetIdentifierWithTag(this.ConstructorClass, Constants.ConstructorTag);
            }
            else
            {
                return this.Context.Process.IdentifierTranslator.RegisterAndGetIdentifier(this.MethodSymbol);
            }
        }

        enum SilverKind
        {
            Method,
            Function,
            Predicate
        }
    }
}
