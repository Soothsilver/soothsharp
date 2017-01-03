using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;
// ReSharper disable All

namespace Soothsharp.Translation.Tests.Systemwide.scala2silver.translation
{
    class Constants
    {
        [Predicate]
        public bool P()
        {
            return 3 + 5 > 7 &&
                ((3 == 3) || false) &&
                true &&
                F(this)
                ;
        }
        [Pure]
        public static bool F(Constants b)
        {
            return b != null && 3 < 5;
        }

        public void Main()
        {
            Fold(Acc(P(), Permission.Write));
        }

        [Pure]
        public static Constants g()
        {
            return null;
        }
    }
}
