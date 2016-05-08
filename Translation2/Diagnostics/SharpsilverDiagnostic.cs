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
        public static SharpsilverDiagnostic SSIL101 =
            SharpsilverDiagnostic.Create(
                "SSIL101",
                "The Sharpsilver translator does not support elements of the syntax kind '{0}'.",
                "A syntax node of this kind cannot be translated by the Sharpsilver translator because the feature it provides is unavailable in Silver or because it is difficult to translate. If you can use a less advanced construct, please do so. Otherwise, you may mark the enclosing method or class with the [Unverified] attribute.",
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
