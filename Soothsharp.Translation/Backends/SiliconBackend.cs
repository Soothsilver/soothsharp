using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Backends
{
    /// <summary>
    /// Connects to the "silicon" backend verifier using the batch file.
    /// </summary>
    public class SiliconBackend : IBackend
    {    
        public VerificationResult Verify(Silvernode silvernode)
        {
            string silvercode = silvernode.ToString();
            string filename = Path.GetTempFileName();
            File.WriteAllText(filename, silvercode);
            try
            {
                // Find and run "silicon.bat" in PATH and catch the output
                Process p = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                p.StartInfo.RedirectStandardOutput = true;
                var enviromentPath = Environment.GetEnvironmentVariable("PATH");
                Debug.Assert(enviromentPath != null, "enviromentPath != null");
                var paths = enviromentPath.Split(';');
                var exePath = paths
                    .Select(x => Path.Combine(x, "silicon.bat"))
                    .FirstOrDefault(File.Exists) ?? "silicon.bat";
                p.StartInfo.FileName = exePath;
                p.StartInfo.Arguments = "\"" +  filename + "\"";
                p.Start();

                string output = p.StandardOutput.ReadToEnd();
                VerificationResult r = new VerificationResult
                {
                    OriginalOutput = output,
                    Errors = BackendUtils.ConvertErrorMessages(output, silvernode)
                };
                return r;
            }
            catch (Exception)
            {
               return VerificationResult.Error(new Error(Diagnostics.SSIL201_BackendNotFound, null, "silicon.bat"));
            }
        }
    }
}
