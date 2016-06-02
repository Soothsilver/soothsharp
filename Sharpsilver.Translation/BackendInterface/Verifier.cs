using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.BackendInterface
{
    class Verifier
    {
        public IBackend Backend;

        public Verifier(IBackend backend)
        {
            Backend = backend;
        }

        public VerificationResult Verify(Silvernode program)
        {
            var result = Backend.Verify(program);
            return result;
        }
    }
}
