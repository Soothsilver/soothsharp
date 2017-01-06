using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation
{
    class SeqTranslator
    {
        private const string ContractsNamespace = nameof(Soothsharp) + "." + nameof(Contracts) + ".";
        private const string SeqClass = ContractsNamespace + nameof(Seq<float>) + ".";
        public const string SeqLength = SeqClass + nameof(Seq<float>.Length);
        public const string SeqAccess = SeqClass + "this";
        public const string SeqGetLength = SeqClass + nameof(Seq<float>.GetLength);

    }
}
