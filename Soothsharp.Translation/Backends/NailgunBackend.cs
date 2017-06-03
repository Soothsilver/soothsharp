using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Backends
{
    /// <summary>
    /// Base class for both verifiers using the Nailgun server. These are used primarily because
    /// they're faster (they usually don't need JVM initialization).
    /// </summary>
    /// <seealso cref="Soothsharp.Translation.Backends.IBackend" />
    public abstract class NailgunBackend : IBackend
    {
        private static bool nailgunInitialized;
        private readonly string _runnerClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="NailgunBackend"/> class.
        /// </summary>
        /// <param name="runnerClass">The Java class to run using Nailgun.</param>
        protected NailgunBackend(string runnerClass)
        {
            this._runnerClass = runnerClass;
        }
        
        public VerificationResult Verify(Silvernode silvernode)
        {
            if (!nailgunInitialized)
            {
                // Start Nailgun server.
                ReadyNailgun();
                nailgunInitialized = true;
            }

            string silvercode = silvernode.ToString();
            string filename = Path.GetTempFileName();
            File.WriteAllText(filename, silvercode);
            try
            {
                // Run the nailgun client, catch the output.
                Process p = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        RedirectStandardError = true
                    }
                };
                var enviromentPath = Environment.GetEnvironmentVariable("PATH");
                Debug.Assert(enviromentPath != null, "enviromentPath != null");
                var paths = enviromentPath.Split(';');
                var exePath = paths
                    .Select(x => Path.Combine(x, "ng.exe"))
                    .FirstOrDefault(File.Exists) ?? "ng.exe";
                p.StartInfo.FileName = exePath;
                // Runner class is the class that operates the verifier (programmed in Scala).
                p.StartInfo.Arguments = _runnerClass + " \"" + filename + "\"";
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                VerificationResult r = new VerificationResult
                {
                    OriginalOutput = output,
                    Errors = BackendUtils.ConvertErrorMessages(output, silvernode)
                };
                return r;
            }
            catch (Exception)
            {
                return VerificationResult.Error(new Error(Diagnostics.SSIL201_BackendNotFound, null, "ng.exe"));
            }
        }

        private static void ReadyNailgun()
        {
            // Start Viper server
            Process p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                }
            };
            var enviromentPath = Environment.GetEnvironmentVariable("PATH");
            Debug.Assert(enviromentPath != null, "enviromentPath != null");
            var paths = enviromentPath.Split(';');
            var exePath = paths
                .Select(x => Path.Combine(x, "startviperserver.bat"))
                .FirstOrDefault(File.Exists) ?? "startviperserver.bat";
            p.StartInfo.FileName = exePath;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
            p.Start();

            // Wait until it actually starts.
            System.Threading.Thread.Sleep(500);
            // TODO (future) This is unsafe. We should somehow wait until the Nailgun server
            // it initialized. But how?
        }
    }
}