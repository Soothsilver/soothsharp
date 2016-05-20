using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.StandaloneVerifier
{
    class SiliconRunner
    {
        public void Run(string silvercode)
        {
            string filename = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllText(filename, silvercode);

            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "C:\\viperd\\silicon\\silicon.bat";
                p.StartInfo.Arguments = filename;
                Console.WriteLine("Running silicon...");
                bool success = p.Start();
                if (success)
                {
                    string output = p.StandardOutput.ReadToEnd();
                    Console.WriteLine(output);
                }
                else
                {
                    Console.WriteLine("Silicon could not be started.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - Silicon did not run correctly.");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
