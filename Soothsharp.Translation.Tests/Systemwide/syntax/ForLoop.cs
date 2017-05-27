using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Translation.Tests.Systemwide.syntax
{
    class ForLoop
    {
        int WithDeclarations()
        {
            Ensures(IntegerResult == (1 + 2 + 3 + 4));

            int result = 0;
            for (int a = 0; a != 4; )
            {
                Invariant(result == (1 + a) * a / 2);

                result += (a + 1);
                a++;
            }
            return result;
        }
        int WithInitializers()
        {
            Ensures(IntegerResult == (1 + 2 + 3 + 4));

            int result = 0;
            int a;
            for (a = 0; a != 4;)
            {
                Invariant(result == (1 + a) * a / 2);

                result += (a + 1);
                a++;
            }
            return result;
        }
    }
}
