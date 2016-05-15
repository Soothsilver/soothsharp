using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Translation;
using Mono.Options;

namespace Sharpsilver.StandaloneVerifier
{
    class Csverify
    {
        static bool Verbose = false;
        static bool WaitAfterwards = false; 
        static string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        static string header = System.AppDomain.CurrentDomain.FriendlyName + " " + version + "\n" + "Verifies C# code files for correctness with respect to specified verification conditions.";

        static int Main(string[] args)
        {
            bool showHelp = false;
            bool showVersion = false;

            var references = new List<string>();    // TODO take into account
            var assumedFiles = new List<string>();  // TODO take into account
            var verifiedFiles = new List<string>(); // TODO take into account

            var optionset = new OptionSet()
                .Add("?|help|h", "Shows this message.", option => showHelp = option != null)
                .Add("v|version", $"Shows that the version of this program is {version}.", option => showVersion = option != null)
                .Add("V|verbose", "Enables verbose mode. In verbose mode, additional debugging information is printed and more details are written for each error message.", option => Verbose = option != null)
                .Add("r|reference=", "Adds the {ASSEMBLY.DLL} file as a reference when doing semantic analysis on the code. mscorlib and Sharpsiler.Contracts are added automatically.", filename => references.Add(filename))
                .Add("a|assume=", "Translates the file {CLASS.CS} to Silver and prepends it to the main generated file, but its methods and functions won't be verified - their postconditions will be assumed to be true. ", filename => assumedFiles.Add(filename))   
                .Add("w|wait", "When the program finishes, it will wait for the user to press any key before terminating.", option => WaitAfterwards = option != null)
                ;
            try
            {
                verifiedFiles.AddRange(optionset.Parse(args));
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while parsing command line arguments.");
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine();
                WriteHelp(optionset);
                return (int)ErrorCode.ERROR;
            }
            if (showVersion)
            {
                Console.WriteLine(header);
                return (int)ErrorCode.SUCCESS;
            }
            if (showHelp || args.Length == 0)
            {
                WriteHelp(optionset);
                return (int)ErrorCode.SUCCESS;
            }
            if (verifiedFiles.Count < 1)
            {
                Console.WriteLine("You must specify at least 1 file to verify.");
                return (int)ErrorCode.ERROR;
            }
            string csharpFilename = args[0];
            return (int)RunVerification(verifiedFiles, assumedFiles, references);
        }

        private static void WriteHelp(OptionSet optionSet)
        {
            Console.WriteLine(header);
            Console.WriteLine();
            Console.WriteLine("Usage: " + System.AppDomain.CurrentDomain.FriendlyName + " [OPTIONS] file1.cs [file2.cs ...]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            optionSet.WriteOptionDescriptions(Console.Out);
        }

        private static ErrorCode RunVerification(List<string> verifiedFiles, List<string> assumedFiles, List<string> references)
        {
            string csharpFilename = verifiedFiles[0];
            Console.WriteLine($"Sharpsilver Standalone Verifier will now verify '{csharpFilename}'.");
            Console.WriteLine();
            try
            {
                if (!System.IO.File.Exists(csharpFilename))
                {
                    Console.WriteLine("The first argument must be a C# file.");
                    return ErrorCode.ERROR;
                }
                if (Verbose) Console.WriteLine("- Reading file from disk.");
                string csharpCode = System.IO.File.ReadAllText(csharpFilename);

                var translation = new TranslationProcess();
                var result = translation.TranslateCode(csharpCode, Verbose);

                Console.WriteLine(
                    result.WasTranslationSuccessful ? "Successfully translated." : $"Translation failed with {result.ReportedDiagnostics.Count} errors."
                    );
                Console.WriteLine();
                Console.WriteLine("Resultant Silver code: ");
                Console.WriteLine("=======================");
                Console.WriteLine(result.GetSilverCodeAsString());
                Console.WriteLine("=======================");
                Console.WriteLine($"Errors: {result.ReportedDiagnostics.Count}.");
                foreach (Error error in result.ReportedDiagnostics)
                {
                    Console.WriteLine(error.ToString());
                    if (Verbose)
                    {
                        Console.WriteLine("Details: " + error.Diagnostic.Details);
                        Console.WriteLine();
                    }
                }
                if (WaitAfterwards)
                {
                    Console.WriteLine();
                    Console.
                        WriteLine("Press any key to exit...");
                    Console.ReadKey();
                }
                if (result.WasTranslationSuccessful)
                {
                    return ErrorCode.SUCCESS;
                }
                else
                {
                    return ErrorCode.ERROR;
                }
            }
            catch (System.IO.IOException exception)
            {
                Console.WriteLine($"The file '{csharpFilename}' could not be read.");
                if (Verbose)
                {
                    Console.WriteLine(exception.ToString());
                }
                return ErrorCode.ERROR;
            }
        }
    }
}
