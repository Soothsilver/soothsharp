using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation
{
    public class SharpsilverDiagnostic
    {
        private SharpsilverDiagnostic(string errorCode, string caption, string details, DiagnosticSeverity severity)
        {
            this.ErrorCode = errorCode;
            this.Caption = caption;
            this.Details = details;
            this.Severity = severity;
        }

        public string ErrorCode { get; }
        public string Caption { get; }
        public string Details { get; }
        public DiagnosticSeverity Severity { get; }


        public static SharpsilverDiagnostic Create(
            string errorCode, 
            string caption, 
            string details, DiagnosticSeverity severity)
        {
            SharpsilverDiagnostic sd = new SharpsilverDiagnostic(errorCode, caption, details, severity);
            return sd;
        }
        
    }
    public class Diagnostics
    {
        public static SharpsilverDiagnostic SSIL101_UnknownNode =
            SharpsilverDiagnostic.Create(
                "SSIL101",
                "The Sharpsilver translator does not support elements of the syntax kind '{0}'.",
                "A syntax node of this kind cannot be translated by the Sharpsilver translator because the feature it provides is unavailable in Silver, or because it is difficult to translate. If you can use a less advanced construct, please do so.",
                DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL102_UnexpectedNode =
            SharpsilverDiagnostic.Create(
                "SSIL102",
                "An element of the syntax kind '{0}' is not expected at this code location.",
                "While the Sharpsilver translator might otherwise be able to handle this kind of C# nodes, this is not a place where it is able to do so. There may be an error in your C# syntax (check compiler errors) or you may be using C# features that the translator does not understand.",
                DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL103_ExceptionConstructingCSharp =
            SharpsilverDiagnostic.Create(
                "SSIL103",
                "An exception ('{0}') occured during the construction of the C# abstract syntax tree.",
                "While this is an internal error of the translator, it mostly occurs when there is a C# syntax or semantic error in your code. Try to fix any other compiler errors and maybe this issue will be resolved.",
                DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL104_ExceptionConstructingSilver =
            SharpsilverDiagnostic.Create(
                "SSIL104",
                "An exception ('{0}') occured during the construction of the Silver abstract syntax tree.",
                "While this is an internal error of the translator, it mostly occurs when there is a C# syntax or semantic error in your code. Try to fix any other compiler errors and maybe this issue will be resolved.",
                DiagnosticSeverity.Error);

        public static IEnumerable<SharpsilverDiagnostic> GetAllDiagnostics()
        {
            Type t = typeof(Diagnostics);
            var fs = t.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach(var f in fs)
            {
                object diagnostic = f.GetValue(null);
                yield return diagnostic as SharpsilverDiagnostic;
            }
        }
    }
}
