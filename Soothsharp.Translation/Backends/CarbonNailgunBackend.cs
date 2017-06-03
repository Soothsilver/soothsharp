using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Translation.Backends
{
    /// <summary>
    /// The Carbon verifier translates Viper into Boogie. The Nailgun server is used for speedup.
    /// </summary>
    /// <seealso cref="Soothsharp.Translation.Backends.NailgunBackend" />
    public class CarbonNailgunBackend : NailgunBackend
    {
        public CarbonNailgunBackend() : base("viper.carbon.Carbon")
        {
        }
    }
}
