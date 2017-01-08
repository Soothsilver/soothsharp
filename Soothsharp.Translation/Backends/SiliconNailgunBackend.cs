using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Soothsharp.Translation.BackendInterface;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.BackendInterface
{
    public class SiliconNailgunBackend : NailgunBackend
    {
        public SiliconNailgunBackend() : base("viper.silicon.SiliconRunner")
        {
        }
    }
}
