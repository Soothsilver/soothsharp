using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Files.Abstract
{
    class SimpleAbstractPredicate
    {
        [Abstract]
        [Predicate]
        public static int ReturnANumber()
        {
            return 0;
        } 

        public static void Do()
        {
            int a = SimpleAbstractPredicate.ReturnANumber();
        }
    }
}
