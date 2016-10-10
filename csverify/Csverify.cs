using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sharpsilver.Translation;
using Mono.Options;
using Sharpsilver.Translation.BackendInterface;
using Sharpsilver.Translation.Trees.Silver;
// ReSharper disable RedundantDefaultMemberInitializer

namespace Sharpsilver.Cs2Sil
{
    /// <summary>
    /// This class is compiled into the csverify.exe executable which takes C# code files, produces Silver files and verifies them for formal correctness. 
    /// </summary>
    internal class Csverify
    {
        /// <summary>
        /// Whether additional messages about the translation process should be printed.
        /// </summary>
        private static bool verbose = false;
        /// <summary>
        /// In quiet mode, some messages are suppressed.
        /// </summary>
        private static bool quiet = false;
        /// <summary>
        /// Whether the program should terminate normally or wait for the user to press a key after it completes.
        /// </summary>
        private static bool waitAfterwards = false;
        /// <summary>
        /// Whether only classes and members tagged with <see cref="Sharpsilver.Contracts.VerifiedAttribute"/> should be translated into Silver.
        /// If false, then all classes and members not tagged with <see cref="Sharpsilver.Contracts.UnverifiedAttribute"/> will be translated. 
        /// </summary>
        private static bool onlyAnnotated = false;
        /// <summary>
        /// Whether Silicon is used to verify correctness.
        /// </summary>
        private static bool useSilicon = false;
        /// <summary>
        /// Whether Carbon is used to verify correctness. 
        /// </summary>
        private static bool useCarbon = false;
        /// <summary>
        /// Filename where the Silver output should be written out. If null, then the Silver output is not written to disk.
        /// </summary>
        private static string outputSilverFile = null;
        /// <summary>
        /// Current version of this assembly.
        /// </summary>
        private static string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        /// <summary>
        /// Whether line numbers should be printed when printed the resultant Silver code
        /// </summary>
        private static bool lineNumbers = true;
        /// <summary>
        /// Name and short description of this assembly.
        /// </summary>
        private static string header = AppDomain.CurrentDomain.FriendlyName + " " + version + "\n" +
            "Verifies C# code files for correctness with respect to specified verification conditions.";

        private static int Main(string[] args)
        {
            bool showHelp = false;
            bool showVersion = false;

            var references = new List<string>();    
            var assumedFiles = new List<string>(); 
            var verifiedFiles = new List<string>();

            var optionset = new OptionSet()
                .Add("?|help|h", "Shows this message.", option => showHelp = option != null)
                .Add("v|version", $"Shows that the version of this program is {version}.", option => showVersion = option != null)
                .Add("V|verbose", "Enables verbose mode. In verbose mode, additional debugging information is printed and more details are written for each error message.", option => verbose = option != null)
                .Add("q|quiet", "Enable quiet mode. In quiet mode, only the resulting Silver code or error messages are shown.", option => quiet = option != null)
                .Add("r|reference=", "Adds the {ASSEMBLY.DLL} file as a reference when doing semantic analysis on the code. mscorlib and Sharpsilver.Contracts are added automatically.", filename => references.Add(filename))
                .Add("a|assume=", "Translates the file {CLASS.CS} to Silver and prepends it to the main generated file, but its methods and functions won't be verified - their postconditions will be assumed to be true. ", filename => assumedFiles.Add(filename))   
                .Add("w|wait", "When the program finishes, it will wait for the user to press any key before terminating.", option => waitAfterwards = option != null)
                .Add("O|only-annotated", "Only transcompile classes that have the [Verified] attribute, and static methods that have the [Verified] attribute even if their containing classes don't have the [Verified] attribute." , option => onlyAnnotated = option != null)
                .Add("o|output-file=", "Print the resulting Silver code into the {OUTPUT.SIL} file.", filename => outputSilverFile = filename)
                .Add("line-numbers", "Print line numbers before the Silver code", ln => lineNumbers = ln != null)
                .Add("s|silicon", "Use the Silicon backend to verify the Silver code. Use the  \"-s-\" option to disable Silicon verification.", option => useSilicon = option != null)
                .Add("c|carbon", "Use the Carbon backend to verify the Silver code. Use the  \"-c-\" option to disable Carbon verification.", option => useCarbon = option != null)
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
                return (int)ExitCode.InputError;
            }
            if (showVersion)
            {
                Console.WriteLine(header);
                return (int)ExitCode.Success;
            }
            if (showHelp || args.Length == 0)
            {
                WriteHelp(optionset);
                return (int)ExitCode.Success;
            }
            if (verbose && quiet)
            {
                Console.WriteLine("Verbose and quiet mode are not compatible.");
                return (int)ExitCode.InputError;
            }
            if (useCarbon && useSilicon)
            {
                Console.WriteLine("It's not possible to use both Carbon and Silicon during the same run.");
                return (int)ExitCode.InputError;
            }
            if (verifiedFiles.Count < 1)
            {
                Console.WriteLine("You must specify at least 1 file to verify.");
                return (int)ExitCode.InputError;
            }

            if (verbose)
            {
                Console.WriteLine(header);
                Console.WriteLine();
            }

            int result = (int)RunVerification(verifiedFiles, assumedFiles, references);

            if (waitAfterwards)
            {
                Console.WriteLine();
                Console.WriteLine("Work complete. Press any key to exit...");
                Console.ReadKey();
            }

            return result;
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

        private static ExitCode RunVerification(
            List<string> verifiedFiles,
            List<string> assumedFiles,
            List<string> references)
        {
            TranslationProcess process;
            try
            {
                process = TranslationProcess.Create(
                    verifiedFiles,
                    assumedFiles,
                    references,
                    new TranslationConfiguration()
                    {
                        Verbose = verbose,
                        VerifyUnmarkedItems = !onlyAnnotated
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine("The translation process could not be created.");
                Console.WriteLine("Error: " + ex.Message);
                return ExitCode.InputError;
            }

            TranslationProcessResult result = process.Execute();

            if (!quiet)
            {
                Console.WriteLine(
                    result.WasTranslationSuccessful ? "Successfully translated." : $"Translation failed with {result.Errors.Count} errors."
                    );
                Console.WriteLine();
                Console.WriteLine("Resultant Silver code: ");
                Console.WriteLine("=======================");
            }
            string silvercode = result.Silvernode.ToString();
            if (lineNumbers)
            {
                int lineId = 1;
                foreach (var line in silvercode.Split('\n'))
                {
                    Console.WriteLine(lineId + ": " + line);
                    lineId++;
                }
            } else
            {
                Console.WriteLine(silvercode);
            }
            if (result.Errors.Count > 0) {
                if (!quiet)
                {
                    Console.WriteLine("=======================");
                    Console.WriteLine($"Errors: {result.Errors.Count}.");
                }
                foreach (Error error in result.Errors)
                {
                    Console.WriteLine(error.ToString());
                    if (verbose)
                    {
                        if (!String.IsNullOrEmpty(error.Diagnostic.Details))
                        {
                            Console.WriteLine("Details: " + error.Diagnostic.Details);
                        }
                        Console.WriteLine("Node source: " + error.Node.GetText().ToString().Trim());
                        Console.WriteLine();
                    }
                }
                if (!quiet)
                {
                    Console.WriteLine("=======================");
                }
            }
            // Write output to file
            if (outputSilverFile != null)
            {
                try
                {
                    System.IO.File.WriteAllText(outputSilverFile, silvercode);
                    if (!quiet)
                    {
                        Console.WriteLine($"Silver code written to {outputSilverFile}.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine(
                        $"Error - Silver code could not be written to {outputSilverFile}. Check that you have write permissions to that location and that the directory leading up to it exists.");
                    return ExitCode.VerificationError;
                }
            }
            // Run verifier
            if (useSilicon || useCarbon)
            {
                if (!quiet)
                {
                    Console.WriteLine("=======================");
                }

                if (result.WasTranslationSuccessful)
                {
                    IBackend backend;
                    if (useSilicon) backend = new SiliconBackend();
                    else backend = new CarbonBackend();

                    var verificationResult = backend.Verify(result.Silvernode);
                    if (verbose)
                    {
                        Console.WriteLine(verificationResult.OriginalOutput);
                        Console.WriteLine("=======================");
                    }
                    if (verificationResult.VerificationSuccessful)
                    {
                        Console.WriteLine("Verification successful.");
                    }
                    if (verificationResult.Errors.Count > 0)
                    {
                        if (!quiet)
                        {
                            Console.WriteLine($"Errors: {verificationResult.Errors.Count}.");
                        }
                        foreach (Error error in verificationResult.Errors)
                        {
                            Console.WriteLine(error.ToString());
                            if (verbose)
                            {
                                if (!String.IsNullOrEmpty(error.Diagnostic.Details))
                                {
                                    Console.WriteLine("Details: " + error.Diagnostic.Details);
                                }
                                Console.WriteLine("Node source: " + error.Node.GetText().ToString().Trim());
                                Console.WriteLine();
                            }
                        }
                    }
                    if (verificationResult.VerificationSuccessful)
                    {
                        return ExitCode.Success;
                    }
                    else
                    {
                        return ExitCode.VerificationError;
                    }
                }
                else
                {
                    Console.WriteLine("The translation was not successful so a backend verifier will not be run.");
                    return ExitCode.VerificationError;
                }
            }
            else if (result.WasTranslationSuccessful)
            {
                return ExitCode.Success;
            }
            else
            {
                return ExitCode.VerificationError;
            }
        }
    }
}
