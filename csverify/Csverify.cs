using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sharpsilver.Translation;
using Mono.Options;
using Sharpsilver.Translation.BackendInterface;

namespace Sharpsilver.StandaloneVerifier
{
    class Csverify
    {
        private static bool Verbose;
        private static bool WaitAfterwards;
        static bool OnlyAnnotated = false;
        static bool UseSilicon = false;
        static bool UseCarbon = true;
        static string outputSilverFile = null;

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
                .Add("O|only-annotated", "Only transcompile classes that have the [Verified] attribute, and static methods that have the [Verified] attribute even if their containing classes don't have the [Verified] attribute." , option => OnlyAnnotated = option != null)
                .Add("o|output-file=", "Print the resulting Silver code into the {OUTPUT.SIL} file.", filename => outputSilverFile = filename)
                .Add("s|silicon", "Use the Silicon backend to verify the Silver code. Use the  \"-s-\" option to disable Silicon verification.", option => UseSilicon = option != null)
                .Add("c|carbon", "Use the Carbon backend to verify the Silver code. Use the  \"-c-\" option to disable Carbon verification.", option => UseCarbon = option != null)
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
                return (int)ErrorCode.Error;
            }
            if (showVersion)
            {
                Console.WriteLine(header);
                return (int)ErrorCode.Success;
            }
            if (showHelp || args.Length == 0)
            {
                WriteHelp(optionset);
                return (int)ErrorCode.Success;
            }
            if (verifiedFiles.Count < 1)
            {
                Console.WriteLine("You must specify at least 1 file to verify.");
                return (int)ErrorCode.Error;
            }
            return (int)RunVerification(verifiedFiles, assumedFiles, references);
        }

        private static void WriteHelp(OptionSet optionSet)
        {
            Console.WriteLine(header);
            Console.WriteLine();
            Console.WriteLine("Usage: " + AppDomain.CurrentDomain.FriendlyName + " [OPTIONS] file1.cs [file2.cs ...]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            optionSet.WriteOptionDescriptions(Console.Out);
            Console.WriteLine();
            Console.WriteLine("By default, the Carbon backend is used.");
        }

        private static ErrorCode RunVerification(
            List<string> verifiedFiles,
            List<string> assumedFiles,
            List<string> references)
        {
            // TODO verify multiple codefiles
            string csharpFilename = verifiedFiles[0];
            Console.WriteLine($"Csverify will now verify '{csharpFilename}'.");
            Console.WriteLine();
            try
            {
                if (!System.IO.File.Exists(csharpFilename))
                {
                    Console.WriteLine("The first argument must be a C# file.");
                    return ErrorCode.Error;
                }
                if (Verbose) Console.WriteLine("- Reading file from disk.");
                string csharpCode = System.IO.File.ReadAllText(csharpFilename);

                var translation = new TranslationProcess();
                var result = translation.TranslateCode(csharpCode, Verbose);

                Console.WriteLine(
                    result.WasTranslationSuccessful ? "Successfully translated." : $"Translation failed with {result.Errors.Count} errors."
                    );
                Console.WriteLine();
                Console.WriteLine("Resultant Silver code: ");
                Console.WriteLine("=======================");
                string silvercode = result.GetSilverCodeAsString();
                Console.WriteLine(silvercode);
                Console.WriteLine("=======================");
                Console.WriteLine($"Errors: {result.Errors.Count}.");
                foreach (Error error in result.Errors)
                {
                    Console.WriteLine(error.ToString());
                    if (Verbose)
                    {
                        Console.WriteLine("Details: " + error.Diagnostic.Details);
                        Console.WriteLine();
                    }
                }
                // Write output to file
                if (outputSilverFile != null)
                {
                    Console.WriteLine("=======================");
                    try
                    {
                        System.IO.File.WriteAllText(outputSilverFile, silvercode);
                        Console.WriteLine($"Silver code written to {outputSilverFile}.");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(
                            $"Error - Silver code could not be written to {outputSilverFile}. Check that you have write permissions to that location and that the directory leading up to it exists.");
                        return ErrorCode.Error;
                    }
                }
                // Run verifier
                if (UseSilicon || UseCarbon)
                {
                    Console.WriteLine("=======================");
                    if (result.WasTranslationSuccessful)
                    {
                        IBackend backend;
                        if (UseSilicon) backend = new SiliconBackend();
                        else backend = new CarbonBackend();

                        var verificationResult = backend.Verify(result.Silvernode);
                        Console.WriteLine(verificationResult.OriginalOutput);
                        Console.WriteLine("=======================");
                        Console.WriteLine($"Errors: {verificationResult.Errors.Count}.");
                        foreach (Error error in verificationResult.Errors)
                        {
                            Console.WriteLine(error.ToString());
                            if (Verbose)
                            {
                                Console.WriteLine("Details: " + error.Diagnostic.Details);
                                Console.WriteLine();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("The translation was not successful so a backend will not be run.");
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
                    return ErrorCode.Success;
                }
                else
                {
                    return ErrorCode.Error;
                }
            }
            catch (System.IO.IOException exception)
            {
                Console.WriteLine($"The file '{csharpFilename}' could not be read.");
                if (Verbose)
                {
                    Console.WriteLine(exception.ToString());
                }
                return ErrorCode.Error;
            }
        }
    }
}
