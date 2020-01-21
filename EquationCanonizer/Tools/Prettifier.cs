using EquationCanonizer.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EquationCanonizer.Tools
{
    /// <summary>
    /// This class is responsible for converting the token into a equation string.
    /// </summary>
    public class Prettifier
    {
        /// <summary>
        /// Combine an equation string from tokens.
        /// </summary>
        /// <param name="tokenCollection">Token collection.</param>
        /// <returns>Equation string.</returns>
        public string CombineTokensToEquationString(IList<IToken> tokenCollection)
        {
            var equationStringBuilder = new StringBuilder();

            foreach (var token in tokenCollection)
            {
                if (token is LeftParenthesisToken)
                {
                    equationStringBuilder.Append(LeftParenthesisToken.Representation);
                }
                else if (token is RightParenthesisToken)
                {
                    equationStringBuilder.Append(RightParenthesisToken.Representation);
                }
                else if (token is SignToken signToken)
                {
                    equationStringBuilder.Append(" ");
                    equationStringBuilder.Append(signToken.Sign);
                    equationStringBuilder.Append(" ");
                }
                else if (token is TermToken termToken)
                {
                    // // If the coefficient is not 0.
                    if (Math.Abs(termToken.Coefficient - 1) > double.Epsilon)
                    {
                        equationStringBuilder.Append(termToken.Coefficient.ToString(CultureInfo.InvariantCulture));
                    }

                    equationStringBuilder.Append(termToken.Variable);
                }
            }

            return PrettifyResult(equationStringBuilder.ToString());
        }

        private static string PrettifyResult(string equationString)
        {
            // Remove extra spaces at the beginning and remove the plus sign, as it is redundant.
            if (equationString[1] == SignToken.MinusSignRepresentation)
            {
                return equationString.Remove(0, 1).Remove(1, 1);
            }

            return equationString.Remove(0, 3);
        }
    }
}
