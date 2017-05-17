using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Systemwide.sharpsilver.issues
{
    class SeqParam
    {
        public static void Hello(int param)
        {
            Seq<int> s = new Seq<int>(param);
        }
    }
}
