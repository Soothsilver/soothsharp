using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace Sharpsilver.Translation
{
    public class CompilationUnit
    {
        public CSharpSyntaxTree RoslynTree { get; private set; }
        public CompilationUnitVerificationStyle Style { get; private set; }

        private CompilationUnit(CSharpSyntaxTree tree, CompilationUnitVerificationStyle style)
        {
            this.RoslynTree = tree;
            this.Style = style;
        }

        public static CompilationUnit CreateFromFile(string filename, CompilationUnitVerificationStyle style)
        {
            string text = System.IO.File.ReadAllText(filename);
            CSharpSyntaxTree tree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(text);
            return new CompilationUnit(tree, style);
        }
        public static CompilationUnit CreateFromTree(CSharpSyntaxTree tree, CompilationUnitVerificationStyle style)
        {
            return new CompilationUnit(tree, style);
        }
    }
    public enum CompilationUnitVerificationStyle
    {
        FullVerification,
        ContractsOnly
    } 
}
