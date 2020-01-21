using System.Collections.Generic;

namespace EquationCanonizer.Tokens
{
    /// <summary>
    /// Sign token.
    /// </summary>
    public class SignToken : IToken
    {
        /// <summary>
        /// Plus sign representation.
        /// </summary>
        public const char PlusSignRepresentation = '+';

        /// <summary>
        /// Minus sign representation.
        /// </summary>
        public const char MinusSignRepresentation = '-';

        /// <summary>
        /// Equal sign representation.
        /// </summary>
        public const char EqualSignRepresentation = '=';

        /// <summary>
        /// Sign representation collection.
        /// </summary>
        public static readonly IReadOnlyList<char> SignRepresentationCollection = new List<char>
        {
            PlusSignRepresentation,
            MinusSignRepresentation,
            EqualSignRepresentation
        };

        /// <summary>
        /// Sign.
        /// </summary>
        public char Sign { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sign">Sign.</param>
        public SignToken(char sign)
        {
            Sign = sign;
        }
    }
}
