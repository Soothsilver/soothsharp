using Sharpsilver.Translation.Trees.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Contracts;

namespace Sharpsilver.Translation
{
    public class ContractsTranslator
    {
        private const string ContractsNamespace = nameof(Sharpsilver) + "." + nameof(Contracts) + ".";
        private const string ContractsClass = ContractsNamespace + nameof(Contract) + ".";
        public const string ContractEnsures = ContractsClass + nameof(Contract.Ensures);
        public const string ContractRequires = ContractsClass + nameof(Contract.Requires);
        public const string ContractInvariant = ContractsClass + nameof(Contract.Invariant);
        public const string ContractRead = ContractsClass + nameof(Contract.Read);
        public const string ContractAcc = ContractsClass + nameof(Contract.Acc);
        public const string ContractAssert = ContractsClass + nameof(Contract.Assert);
        public const string ContractAssume = ContractsClass + nameof(Contract.Assume);
        public const string ContractIntResult = ContractsClass + nameof(Contract.IntegerResult);
        public const string Implication = "System.Boolean." + nameof(StaticExtension.Implies);


        public const string SilvernameAttribute = ContractsNamespace + nameof(Contracts.SilvernameAttribute);
        public const string PredicateAttribute = ContractsNamespace + nameof(Contracts.PredicateAttribute);
        public const string PureAttribute = ContractsNamespace + nameof(Contracts.PureAttribute);
        public const string VerifiedAttribute = ContractsNamespace + nameof(Contracts.VerifiedAttribute);
        public const string UnverifiedAttribute = ContractsNamespace + nameof(Contracts.UnverifiedAttribute);
        
        private TranslationProcess parent;

        public ContractsTranslator(TranslationProcess process)
        {
            this.parent = process;
        }
        
    }
}
