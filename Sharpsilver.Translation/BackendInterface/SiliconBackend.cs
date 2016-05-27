using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.BackendInterface
{
    public class SiliconBackend : IBackend
    {
        private List<Error> ConvertErrorMessages(string siliconResult, Silvernode originalCode)
        {
            List<Error> errors = new List<Error>();

            foreach (string line in siliconResult.Split(new[] {  '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                errors.Add(new Error(Diagnostics.SSIL202_BackendUnknownLine, null, line));
            }

            return errors;
        }

        public VerificationResult Verify(Silvernode silvernode)
        {
            string silvercode = silvernode.ToString();
            string filename = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllText(filename, silvercode);
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                var enviromentPath = System.Environment.GetEnvironmentVariable("PATH");
                Debug.Assert(enviromentPath != null, "enviromentPath != null");
                var paths = enviromentPath.Split(';');
                var exePath = paths
                    .Select(x => Path.Combine(x, "silicon.bat"))
                    .FirstOrDefault(File.Exists);
                if (exePath == null) exePath = "silicon.bat";
                p.StartInfo.FileName = exePath;
                p.StartInfo.Arguments = filename;
                p.Start();

                string output = p.StandardOutput.ReadToEnd();
                VerificationResult r = new VerificationResult();
                r.OriginalOutput = output;
                r.Errors = ConvertErrorMessages(output, silvernode);
                return r;
            }
            catch (Exception)
            {
               return VerificationResult.Error(new Error(Diagnostics.SSIL201_BackendNotFound, null, "silicon.bat"));
            }
        }
    }
}
