using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation
{
    public class SilverSourceCode
    {
        public List<string> SourceLines = new List<string>();
        public SyntaxNode SourceNode;
        public SilverSourceCode(string silvercode, SyntaxNode sourceNode)
        {
            SourceNode = sourceNode;
            SourceLines = silvercode.Split(new char[] { '\n' }, StringSplitOptions.None).ToList();
        }

        public string GetSilverCode()
        {
            return String.Join("\n", SourceLines);
        }
    }
}
