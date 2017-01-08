using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Translation.BackendInterface
{
    public class CarbonNailgunBackend : NailgunBackend
    {
        public CarbonNailgunBackend() : base("viper.carbon.CarbonRunner")
        {
        }
    }
}
