//SUCCEEDS

using System;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Systemwide.scala2silver.basic
{
    [Verified]
    static class Implications
    {
        public static void Impl(bool a)
        {
            Contract.Requires(a.Implies(true));
              
            // ReSharper disable once UnusedVariable
            bool b = a.Implies(Implications.M());
        }
        static Boolean M()
        {
            return true;
        }
    }
}
