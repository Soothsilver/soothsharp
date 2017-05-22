// expect SSIL204
// expect SSIL204
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Translation.Tests.Syntax.Arrays
{
    class InvalidArrayWriteRead
    {
        public static void ModifyArray(int[] array)
        {
            Contract.Requires(AccArray(array));
            Contract.Requires(array.Length == 2);

            int invalidRead = array[8];
            array[16] = 50;
        }
    }
}
