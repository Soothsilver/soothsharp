using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.Translators
{
    public class ContractsTranslator
    {
        // TODO use nameof() here.
        private const string CONTRACTS_NAMESPACE = "Sharpsilver.Contracts.";
        private const string CONTRACTS_CLASS = CONTRACTS_NAMESPACE + "Contract.";
        public const string CONTRACT_ENSURES = CONTRACTS_CLASS + "Ensures";
        public const string CONTRACT_REQUIRES = CONTRACTS_CLASS + "Requires";
        public const string CONTRACT_INVARIANT = CONTRACTS_CLASS + "Invariant";
        public const string CONTRACT_ASSERT = CONTRACTS_CLASS + "Assert";
        public const string CONTRACT_ASSUME = CONTRACTS_CLASS + "Assume";
        public const string CONTRACT_INT_RESULT = CONTRACTS_CLASS + "IntegerResult";


        public const string SILVERNAME_ATTRIBUTE = CONTRACTS_NAMESPACE + "SilvernameAttribute";
        public const string PREDICATE_ATTRIBUTE = CONTRACTS_NAMESPACE + "PredicateAttribute";
        public const string PURE_ATTRIBUTE = CONTRACTS_NAMESPACE + "PureAttribute";
        public const string VERIFIED_ATTRIBUTE = CONTRACTS_NAMESPACE + "VerifiedAttribute";
        public const string UNVERIFIED_ATTRIBUTE = CONTRACTS_NAMESPACE + "UnverifiedAttribute";


        private TranslationProcess parent;

        public ContractsTranslator(TranslationProcess process)
        {
            parent = process;
        }
        
    }
}
