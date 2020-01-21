using EquationCanonizer.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EquationCanonizer.Tools
{
    /// <summary>
    /// This class contains the logic of splitting the equation into tokens.
    /// </summary>
    public class Tokenizer
    {
        /// <summary>
        /// Split the equations into tokens.
        /// </summary>
        /// <param name="equation">The string representation of the equation.</param>
        /// <returns>Token collection.</returns>
        public IList<IToken> SplitEquationIntoTokens(string equation)
        {
            var coefficientBuffer = new StringBuilder();
            var variableBuffer = new StringBuilder();
            var tokenCollection = new List<IToken>();

            foreach (var character in equation.Where(character => character != ' '))
            {
                if (IsTermTokenCharacter(character, variableBuffer))
                {
                    variableBuffer.Append(character);
                }
                else if (character == TermToken.DoubleDelimiter || char.IsDigit(character))
                {
                    coefficientBuffer.Append(character);
                }
                else if (IsSignTokenCharacter(character))
                {
                    if (coefficientBuffer.Length > 0 || variableBuffer.Length > 0)
                    {
                        tokenCollection.Add(new TermToken(GetCoefficientFromBuffer(coefficientBuffer), GetVariableFromBuffer(variableBuffer)));
                    }

                    tokenCollection.Add(new SignToken(character));
                }
                else if (character == LeftParenthesisToken.Representation)
                {
                    tokenCollection.Add(new LeftParenthesisToken());
                }
                else if (character == RightParenthesisToken.Representation)
                {
                    if (coefficientBuffer.Length > 0 || variableBuffer.Length > 0)
                    {
                        tokenCollection.Add(new TermToken(GetCoefficientFromBuffer(coefficientBuffer), GetVariableFromBuffer(variableBuffer)));
                    }

                    tokenCollection.Add(new RightParenthesisToken());
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected character {character} was found!");
                }
            }

            // Collect the remaining data from the buffers after passing through the loop and combine the last term.
            if (coefficientBuffer.Length > 0 || variableBuffer.Length > 0)
            {
                tokenCollection.Add(new TermToken(GetCoefficientFromBuffer(coefficientBuffer), GetVariableFromBuffer(variableBuffer)));
            }

            return tokenCollection;
        }

        private static bool IsTermTokenCharacter(char character, StringBuilder variableBuffer)
        {
            if (char.IsLetter(character))
            {
                return true;
            }

            var isPowCharacter = character == '^';
            var bufferIsNotEmpty = variableBuffer.Length > 0;
            var isDigitOrPowCharacter = char.IsDigit(character) || isPowCharacter;

            // If the current character is a digit or a pow character, then the buffer must contain some value to combine a term.
            return bufferIsNotEmpty && isDigitOrPowCharacter;
        }

        private static bool IsSignTokenCharacter(char c)
        {
            return SignToken.SignRepresentationCollection.Contains(c);
        }

        private static double GetCoefficientFromBuffer(StringBuilder coefficientBuffer)
        {
            if (coefficientBuffer.Length == 0)
            {
                return 1;
            }

            var coefficientString = coefficientBuffer.ToString();
            var coefficient = double.Parse(coefficientString, CultureInfo.InvariantCulture);
            coefficientBuffer.Clear();

            return coefficient;
        }

        private static string GetVariableFromBuffer(StringBuilder variableBuffer)
        {
            if (variableBuffer.Length == 0)
            {
                return string.Empty;
            }

            var variable = variableBuffer.ToString();
            variableBuffer.Clear();

            return variable;
        }
    }
}
