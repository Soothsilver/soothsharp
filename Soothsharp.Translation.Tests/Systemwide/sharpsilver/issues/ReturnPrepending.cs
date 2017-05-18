using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Systemwide.sharpsilver.issues
{
    public static class SeqUtilsIssue
    {
        public static int[] SeqToArray(Seq<int> sequence)
        {
            return new int[] {32};
        }
    }
}
