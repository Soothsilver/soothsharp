using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.BackendInterface
{
    public class VerificationResult
    {
        public bool VerificationSuccessful => Errors.Count == 0;

        public List<Error> Errors = new List<Error>();
        public string OriginalOutput = "";

        public static VerificationResult Error(Error error)
        {
            VerificationResult r = new VerificationResult();
            r.Errors.Add(error);
            return r;
        }
    }
}
