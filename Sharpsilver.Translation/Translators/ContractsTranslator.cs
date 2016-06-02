using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Contracts;

namespace Sharpsilver.Translation.Translators
{
    public class ContractsTranslator
    {
        private const string ContractsNamespace = 
            nameof(Sharpsilver) + "." + 
            nameof(Sharpsilver.Contracts) + ".";
        private const string ContractsClass = 
            ContractsNamespace + 
            nameof(Sharpsilver.Contracts.Contract) + ".";
        public const string ContractEnsures = ContractsClass + nameof(Contract.Ensures);
        public const string ContractRequires = ContractsClass + nameof(Contract.Requires);
        public const string ContractInvariant = ContractsClass + nameof(Contract.Invariant);
        public const string ContractAssert = ContractsClass + nameof(Contract.Assert);
        public const string ContractAssume = ContractsClass + nameof(Contract.Assume);
        public const string ContractIntResult = ContractsClass + nameof(Contract.IntegerResult);
        public const string Implication = "System.Boolean." + nameof(StaticExtension.Implies);


        public const string SilvernameAttribute = ContractsNamespace + nameof(Sharpsilver.Contracts.SilvernameAttribute);
        public const string PredicateAttribute = ContractsNamespace + nameof(Sharpsilver.Contracts.PredicateAttribute);
        public const string PureAttribute = ContractsNamespace + nameof(Sharpsilver.Contracts.PureAttribute);
        public const string VerifiedAttribute = ContractsNamespace + nameof(Sharpsilver.Contracts.VerifiedAttribute);
        public const string UnverifiedAttribute = ContractsNamespace + nameof(Sharpsilver.Contracts.UnverifiedAttribute);
        
        private TranslationProcess parent;

        public ContractsTranslator(TranslationProcess process)
        {
            parent = process;
        }
        
    }
}
