using EquationCanonizer.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EquationCanonizer.Tools
{
    /// <summary>
    /// This class is responsible for simplifying the tokens of the equation.
    /// </summary>
    public class Simplifier
    {
        /// <summary>
        /// Simplify the tokens of the equation. (open the parentheses and assign value to zero).
        /// </summary>
        /// <param name="tokenCollecton">Token collection.</param>
        /// <returns>Simplified token collection.</returns>
        public IList<IToken> SimplifyEquationTokens(IList<IToken> tokenCollecton)
        {
            if (tokenCollecton.OfType<SignToken>().Count(token => token.Sign == SignToken.EqualSignRepresentation) != 1)
            {
                throw new ArgumentException("Count of equal signs must be equal to 1!");
            }

            var equalityToken = tokenCollecton.OfType<SignToken>()
                .Single(token => token.Sign == SignToken.EqualSignRepresentation);

            var leftSideEquation = tokenCollecton
                .TakeWhile(token => token != equalityToken);

            var rightSideEquation = tokenCollecton
                .SkipWhile(token => token != equalityToken).Skip(1);

            var oneSideTokenCollection = new List<IToken>();

            // Move all terms to the left side of the equation, enclose them in parentheses and add negation.
            oneSideTokenCollection.AddRange(leftSideEquation);
            oneSideTokenCollection.Add(new SignToken(SignToken.MinusSignRepresentation));
            oneSideTokenCollection.Add(new LeftParenthesisToken());
            oneSideTokenCollection.AddRange(rightSideEquation);
            oneSideTokenCollection.Add(new RightParenthesisToken());

            return SumCoefficients(ExpandParentheses(oneSideTokenCollection));
        }

        private IList<IToken> ExpandParentheses(IList<IToken> tokenCollection)
        {
            EnsureCorrectParenthesisCount(tokenCollection);

            var parenthesislessTokenCollection = new List<IToken>();
            var mustInvertArithmeticSign = false;
            var previousSign = SignToken.PlusSignRepresentation;

            foreach (var token in tokenCollection)
            {
                if (token is SignToken signToken)
                {
                    previousSign = signToken.Sign;
                    if (mustInvertArithmeticSign)
                    {
                        var invertedArithmeticSign = InvertArithmeticSign(signToken.Sign);
                        parenthesislessTokenCollection.Add(new SignToken(invertedArithmeticSign));
                    }
                    else
                    {
                        parenthesislessTokenCollection.Add(signToken);
                    }
                }
                else if (token is LeftParenthesisToken && previousSign == SignToken.MinusSignRepresentation)
                {
                    mustInvertArithmeticSign = !mustInvertArithmeticSign;
                    parenthesislessTokenCollection.Add(new SignToken(SignToken.MinusSignRepresentation));
                }
                else if (token is RightParenthesisToken)
                {
                    mustInvertArithmeticSign = false;
                }
                else
                {
                    parenthesislessTokenCollection.Add(token);
                }
            }

            return parenthesislessTokenCollection;
        }

        private static char InvertArithmeticSign(char arithmeticSign)
        {
            return arithmeticSign == SignToken.PlusSignRepresentation
                ? SignToken.MinusSignRepresentation
                : SignToken.PlusSignRepresentation;
        }

        private static void EnsureCorrectParenthesisCount(IList<IToken> tokenCollection)
        {
            var leftParenthesisCount = 0;
            var rightParenthesisCount = 0;

            foreach (var token in tokenCollection)
            {
                if (token is LeftParenthesisToken)
                {
                    leftParenthesisCount++;
                }
                else if (token is RightParenthesisToken)
                {
                    if (rightParenthesisCount == leftParenthesisCount)
                    {
                        throw new InvalidOperationException("An extra right parenthesis was detected!");
                    }

                    rightParenthesisCount++;
                }
            }

            if (leftParenthesisCount != rightParenthesisCount)
            {
                throw new InvalidOperationException("Count of left and right parentheses do not match!");
            }
        }

        private static IList<IToken> SumCoefficients(IList<IToken> tokenCollection)
        {
            // Term storage used for easy calculation of coefficients.
            var termStorage = new Dictionary<string, double>();

            var mustNegateCoefficient = false;
            var simplifiedTokenCollection = new List<IToken>();

            foreach (var token in tokenCollection)
            {
                if (token is TermToken termToken)
                {
                    var coefficient = mustNegateCoefficient ? -termToken.Coefficient : termToken.Coefficient;
                    var termTokenVariable = termToken.Variable;

                    // Save new terms in the storage or get already saved and calculate coefficient.
                    if (termStorage.TryGetValue(termTokenVariable, out var currentCoefficient))
                    {
                        termStorage[termTokenVariable] = currentCoefficient + coefficient;
                    }
                    else
                    {
                        termStorage[termTokenVariable] = coefficient;
                    }
                }
                else if (token is SignToken signToken)
                {
                    mustNegateCoefficient = signToken.Sign == SignToken.MinusSignRepresentation;
                }
            }

            foreach (var term in termStorage)
            {
                // If the coefficient is 0 (variable only).
                if (Math.Abs(term.Value) < double.Epsilon)
                {
                    continue;
                }

                var tokenSignArithmeticOperator = term.Value > 0
                    ? SignToken.PlusSignRepresentation
                    : SignToken.MinusSignRepresentation;

                simplifiedTokenCollection.Add(new SignToken(tokenSignArithmeticOperator));
                simplifiedTokenCollection.Add(new TermToken(Math.Abs(term.Value), term.Key));
            }

            // Add a zero assignment to the final result.
            simplifiedTokenCollection.Add(new SignToken(SignToken.EqualSignRepresentation));
            simplifiedTokenCollection.Add(new TermToken(0, string.Empty));

            return simplifiedTokenCollection;
        }
    }
}
