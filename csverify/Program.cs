using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cs2Sil.Translation;

namespace Cs2Sil.StandaloneVerifier
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("You must supply exactly one argument.");
                return;
            }
            string csharpFilename = args[0];
            try {
                if (!System.IO.File.Exists(csharpFilename))
                {
                    Console.WriteLine("The first argument must be a C# file.");
                    return;
                }
                string csharpCode = System.IO.File.ReadAllText(csharpFilename);
                TranslationProcess translation = new TranslationProcess(csharpCode);
                TranslationResult result = translation.Translate();
                Console.WriteLine(result.WasTranslationSuccessful ? "Successfully translated." : $"Translation failed with {result.Errors.Count} errors.");
                Console.WriteLine("Resultant Silver code: ");
                Console.WriteLine("=======================");
                Console.WriteLine(result.GetSilverCodeAsString());
                foreach (Error error in result.Errors)
                {
                    Console.WriteLine(error.ToString());
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine($"The file '{args[0]}' could not be read.");
                return;
            }
        }
    }
}
