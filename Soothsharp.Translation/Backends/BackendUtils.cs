using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.BackendInterface
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
            harmlessLines.Add(new Regex("Silicon finished in .* seconds."));
            harmlessLines.Add(new Regex("carbon finished in .* seconds."));
            harmlessLines.Add(new Regex(@"^\W*$"));
            harmlessLines.Add(new Regex(@"\(c\).*ETH Zurich.*"));
            harmlessLines.Add(new Regex("Silicon 1.1-SNAPSHOT .*"));
            harmlessLines.Add(new Regex(@"\(c\) 2013 ETH Zurich .*"));
            harmlessLines.Add(new Regex("carbon 1.0.*"));
            harmlessLines.Add(new Regex("No errors found."));
            harmlessLines.Add(new Regex("The following errors were found:"));
        }

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

        public static SyntaxNode GetSyntaxNodeFromCodePosition(string codePosition, Silvernode originalCode)
        {
            Match m = regexCodePosition.Match(codePosition);
            if (m.Success)
            {
                // ReSharper disable once UnusedVariable
                string codefile = m.Groups[1].Value;
                string lineString = m.Groups[2].Value;
                string columnString = m.Groups[3].Value;
                int line = Int32.Parse(lineString);
                int column = Int32.Parse(columnString);
                // tut, tut, here we must do something smart.
                string silvercode = originalCode.ToString();
                string[] lines = silvercode.Split('\n');
                int position = 0;
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
