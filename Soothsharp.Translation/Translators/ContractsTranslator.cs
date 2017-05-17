using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Soothsharp.Contracts;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ContractsTranslator
    {
        // Contracts class
        private const string ContractsNamespace = nameof(Soothsharp) + "." + nameof(Contracts) + ".";
        private const string ContractsClass = ContractsTranslator.ContractsNamespace + nameof(Contract) + ".";
        public const string ContractEnsures = ContractsTranslator.ContractsClass + nameof(Contract.Ensures);
        public const string ContractRequires = ContractsTranslator.ContractsClass + nameof(Contract.Requires);
        public const string ContractInvariant = ContractsTranslator.ContractsClass + nameof(Contract.Invariant);
        public const string ContractAcc = ContractsTranslator.ContractsClass + nameof(Contract.Acc);
        public const string ContractAccArray = ContractsTranslator.ContractsClass + nameof(Contract.AccArray);
        public const string ContractAssert = ContractsTranslator.ContractsClass + nameof(Contract.Assert);
        public const string ContractAssume = ContractsTranslator.ContractsClass + nameof(Contract.Assume);
        public const string ContractInhale = ContractsTranslator.ContractsClass + nameof(Contract.Inhale);
        public const string ContractExhale = ContractsTranslator.ContractsClass + nameof(Contract.Exhale);
        private const string ContractIntResult = ContractsTranslator.ContractsClass + nameof(Contract.IntegerResult);
        public const string Result = ContractsTranslator.ContractsClass + nameof(Contract.Result);
        public const string Fold = ContractsTranslator.ContractsClass + nameof(Contract.Fold);
        public const string Unfold = ContractsTranslator.ContractsClass + nameof(Contract.Unfold);
        public const string Folding = ContractsTranslator.ContractsClass + nameof(Contract.Folding);
        public const string Unfolding = ContractsTranslator.ContractsClass + nameof(Contract.Unfolding);
        public const string Old = ContractsTranslator.ContractsClass + nameof(Contract.Old);
        public const string ForAll = ContractsTranslator.ContractsClass + nameof(Contract.ForAll);
        public const string Exists = ContractsTranslator.ContractsClass + nameof(Contract.Exists);

        // Static extensions
        public const string Implication = "System.Boolean." + nameof(StaticExtension.Implies);
        public const string Equivalence = "System.Boolean." + nameof(StaticExtension.EquivalentTo);

        // Permission class
        public const string PermissionType = ContractsTranslator.ContractsNamespace + nameof(Permission);
        private const string PermissionTypeDot = ContractsTranslator.PermissionType + ".";
        private const string PermissionWrite = ContractsTranslator.PermissionTypeDot + nameof(Permission.Write);
        private const string PermissionHalf = ContractsTranslator.PermissionTypeDot + nameof(Permission.Half);
        private const string PermissionNone = ContractsTranslator.PermissionTypeDot + nameof(Permission.None);
        private const string PermissionWildcard = ContractsTranslator.PermissionTypeDot + nameof(Permission.Wildcard);
        public const string PermissionCreate = ContractsTranslator.PermissionTypeDot + nameof(Permission.Create);
        public const string PermissionFromLocation = ContractsTranslator.PermissionTypeDot + nameof(Permission.FromLocation);

        // Attributes
        public const string PredicateAttribute = ContractsTranslator.ContractsNamespace + nameof(Contracts.PredicateAttribute);
        public const string PureAttribute = ContractsTranslator.ContractsNamespace + nameof(Contracts.PureAttribute);
        public const string VerifiedAttribute = ContractsTranslator.ContractsNamespace + nameof(Contracts.VerifiedAttribute);
        public const string UnverifiedAttribute = ContractsTranslator.ContractsNamespace + nameof(Contracts.UnverifiedAttribute);
        public const string AbstractAttribute = ContractsTranslator.ContractsNamespace + nameof(Contracts.AbstractAttribute);
        public const string SignatureOnlyAttribute = ContractsTranslator.ContractsNamespace + nameof(Contracts.SignatureOnlyAttribute);


        public TranslationResult TranslateIdentifierAsContract(ISymbol symbol, SyntaxNode originalNode, TranslationContext context)
        {
            string silvertext = null;
            switch(symbol.GetQualifiedName())
            {
                case ContractsTranslator.ContractIntResult:
                    silvertext = context.IsFunctionOrPredicateBlock ? "result" : Constants.SilverReturnVariableName;
                    break;
                case ContractsTranslator.PermissionNone:
                    silvertext = "none";
                    break;
                case ContractsTranslator.PermissionHalf:
                    silvertext = "1/2";
                    break;
                case ContractsTranslator.PermissionWildcard:
                    silvertext = "wildcard";
                    break;
                case ContractsTranslator.PermissionWrite:
                    silvertext = "write";
                    break;
            }
            if (silvertext != null) { 
            return TranslationResult.FromSilvernode(
                                new TextSilvernode(silvertext, originalNode)
                                );
            }
            return null;
        }

        public static bool IsMethodPureOrPredicate(IMethodSymbol theMethod)
        {
            var attributes = theMethod.GetAttributes();
            return attributes.Any(
                attr => attr.AttributeClass.GetQualifiedName() == ContractsTranslator.PureAttribute ||
                        attr.AttributeClass.GetQualifiedName() == ContractsTranslator.PredicateAttribute);
        }
    }
}
