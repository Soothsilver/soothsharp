using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests
{
    public static class SeqUtilsIssue
    {
        public static Seq<int> ReturnSequence()
        {
            Contract.Ensures(Contract.Result<Seq<int>>().Length == 1);

            return new Seq<int>(20);
        }
    }
}
