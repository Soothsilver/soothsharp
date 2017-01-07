using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation
{
    internal static class SeqTranslator
    {
        private const string CONTRACTS_NAMESPACE = nameof(Soothsharp) + "." + nameof(Contracts) + ".";
        private const string SEQ_CLASS = SeqTranslator.CONTRACTS_NAMESPACE + nameof(Seq<float>) + ".";
        public const string SeqClassWithoutEndDot = SeqTranslator.CONTRACTS_NAMESPACE + nameof(Seq<float>);
        public const string SeqLength = SeqTranslator.SEQ_CLASS + nameof(Seq<float>.Length);
        public const string SeqAccess = SeqTranslator.SEQ_CLASS + "this";
        public const string Contains = SeqTranslator.SEQ_CLASS + nameof(Seq<float>.Contains);
        public const string TakeDrop = SeqTranslator.SEQ_CLASS + nameof(Seq<float>.TakeDrop);
        public const string Take = SeqTranslator.SEQ_CLASS + nameof(Seq<float>.Take);
        public const string Drop = SeqTranslator.SEQ_CLASS + nameof(Seq<float>.Drop);

    }
}
