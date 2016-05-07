using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Cs2Sil.Translation
{
    public class Error
    {
        public string Caption;
        public string ErrorCode;
        public string CsharpFilename;
        public int CsharpColumn;
        public int CsharpLine;
        
        private SyntaxNode node;

        public Error(string code, string text, SyntaxNode node)
        {
            this.ErrorCode = code;
            this.Caption = text;
            this.node = node;
        }

        public override string ToString()
        {
            return ErrorCode + ": " + Caption + " (line " + 
                (node.GetLocation().GetLineSpan().StartLinePosition.Line + 1) + ")";
        }
    }
}
