using System;
using System.Collections.Generic;

namespace Sharpsilver.Translation
{
    /// <summary>
    /// This class contains static constants that represent the various errors and warnings Sharpsilver might generate.
    /// <para>
    /// Error codes 100-199 are translation errors that mean that a program cannot be translated into Silver.
    /// </para>
    /// <para>
    /// Error codes 200-299 are verification warnings that mean that the backend does not guarantee correctness of the code.
    /// </para>
    /// <para>
    /// Error codes 300-399 are internal errors of the transcompiler.
    /// </para>
    /// </summary>
    public class Diagnostics
    {
        // *********************************** 100 Translation Errors 
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
        public static SharpsilverDiagnostic SSIL105_FeatureNotYetSupported =
            SharpsilverDiagnostic.Create(
                "SSIL105",
                "This feature ({0}) is not yet supported.",
                "As the C#-to-Silver translation project is developed, we plan to allow this feature to be used in verifiable C# class files. For now, however, it is unsupported and won't work.",
                DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL106_TypeNotSupported =
            SharpsilverDiagnostic.Create(
                "SSIL106",
                "The type {0} is not supported in Silver.",
                "The Silver language can only use an integer, a boolean and reference objects. Other value types besides these three cannot be translated.",
                DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL107_ThisExpressionCannotBeStatement =
            SharpsilverDiagnostic.Create(
                "SSIL107",
                "This expression cannot form an expression statement in Silver.",
                "The Silver language does not support this expression as a standalone expression in a statement, even if C# supported it.",
                DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL108_FeatureNotSupported =
          SharpsilverDiagnostic.Create(
              "SSIL108",
              "This feature ({0}) is not supported.",
              "This feature of C# is not supported by the translator, and will probably never be supported. Could you please try to replace it with less advanced C# features?",
              DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL109_FeatureNotSupportedBecauseSilver =
          SharpsilverDiagnostic.Create(
              "SSIL109",
              "This feature ({0}) is not supported by Silver.",
              "This feature of C# cannot be meaningfully represented in Silver.",
              DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL110_InvalidSyntax =
          SharpsilverDiagnostic.Create(
              "SSIL110",
              "Syntax is invalid ({0}).",
              "This feature of C# cannot be meaningfully represented in Silver.",
              DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL111_NonStatement =
      SharpsilverDiagnostic.Create(
          "SSIL111",
          "\"{0}\" is not a statement silvernode.",
          "",
          DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL112_FileNotFound =
     SharpsilverDiagnostic.Create(
         "SSIL112",
         "A C# file or reference could not be loaded ({0})",
         "",
         DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL113_VerificationSettingsContradiction =
     SharpsilverDiagnostic.Create(
         "SSIL113",
         "This is marked both [Verified] and [Unverified]. Which do you want?",
         "",
         DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL114_NotPureContext =
  SharpsilverDiagnostic.Create(
      "SSIL114",
      "This ({0}) cannot be translated into a pure assertion.",
      "In this context, C# code is forced to be translated into pure Silver assertions. However, this C# node cannot be translated in a pure way.",
      DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL115_ThisIntegerSizeNotSupported =
          SharpsilverDiagnostic.Create(
              "SSIL115",
              "Use System.Int32 instead of {0}.",
              "The Silver language's integers are unbounded. To prevent confusion, use only 'System.Int32' integers, please. Your actual used type wouldn't matter anyway, since Viper does not check for overflow or underflow.",
              DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL116_MethodAttributeContradiction =
         SharpsilverDiagnostic.Create(
            "SSIL116",
            "Constructors and predicates cannot be declared [Pure].",
            "The [Pure] attribute causes the C# method to be translated as a Viper function. Viper predicates can't be functions. Constructors, in the transcompiler's view, also cannot be functions.",
            DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL117_ConstructorMustNotBeAbstract =
         SharpsilverDiagnostic.Create(
           "SSIL117",
           "Constructors cannot be declared [Abstract].",
           "",
        DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL118_FunctionsMustHaveAReturnType =
         SharpsilverDiagnostic.Create(
           "SSIL118",
           "Methods declared [Pure] must have a non-void return type.",
           "In Viper, functions always return a value. Methods with the [Pure] attribute are translated as a functions and thus can't have the void return type. ",
           DiagnosticSeverity.Error);
        public static SharpsilverDiagnostic SSIL119_PredicateMustBeBool =
         SharpsilverDiagnostic.Create(
           "SSIL119",
           "Methods declared [Predicate] must have the boolean return type.",
           "Methods with the [Predicate] attribute are translated as Viper predicates. Predicates are either spatial or boolean - either way, the return type must be System.Boolean.",
           DiagnosticSeverity.Error);
        // ****************************** 200 Backend Verifier Errors
        public static SharpsilverDiagnostic SSIL201_BackendNotFound =
            SharpsilverDiagnostic.Create(
                "SSIL201",
                "Back-end ({0}) not found.",
                "The back-end chosen to verify the translated Silver code was not found in PATH nor in the local directory and so the code was not verified.",
                DiagnosticSeverity.Warning);

        public static SharpsilverDiagnostic SSIL202_BackendUnknownLine =
            SharpsilverDiagnostic.Create(
                "SSIL202",
                "Backend: {0}",
                "This line was returned by the backend but Sharpsilver does not recognize it.",
                DiagnosticSeverity.Warning);

        public static SharpsilverDiagnostic SSIL203_ParseError =
            SharpsilverDiagnostic.Create(
                "SSIL203",
                "Silver parse error: {0}",
                "This C# code was transformed into a Silver segment that does not conform to Silver grammar and therefore the code could not be verified. This should not ordinarily happen and indicates an error in the Sharpsilver transcompiler.",
                DiagnosticSeverity.Warning);

        public static SharpsilverDiagnostic SSIL204_OtherLocalizedError =
        SharpsilverDiagnostic.Create(
            "SSIL204",
            "{0}",
            "",
            DiagnosticSeverity.Warning);

        // ****************************** 300 Internal Errors
        public static SharpsilverDiagnostic SSIL301_InternalLocalizedError =
            SharpsilverDiagnostic.Create(
                "SSIL301",
                "The transcompiler encountered an internal error ({0}) while parsing this.",
                "Try to remove the infringing C# code fragment. You may be forced to make do without that C# feature. You can also submit this as a bug report as this error should never be displayed to the user normally.",
                DiagnosticSeverity.Error);

        public static SharpsilverDiagnostic SSIL302_InternalError =
            SharpsilverDiagnostic.Create(
                "SSIL302",
                "The transcompiler encountered an internal error ({0}).",
                "Try to undo your most recent change to the code as it may be triggering this error. You may be forced to make do without that C# feature. You can also submit this as a bug report as this error should never be displayed to the user normally.",
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