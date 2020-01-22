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
        [TestCase("x^2 + 3.5xy + y = -4 + y^2 - xy + y", ExpectedResult = "x^2 - y^2 + 4.5xy + 4 = 0")]
        [TestCase("-2x^2 + 3.5xy + y = -4 + y^2 - xy + y", ExpectedResult = "-2x^2 - y^2 + 4.5xy + 4 = 0")]
        [TestCase("2x^2 + 3.5xy + y = -4 + y^2 - xy + y + 2x", ExpectedResult = "2x^2 - y^2 + 4.5xy - 2x + 4 = 0")]
        [TestCase("2x^2 + (3.5xy + y) - 1 = 1 + 2y^2", ExpectedResult = "2x^2 - 2y^2 + 3.5xy + y - 2 = 0")]
        [TestCase("2x^2 - (3.5xy + y) - 1 = 1 + 2y^2", ExpectedResult = "2x^2 - 2y^2 - 3.5xy - y - 2 = 0")]
        [TestCase("2 - 1 + x = 1 + x + x + y", ExpectedResult = "-x - y = 0")]
        [TestCase("0 + xy - 3xy + (13 - 1) = 2x^2", ExpectedResult = "-2x^2 - 2xy + 12 = 0")]
        [TestCase("x = x", ExpectedResult = "0 = 0")]
        [TestCase("0 = 0", ExpectedResult = "0 = 0")]
        [TestCase("xy - yx + z = 0", ExpectedResult = "z = 0")]
        [TestCase("zyx - yzx - xzy - yxz + 1 = 1", ExpectedResult = "-2xyz = 0")]
        [TestCase("y^3x^2 + z = 0", ExpectedResult = "x^2y^3 + z = 0")]
        [TestCase("z^2x^11y^13 + z + x + 2z = -1", ExpectedResult = "x^11y^13z^2 + x + 3z + 1 = 0")]
        [TestCase("z + x + 2z = -1 - z^2x^11y^13", ExpectedResult = "x^11y^13z^2 + x + 3z + 1 = 0")]
        [TestCase("x + 3y + z^2 = 0", ExpectedResult = "z^2 + x + 3y = 0")]
        [TestCase("x + 3y + z^2 = -x^3 + 2y^2 + z - 17", ExpectedResult = "x^3 - 2y^2 + z^2 + x + 3y - z + 17 = 0")]
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
