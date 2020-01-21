using EquationCanonizer.Tools;
using NUnit.Framework;

namespace EquationCanonizer.Tests
{
    /// <summary>
    /// Tests for general correctness.
    /// </summary>
    [TestFixture]
    public class EquationCanonizerTests
    {
        /// <summary>
        /// Tests for the correctness of the final result. Parentheses and constants have been tried.
        /// </summary>
        /// <param name="equation">Equation string.</param>
        /// <returns>Canonized equation string.</returns>
        [TestCase("x^2 + 3.5xy + y = -4 + y^2 - xy + y", ExpectedResult = "x^2 + 4.5xy + 4 - y^2 = 0")]
        [TestCase("-2x^2 + 3.5xy + y = -4 + y^2 - xy + y", ExpectedResult = "-2x^2 + 4.5xy + 4 - y^2 = 0")]
        [TestCase("2x^2 + 3.5xy + y = -4 + y^2 - xy + y + 2x", ExpectedResult = "2x^2 + 4.5xy + 4 - y^2 - 2x = 0")]
        [TestCase("2x^2 + (3.5xy + y) - 1 = 1 + 2y^2", ExpectedResult = "2x^2 + 3.5xy + y - 2 - 2y^2 = 0")]
        [TestCase("2x^2 - (3.5xy + y) - 1 = 1 + 2y^2", ExpectedResult = "2x^2 - 3.5xy - y - 2 - 2y^2 = 0")]
        // Let's try something weird.
        [TestCase("2 - 1 + x = 1 + x + x + y", ExpectedResult = "-x - y = 0")]
        [TestCase("0 + xy - 3xy + (13 - 1) = 2x^2", ExpectedResult = "12 - 2xy - 2x^2 = 0")]
        public string EnsureEquationCanonizerCorrectness(string equation)
        {
            var tokenizer = new Tokenizer();
            var simplifier = new Simplifier();
            var prettifier = new Prettifier();

            var tokens = tokenizer.SplitEquationIntoTokens(equation);
            var simplifiedTokens = simplifier.SimplifyEquationTokens(tokens);

            return prettifier.CombineTokensToEquationString(simplifiedTokens);
        }
    }
}
