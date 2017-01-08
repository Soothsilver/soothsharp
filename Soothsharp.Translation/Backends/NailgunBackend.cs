using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.BackendInterface
{
    public abstract class NailgunBackend : IBackend
    {
        private static bool nailgunInitialized;
        private readonly string _runnerClass;

        protected NailgunBackend(string runnerClass)
        {
            this._runnerClass = runnerClass;
        }
        public VerificationResult Verify(Silvernode silvernode)
        {
            if (!NailgunBackend.nailgunInitialized)
            {
                ReadyNailgun();
                NailgunBackend.nailgunInitialized = true;
            }

            string silvercode = silvernode.ToString();
            string filename = Path.GetTempFileName();
            File.WriteAllText(filename, silvercode);
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardError = true;
                var enviromentPath = Environment.GetEnvironmentVariable("PATH");
                Debug.Assert(enviromentPath != null, "enviromentPath != null");
                var paths = enviromentPath.Split(';');
                var exePath = paths
                    .Select(x => Path.Combine(x, "ng.exe"))
                    .FirstOrDefault(File.Exists) ?? "ng.exe";
                p.StartInfo.FileName = exePath;
                p.StartInfo.Arguments = _runnerClass + " \"" + filename + "\"";
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                VerificationResult r = new VerificationResult();
                r.OriginalOutput = output;
                r.Errors = BackendUtils.ConvertErrorMessages(output, silvernode);
                return r;
            }
            catch (Exception)
            {
                return VerificationResult.Error(new Error(Diagnostics.SSIL201_BackendNotFound, null, "ng.exe"));
            }
        }

        private static void ReadyNailgun()
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            var enviromentPath = Environment.GetEnvironmentVariable("PATH");
            Debug.Assert(enviromentPath != null, "enviromentPath != null");
            var paths = enviromentPath.Split(';');
            var exePath = paths
                .Select(x => Path.Combine(x, "startviperserver.bat"))
                .FirstOrDefault(File.Exists) ?? "startviperserver.bat";
            p.StartInfo.FileName = exePath;
            p.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(exePath);
            p.Start();
            System.Threading.Thread.Sleep(500);// TODO improve this
        }
    }
}