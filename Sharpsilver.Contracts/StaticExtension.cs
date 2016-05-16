using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts
{
    public static class StaticExtension
    {
        public static bool Implies(this bool condition, bool result)
        {
            return !condition || result;
        }
    }
}
