using EquationCanonizer.Tools;
using System;

namespace EquationCanonizer
{
    /// <summary>
    /// Defines the entry point to the application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            while (true)
            {
                Console.WriteLine("Please enter the equation:");
                var equationString = Console.ReadLine();

                var tokenizer = new Tokenizer();
                var simplifier = new Simplifier();
                var prettifier = new Prettifier();

                var tokens = tokenizer.SplitEquationIntoTokens(equationString);
                var simplifiedTokens = simplifier.SimplifyEquationTokens(tokens);

                Console.WriteLine("Canonized equation:");
                Console.WriteLine(prettifier.CombineTokensToEquationString(simplifiedTokens));
                Console.Write(Environment.NewLine);
            }
        }

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is InvalidOperationException ioe)
            {
                Console.Write(Environment.NewLine);
                Console.WriteLine("Error:");
                Console.WriteLine(ioe.Message);
            }
            else
            {
                Console.WriteLine("An unexpected error occurred while the program was running:");
                Console.WriteLine(e.ExceptionObject.ToString());
            }

            Console.Write(Environment.NewLine);
            Console.WriteLine("Please press Enter to exit the program.");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
