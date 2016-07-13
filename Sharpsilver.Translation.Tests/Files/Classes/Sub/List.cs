using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Contracts;

namespace Sharpsilver.Translation.Tests.Files.Classes
{
    [Verified]
    class List
    {
        public int Value;
        public List Next;

        public List(int value, List next)
        {
            Next = next;
            Value = value;    
        }

        public static void Test()
        {
            List l = new List(10, null);
            List l2 = new List(20, null);
            l.Next = l2;
            l.Value = 5;
            l2.Value = l.Value + 8;
            int a = l.Value + l.Next.Value;
            Contract.Assert(a == 18);
        }
    }
}
