using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming
// These names are based on error codes.
#pragma warning disable 1591

namespace Soothsharp.Translation
{
    /// <summary>
    /// This class contains static constants that represent the various errors and warnings Sootsharp might generate.
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
        public static SoothsharpDiagnostic SSIL101_UnknownNode =
            SoothsharpDiagnostic.Create(
                "SSIL101",
                "The Soothsharp translator does not support elements of the syntax kind '{0}'.",
                "A syntax node of this kind cannot be translated by the Soothsharp translator because the feature it provides is unavailable in Viper, or because it is difficult to translate. If you can use a less advanced construct, please do so.",
                DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL102_UnexpectedNode =
            SoothsharpDiagnostic.Create(
                "SSIL102",
                "An element of the syntax kind '{0}' is not expected at this code location.",
                "While the Soothsharp translator might otherwise be able to handle this kind of C# nodes, this is not a place where it is able to do so. There may be an error in your C# syntax (check compiler errors) or you may be using C# features that the translator does not understand.",
                DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL103_ExceptionConstructingCSharp =
            SoothsharpDiagnostic.Create(
                "SSIL103",
                "An exception ('{0}') occured during the construction of the C# abstract syntax tree.",
                "While this is an internal error of the translator, it mostly occurs when there is a C# syntax or semantic error in your code. Try to fix any other compiler errors and maybe this issue will be resolved.",
                DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL104_ExceptionConstructingSilver =
            SoothsharpDiagnostic.Create(
                "SSIL104",
                "An exception ('{0}') occured during the construction of the Viper abstract syntax tree.",
                "While this is an internal error of the translator, it mostly occurs when there is a C# syntax or semantic error in your code. Try to fix any other compiler errors and maybe this issue will be resolved.",
                DiagnosticSeverity.Error);
        // ReSharper disable once UnusedMember.Global - kept for compatibility and future-proofing
        public static SoothsharpDiagnostic SSIL105_FeatureNotYetSupported =
            SoothsharpDiagnostic.Create(
                "SSIL105",
                "This feature ({0}) is not yet supported.",
                "As the C#-to-Viper translation project is developed, we plan to allow this feature to be used in verifiable C# class files. For now, however, it is unsupported and won't work.",
                DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL106_TypeNotSupported =
            SoothsharpDiagnostic.Create(
                "SSIL106",
                "The type {0} is not supported in Viper.",
                "The Viper language can only use an integer, a boolean and reference objects. Other value types besides these three cannot be translated.",
                DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL107_ThisExpressionCannotBeStatement =
            SoothsharpDiagnostic.Create(
                "SSIL107",
                "This expression cannot form an expression statement in Viper.",
                "The Viper language does not support this expression as a standalone expression in a statement, even if C# supported it.",
                DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL108_FeatureNotSupported =
          SoothsharpDiagnostic.Create(
              "SSIL108",
              "This feature ({0}) is not supported.",
              "This feature of C# is not supported by the translator, and will probably never be supported. Could you please try to replace it with less advanced C# features?",
              DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL109_FeatureNotSupportedBecauseSilver =
          SoothsharpDiagnostic.Create(
              "SSIL109",
              "This feature ({0}) is not supported by Viper.",
              "This feature of C# cannot be meaningfully represented in Viper.",
              DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL110_InvalidSyntax =
          SoothsharpDiagnostic.Create(
              "SSIL110",
              "Syntax is invalid ({0}).",
              "This feature of C# cannot be meaningfully represented in Viper.",
              DiagnosticSeverity.Error);
        // ReSharper disable once UnusedMember.Global - kept for compatibility
        public static SoothsharpDiagnostic SSIL111_NonStatement =
      SoothsharpDiagnostic.Create(
          "SSIL111",
          "\"{0}\" is not a statement silvernode.",
          "",
          DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL112_FileNotFound =
     SoothsharpDiagnostic.Create(
         "SSIL112",
         "A C# file or reference could not be loaded ({0})",
         "",
         DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL113_VerificationSettingsContradiction =
     SoothsharpDiagnostic.Create(
         "SSIL113",
         "This is marked both [Verified] and [Unverified]. Which do you want?",
         "",
         DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL114_NotPureContext =
  SoothsharpDiagnostic.Create(
      "SSIL114",
      "This ({0}) cannot be translated into a pure assertion.",
      "In this context, C# code is forced to be translated into pure Viper assertions. However, this C# node cannot be translated in a pure way.",
      DiagnosticSeverity.Error);
        
        // This error no longer ever triggers -- all integers are now converted to Int, without regard to their size in C#
        public static SoothsharpDiagnostic SSIL115_ThisIntegerSizeNotSupported =
          SoothsharpDiagnostic.Create(
              "SSIL115",
              "[deprecated error] Use System.Int32 instead of {0}.",
              "[no longer true] The Viper language's integers are unbounded. To prevent confusion, use only 'System.Int32' integers, please. Your actual used type wouldn't matter anyway, since Viper does not check for overflow or underflow.",
              DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL116_MethodAttributeContradiction =
         SoothsharpDiagnostic.Create(
            "SSIL116",
            "Constructors and predicates cannot be declared [Pure].",
            "The [Pure] attribute causes the C# method to be translated as a Viper function. Viper predicates can't be functions. Constructors, in the transcompiler's view, also cannot be functions.",
            DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL117_ConstructorMustNotBeAbstract =
         SoothsharpDiagnostic.Create(
           "SSIL117",
           "Constructors cannot be declared [Abstract].",
           "",
        DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL118_FunctionsMustHaveAReturnType =
         SoothsharpDiagnostic.Create(
           "SSIL118",
           "Methods declared [Pure] must have a non-void return type.",
           "In Viper, functions always return a value. Methods with the [Pure] attribute are translated as a functions and thus can't have the void return type. ",
           DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL119_PredicateMustBeBool =
         SoothsharpDiagnostic.Create(
           "SSIL119",
           "Methods declared [Predicate] must have the boolean return type.",
           "Methods with the [Predicate] attribute are translated as Viper predicates. Predicates are either spatial or boolean - either way, the return type must be System.Boolean.",
           DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL120_UndeclaredNameReferenced =
     SoothsharpDiagnostic.Create(
       "SSIL120",
       "Code references the name '{0}' which was not declared.",
       "This error would usually mean that you're using a reference that was not given to the transcompiler or that there is an error in the transcompiler.",
       DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL121_FunctionsAndPredicatesCannotHaveStatements =
   SoothsharpDiagnostic.Create(
     "SSIL121",
     "Methods declared [Pure] or [Predicate] may only contain return statements and contracts.",
     "A method thus declared is translated to a Viper function or predicate. These constructs may have bodies, but these bodies must contain an assertion only, not statements. This is why only contracts (such as Contract.Ensures(...)) and a single return statement are permitted here.",
     DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL122_FunctionsAndPredicatesCannotHaveMoreThanOneReturnStatement =
   SoothsharpDiagnostic.Create(
     "SSIL122",
     "Methods declared [Pure] or [Predicate] must have exactly one return statement.",
     "A method thus declared is translated to a Viper function or predicate. These constructs may have bodies, but these bodies must contain an assertion only, not statements. There can't be any statements, and the single return statement is translated as a Viper assertion. There can't be more than one.",
     DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL123_ThereIsThisCSharpError =
   SoothsharpDiagnostic.Create(
     "SSIL123",
     "A C# compiler diagnostic prevents correct translation: {0}",
     "Only valid C# programs can be translated to Viper. Fix any errors offered by the C# compiler, then attempt translation again.",
     DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL124_QuantifierMustGetLambda =
   SoothsharpDiagnostic.Create(
     "SSIL124",
     "Quantifiers must have a lambda function as an argument.",
     "The contents of the lambda function are converted into Viper code. You cannot use a non-lambda function here.",
     DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL125_LambdasMustHaveSingleParameter =
   SoothsharpDiagnostic.Create(
     "SSIL125",
     "Lambda functions must have a single parameter.",
     "Soothsharp does not allow for lambda functions with more than one parameter.",
     DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL126_LambdasMustBeExpressions =
   SoothsharpDiagnostic.Create(
     "SSIL126",
     "Lambda functions must have an expression body.",
     "Soothsharp does not allow for lambda functions with statement (full) bodies, the body of the function must an expression.",
     DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL127_LambdasOnlyInContracts =
  SoothsharpDiagnostic.Create(
    "SSIL127",
    "Lambda functions can only occur within quantifiers.",
    "Soothsharp does not allow for use of arbitrary lambda functions - you may only use these within quantifiers for syntactic purposes.",
    DiagnosticSeverity.Error);
        public static SoothsharpDiagnostic SSIL128_IndexersAreOnlyForSeqsAndArrays =
  SoothsharpDiagnostic.Create(
    "SSIL128",
    "Element access works only for Seq types and for arrays.",
    "",
    DiagnosticSeverity.Error);

        public static SoothsharpDiagnostic SSIL129_MethodContractsAreOnlyForMethods =
            SoothsharpDiagnostic.Create(
                "SSIL129",
                "Method contracts (Requires and Ensures) must be in a method body, outside of any inner blocks or loops.",
                "It is an error for a Requires or Ensures call to be within a loop. Only invariants are contracts permitted within a loop.",
                DiagnosticSeverity.Error);

        public static SoothsharpDiagnostic SSIL130_InvariantsAreOnlyForLoops=
            SoothsharpDiagnostic.Create(
                "SSIL130",
                "Invariants must be within loops.",
                "It is an error for an Invariant call to be elsewhere than within the code block of a loop (for, while or do).",
                DiagnosticSeverity.Error);

        // TODO (future): Expand this diagnostic so that it triggers more consistently (not just for binary expressions, but assignments, method calls...)
        public static SoothsharpDiagnostic SSIL131_AssignmentsNotInsideExpressions =
            SoothsharpDiagnostic.Create(
                "SSIL131",
                "Assignment expressions must be outermost.",
                "In Viper, expressions with side-effects (such as an assignment) are statements and so can't be within other expressions.",
                DiagnosticSeverity.Error);

        // ****************************** 200 Backend Verifier Errors
        public static SoothsharpDiagnostic SSIL201_BackendNotFound =
            SoothsharpDiagnostic.Create(
                "SSIL201",
                "Back-end ({0}) not found.",
                "The back-end chosen to verify the translated Viper code was not found in PATH nor in the local directory and so the code was not verified.",
                DiagnosticSeverity.Warning);

        public static SoothsharpDiagnostic SSIL202_BackendUnknownLine =
            SoothsharpDiagnostic.Create(
                "SSIL202",
                "Backend: {0}",
                "This line was returned by the backend but Soothsharp does not recognize it.",
                DiagnosticSeverity.Warning);

        public static SoothsharpDiagnostic SSIL203_ParseError =
            SoothsharpDiagnostic.Create(
                "SSIL203",
                "Viper parse error: {0}",
                "This C# code was transformed into a Viper segment that does not conform to Viper grammar and therefore the code could not be verified. This should not ordinarily happen and indicates an error in the Sootsharp transcompiler.",
                DiagnosticSeverity.Warning);

        public static SoothsharpDiagnostic SSIL204_OtherLocalizedError =
        SoothsharpDiagnostic.Create(
            "SSIL204",
            "{0}",
            "",
            DiagnosticSeverity.Warning);

        public static SoothsharpDiagnostic SSIL205_NoErrorsFoundLineNotFound =
       SoothsharpDiagnostic.Create(
           "SSIL205",
           "Verification failed for an unknown reason.",
           "The verifier did not return any errors but it did not certify that the code is correct. This may happen if you don't have Java, Z3 or Boogie installed or if the verifier is set up incorrectly.",
           DiagnosticSeverity.Warning);

        // ****************************** 300 Internal Errors
        public static SoothsharpDiagnostic SSIL301_InternalLocalizedError =
            SoothsharpDiagnostic.Create(
                "SSIL301",
                "The transcompiler encountered an internal error ({0}) while parsing this.",
                "Try to remove the infringing C# code fragment. You may be forced to make do without that C# feature. You can also submit this as a bug report as this error should never be displayed to the user normally.",
                DiagnosticSeverity.Error);

        // ReSharper disable once UnusedMember.Global - kept for futureproofing
        public static SoothsharpDiagnostic SSIL302_InternalError =
            SoothsharpDiagnostic.Create(
                "SSIL302",
                "The transcompiler encountered an internal error ({0}).",
                "Try to undo your most recent change to the code as it may be triggering this error. You may be forced to make do without that C# feature. You can also submit this as a bug report as this error should never be displayed to the user normally.",
                DiagnosticSeverity.Error);



        /// <summary>
        /// Gets all error descriptions that might be outputted by Soothsharp.
        /// </summary>
        public static IEnumerable<SoothsharpDiagnostic> GetAllDiagnostics()
        {
            Type t = typeof(Diagnostics);
            var fs = t.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach(var f in fs)
            {
                object diagnostic = f.GetValue(null);
                yield return diagnostic as SoothsharpDiagnostic;
            }
        }
    }
}