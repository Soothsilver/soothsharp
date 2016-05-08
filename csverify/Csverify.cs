using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Translation;

namespace Sharpsilver.StandaloneVerifier
{
    class Csverify
    {
        static bool Verbose = true;
        static int Main(string[] args)
        {
            // TODO use a library for argument parsing
            /* Necessary arguments:
             * [filename.cs] - the file to verify
             * [assumedfilenames.cs] - files that are being used but are assumed to be verified
             * [otherfilenames.cs] - files to verify, if more than one
             * [siliconexepath] - path to silicon.exe
             * /?, -?, -h, /h, --help
             * -verbose
             */
            if (args.Length != 1)
            {
                Console.WriteLine("You must supply exactly one argument.");
                return (int)ErrorCode.ERROR;
            }
            string csharpFilename = args[0];
            return (int)RunVerification(csharpFilename);
        }

        private static ErrorCode RunVerification(string csharpFilename)
        {
            Console.WriteLine($"Sharpsilver Standalone Verifier will now verify '{csharpFilename}'.");
            try
            {
                if (!System.IO.File.Exists(csharpFilename))
                {
                    Console.WriteLine("The first argument must be a C# file.");
                    return ErrorCode.ERROR;
                }
                string csharpCode = System.IO.File.ReadAllText(csharpFilename);

                TranslationProcess translation = new TranslationProcess();
                TranslationResult result = translation.TranslateCode(csharpCode);

                Console.WriteLine(
                    result.WasTranslationSuccessful ? "Successfully translated." : $"Translation failed with {result.ReportedDiagnostics.Count} errors."
                    );
                Console.WriteLine("Resultant Silver code: ");
                Console.WriteLine("=======================");
                Console.WriteLine(result.GetSilverCodeAsString());
                Console.WriteLine("=======================");
                Console.WriteLine($"Errors: {result.ReportedDiagnostics.Count}.");
                foreach (Error error in result.ReportedDiagnostics)
                {
                    Console.WriteLine(error.ToString());
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return ErrorCode.SUCCESS;
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
