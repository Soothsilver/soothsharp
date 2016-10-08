using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.Trees.CSharp;
using Sharpsilver.Translation.Trees.Silver;
using Sharpsilver.Translation.Trees.Silver.Statements;

namespace Sharpsilver.Translation.Translators
{
    class SubroutineBuilder
    {
        private BlockSharpnode BodySharpnode;
        private bool IsConstructor;
        private IMethodSymbol MethodSymbol;
        private List<ParameterSharpnode> Parameters;
        private TypeSharpnode ReturnTypeSharpnode;
        private TranslationContext Context;
        private INamedTypeSymbol ConstructorClass;
        private SyntaxNode OriginalNode;

        public SubroutineBuilder(
            IMethodSymbol symbol,
            bool isConstructor,
            TypeSharpnode returnType,
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
            this.ReturnTypeSharpnode = returnType;
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

            var attributes = MethodSymbol.GetAttributes();
            switch (VerificationSettings.ShouldVerify(attributes, Context.VerifyUnmarkedItems)) {
                case VerificationSetting.DoNotVerify:
                    return TranslationResult.FromSilvernode(new SinglelineCommentSilvernode($"Method {MethodSymbol.GetQualifiedName()} skipped because it was marked [Unverified].", this.OriginalNode));
                case VerificationSetting.Contradiction:
                    return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL113_VerificationSettingsContradiction);
            }
            foreach(var attribute in attributes)
            {
                switch (attribute.AttributeClass.GetQualifiedName())
                {
                    case ContractsTranslator.SilvernameAttribute:
                        identifier = new Identifier((string)attribute.ConstructorArguments[0].Value);
                        break;
                    case ContractsTranslator.PredicateAttribute:
                        if (silverKind == SilverKind.Method && !IsConstructor)
                        {
                            silverKind = SilverKind.Predicate;
                        }
                        else
                        {
                            return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL116_MethodAttributeContradiction);
                        }
                        break;
                    case ContractsTranslator.PureAttribute:
                        if (silverKind == SilverKind.Method && !IsConstructor)
                        {
                            silverKind = SilverKind.Function;
                        }
                        else
                        {
                            return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL116_MethodAttributeContradiction);
                        }
                        break;
                    case ContractsTranslator.AbstractAttribute:
                        if( IsConstructor)
                        {
                            return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL117_ConstructorMustNotBeAbstract);

                        }
                        isAbstract = true;
                        break;
                }
            }
            TranslationContext bodyContext = new TranslationContext(Context);
            if (silverKind == SilverKind.Function || silverKind == SilverKind.Predicate)
            {
                bodyContext.IsFunctionOrPredicateBlock = true;
            }
            TranslationResult body = this.BodySharpnode.Translate(bodyContext);
            result.Errors.AddRange(body.Errors);

            var silverParameters = new List<ParameterSilvernode>();
            for (int i = 0; i < this.Parameters.Count; i++)
            {
                ParameterSharpnode sharpnode = this.Parameters[i];
                var symbol = MethodSymbol.Parameters[i];
                var rrs = sharpnode.Translate(Context, symbol);
                silverParameters.Add(rrs.Silvernode as ParameterSilvernode);
                result.Errors.AddRange(rrs.Errors);
            }

            // Prepare silvernodes before composing them
            var silName = new IdentifierSilvernode(identifier);
            var silOriginalnode = OriginalNode;
            var silParameters = silverParameters;
            string silReturnValueName = Constants.SilverReturnVariableName;
            if (IsConstructor) silReturnValueName = Constants.SilverThis;
            if (silverKind != SilverKind.Method)
            {
                silReturnValueName = "result"; // "result" is a Silver keyword
            }

            Error diagnostic;
            SilverType silverReturnType =
                            TypeTranslator.TranslateType(
                            IsConstructor ? ConstructorClass : MethodSymbol.ReturnType,
                            null,
                            out diagnostic);
            if (diagnostic != null) result.Errors.Add(diagnostic);
            var silTypeSilvernode = new TypeSilvernode(null, silverReturnType);
            var silVerificationConditions = body?.VerificationConditions;
            var silBlock = body.Silvernode as BlockSilvernode;
            if (silverReturnType == SilverType.Void && silverKind == SilverKind.Function)
            {
                return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL118_FunctionsMustHaveAReturnType);
            }
            if (silverReturnType != SilverType.Bool && silverKind == SilverKind.Predicate)
            {
                return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL119_PredicateMustBeBool);
            }
            if (IsConstructor)
            {
                silBlock.Prepend(
                    new AssignmentSilvernode(Constants.SilverThis,
             new CallSilvernode(
              Context.Process.IdentifierTranslator.GetIdentifierReferenceWithTag(ConstructorClass,
              Constants.InitializerTag)
             , new List<Silvernode>(), SilverType.Ref, null)
             , null));
            }
            silBlock.Add(new LabelSilvernode(Constants.SilverMethodEndLabel, null));

            switch (silverKind)
            {
                case SilverKind.Method:
                    result.Silvernode =
                         new MethodSilvernode(
                             silOriginalnode,
                             silName,
                             silParameters,
                             silReturnValueName,
                             silTypeSilvernode,
                             silVerificationConditions,
                             isAbstract ? null : silBlock
                             );
                    break;
                case SilverKind.Function:
                    result.Silvernode =
                       new FunctionSilvernode(
                           silOriginalnode,
                           silName,
                           silParameters,
                           silReturnValueName,
                           silTypeSilvernode,
                           silVerificationConditions,
                             isAbstract ? null : silBlock
                           );
                    break;
                case SilverKind.Predicate:
                    result.Silvernode =
                       new PredicateSilvernode(
                           silOriginalnode,
                           silName,
                           silParameters,
                           silReturnValueName,
                           silTypeSilvernode,
                           silVerificationConditions,
                           isAbstract ? null : silBlock
                           );
                    break;
            }
         
            return result;
        }

        private Identifier GetSubroutineIdentifier()
        {
            if (IsConstructor)
            {
                return Context.Process.IdentifierTranslator.RegisterAndGetIdentifierWithTag(ConstructorClass, Constants.ConstructorTag);
            }
            else
            {
                return Context.Process.IdentifierTranslator.RegisterAndGetIdentifier(MethodSymbol);
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
