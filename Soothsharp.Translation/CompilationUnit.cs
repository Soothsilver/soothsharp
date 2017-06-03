using Microsoft.CodeAnalysis.CSharp;

namespace Soothsharp.Translation
{
    /// <summary>
    /// Represents a C# source file, along with information on the style of how it should be translated to Viper.
    /// </summary>
    public class CompilationUnit
    {
        public CSharpSyntaxTree RoslynTree { get; private set; }
        public CompilationUnitVerificationStyle Style { get;  private set; }

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
        /// <summary>
        /// Methods in this compilation unit will be translated normally.
        /// </summary>
        FullVerification,
        /// <summary>
        /// Methods in this compilation unit (C# file) will be translated as abstract in Viper.
        /// </summary>
        ContractsOnly
    } 
}
