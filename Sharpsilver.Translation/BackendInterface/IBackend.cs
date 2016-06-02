using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.BackendInterface
{
    public interface IBackend
    {
        VerificationResult Verify(Silvernode silvernode);
    }
}
