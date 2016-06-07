using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.BackendInterface
{
    public class SiliconBackend : IBackend
    {
        private static List<Regex> harmlessLines = new List<Regex>();
        private static Regex regexParseError = new Regex(@"Parse error: (.*) (\([^@]*@[^)]*\))");
        private static Regex regexCodePosition = new Regex(@"\(([^@]+)@([0-9]+)\.([0-9]+)\)");
        static SiliconBackend()
        {
            harmlessLines.Add(new Regex("Silicon finished in .* seconds."));
            harmlessLines.Add(new Regex(@"^\W*$"));
            harmlessLines.Add(new Regex(@"\(c\) Copyright ETH Zurich .*"));
            harmlessLines.Add(new Regex("Silicon 1.1-SNAPSHOT .*"));
            harmlessLines.Add(new Regex("No errors detected."));
            harmlessLines.Add(new Regex("No errors found."));
            harmlessLines.Add(new Regex("The following errors were found:"));
        }

        private List<Error> ConvertErrorMessages(string siliconResult, Silvernode originalCode)
        {
            List<Error> errors = new List<Error>();

            foreach (string line in siliconResult.Split(new[] {  '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (Regex r in harmlessLines)
                {
                    if (r.IsMatch(line)) goto nextline;
                }
                if (line.Contains("Parse error"))
                {
                    Match m = regexParseError.Match(line);
                    if (m.Success)
                    {
                        var errorText = m.Groups[1].Value;
                        var codePosition = m.Groups[2].Value;
                        errors.Add(new Error(Diagnostics.SSIL203_ParseError,
                            getSyntaxNodeFromCodePosition(codePosition, originalCode),
                            errorText));
                        continue;
                    }
                }
                else
                {
                    var matches = regexCodePosition.Matches(line);
                    if (matches.Count > 0)
                    {
                        var errorText = line.Trim();
                        foreach (Match m in matches)
                        {
                            var codePosition = m.Value;
                            errors.Add(new Error(Diagnostics.SSIL204_OtherLocalizedError,
                                getSyntaxNodeFromCodePosition(codePosition, originalCode),
                                errorText));
                        }
                    }
                    else
                    {
                        errors.Add(new Error(Diagnostics.SSIL202_BackendUnknownLine, null, line.Trim()));
                    }
                }
                nextline: ;
            }

            return errors;
        }

        private SyntaxNode getSyntaxNodeFromCodePosition(string codePosition, Silvernode originalCode)
        {
            Match m = regexCodePosition.Match(codePosition);
            if (m.Success)
            {
                string codefile = m.Groups[1].Value;
                string lineString = m.Groups[2].Value;
                string columnString = m.Groups[3].Value;
                int line = int.Parse(lineString);
                int column = int.Parse(columnString);
                // tut, tut, here we must do something smart.
                string silvercode = originalCode.ToString();
                string[] lines = silvercode.Split('\n');
                int position = 0;
                for (int i = 0; i < line-1; i++)
                {
                    position += lines[i].Length + 1;
                }
                position += column-1;
                Silvernode silvernode = originalCode.GetSilvernodeFromOffset(position);

                return silvernode.OriginalNode;
            }
            else
            {
                return null;
            }
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
                p.StartInfo.Arguments = "\"" +  filename + "\"";
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
