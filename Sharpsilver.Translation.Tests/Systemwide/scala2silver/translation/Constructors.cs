
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;
using static Soothsharp.Contracts.Permission;
// ReSharper disable All

namespace Soothsharp.Translation.Tests.Systemwide.scala2silver.translation
{
    class Constructors
    {
        int param;
        bool b;
        int paramTrue;
        int paramFalse;

        // IGNORE
        // TODO field invariant
#pragma warning disable CS0414
        int a = 3;
#pragma warning restore CS0414

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
