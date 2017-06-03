using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Backends
{
    /// <summary>
    /// Contains static constants and functions used by both backends.
    /// </summary>
    public static class BackendUtils
    {
        private static List<Regex> harmlessLines = new List<Regex>();
        private static Regex regexParseError = new Regex(@"Parse error: (.*) (\([^@]*@[^)]*\))");
        private static Regex regexCodePosition = new Regex(@"\(([^@]+)@([0-9]+)\.([0-9]+)\)");

        static BackendUtils()
        {
            // These lines can be output by the verifier, but they are not errors.
            harmlessLines.Add(new Regex("Silicon finished in .* seconds."));
            harmlessLines.Add(new Regex("carbon finished in .* seconds."));
            harmlessLines.Add(new Regex(@"^\W*$"));
            harmlessLines.Add(new Regex(@"\(c\).*ETH Zurich.*"));
            harmlessLines.Add(new Regex("Silicon 1.1-SNAPSHOT .*"));
            harmlessLines.Add(new Regex(@"\(c\) 2013 ETH Zurich .*"));
            harmlessLines.Add(new Regex("carbon 1.0.*"));
            harmlessLines.Add(new Regex("No errors found."));
            harmlessLines.Add(new Regex("might not work with Z3 version 4.4.1"));
            harmlessLines.Add(new Regex("The following errors were found:"));
        }

        /// <summary>
        /// Converts the standard output of a backend verifier to a list of verification errors.
        /// </summary>
        /// <param name="backendToolResult">The output of the verifier.</param>
        /// <param name="originalCode">The master silvernode tree that was passed to the verifier.</param>
        public static List<Error> ConvertErrorMessages(string backendToolResult, Silvernode originalCode)
        {
            List<Error> errors = new List<Error>();

            foreach (string line in backendToolResult.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (Regex r in harmlessLines)
                {
                    if (r.IsMatch(line)) goto nextline;
                }
                if (line.Contains("Parse error"))
                {
                    // Parse error has different format, special case.
                    Match m = regexParseError.Match(line);
                    if (m.Success)
                    {
                        var errorText = m.Groups[1].Value;
                        var codePosition = m.Groups[2].Value;
                        errors.Add(new Error(Diagnostics.SSIL203_ParseError, GetSyntaxNodeFromCodePosition(codePosition, originalCode),
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
                            errors.Add(new Error(Diagnostics.SSIL204_OtherLocalizedError, GetSyntaxNodeFromCodePosition(codePosition, originalCode),
                                errorText));
                        }
                    }
                    else
                    {
                        errors.Add(new Error(Diagnostics.SSIL202_BackendUnknownLine, null, line.Trim()));
                    }
                }
                nextline:;
            }

            return errors;
        }

        private static SyntaxNode GetSyntaxNodeFromCodePosition(string codePosition, Silvernode originalCode)
        {
            Match m = regexCodePosition.Match(codePosition);
            if (m.Success)
            {
                // ReSharper disable once UnusedVariable
                string codefile = m.Groups[1].Value;
                string lineString = m.Groups[2].Value;
                string columnString = m.Groups[3].Value;
                int line = Int32.Parse(lineString); // < Silver line
                int column = Int32.Parse(columnString); // < Silver column

                string silvercode = originalCode.ToString();
                string[] lines = silvercode.Split('\n');
                int position = 0; // < Let's count the position from the start of the text
                for (int i = 0; i < line-1; i++)
                {
                    position += lines[i].Length + 1;
                }
                position += column-1;
                SyntaxNode syntaxNode = originalCode.GetSyntaxNodeFromOffset(position);
                return syntaxNode;
            }
            else
            {
                return null;
            }
        }
    }
}
