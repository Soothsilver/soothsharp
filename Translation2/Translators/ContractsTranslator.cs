using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation
{
    public class ContractsTranslator
    {
        private const string CONTRACTS_NAMESPACE = "Sharpsilver.Contracts.";
        private const string CONTRACTS_CLASS = CONTRACTS_NAMESPACE + "Contract.";
        public const string CONTRACT_ENSURES = CONTRACTS_CLASS + "Ensures";
        public const string CONTRACT_REQUIRES = CONTRACTS_CLASS + "Requires";
        public const string CONTRACT_INT_RESULT = CONTRACTS_CLASS + "IntegerResult";
        public const string VERIFIED_ATTRIBUTE = CONTRACTS_NAMESPACE + "VerifiedAttribute";
        public const string UNVERIFIED_ATTRIBUTE = CONTRACTS_NAMESPACE + "UnverifiedAttribute";


        private TranslationProcess parent;

        public ContractsTranslator(TranslationProcess process)
        {
            parent = process;
        }
        
    }
}
