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
    /// <summary>
    /// The Silicon verifier uses symbolic execution and queries Z3 directly. Nailgun is used to speed up.
    /// </summary>
    /// <seealso cref="Soothsharp.Translation.Backends.NailgunBackend" />
    public class SiliconNailgunBackend : NailgunBackend
    {
        public SiliconNailgunBackend() : base("viper.silicon.SiliconRunner")
        {
        }
    }
}
