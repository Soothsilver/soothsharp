using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Soothsharp.Translation.Backends;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Backends
{
    public class SiliconNailgunBackend : NailgunBackend
    {
        public SiliconNailgunBackend() : base("viper.silicon.SiliconRunner")
        {
        }
    }
}
