
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Contracts;
using static Sharpsilver.Contracts.Contract;
using static Sharpsilver.Contracts.Permission;
// ReSharper disable All
#pragma warning disable 169

namespace Sharpsilver.Translation.Tests.Systemwide.scala2silver.translation
{
    class Constructors
    {
        int param;
        bool b;
        int paramTrue;
        int paramFalse;

        // IGNORE
        // TODO field invariant
        int a = 3;

        public Constructors(int _param, bool _b, int _paramTrue, int _paramFalse)
        {
            this.param = _param;
            this.b = _b;
            this.paramTrue = _paramTrue;
            this.paramFalse = _paramFalse;

            Ensures(Acc(param, Wildcard));
        }
    }
}
