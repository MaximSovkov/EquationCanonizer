namespace EquationCanonizer.Tokens
{
    /// <summary>
    /// Term token. It can store both a coefficient and a variable.
    /// </summary>
    public class TermToken : IToken
    {
        /// <summary>
        /// Number delimiter. Used for tokenization.
        /// </summary>
        public const char DoubleDelimiter = '.';

        /// <summary>
        /// Coefficient.
        /// </summary>
        public double Coefficient { get; }

        /// <summary>
        /// Variable (like x or y).
        /// </summary>
        public string Variable { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="coefficient">Coefficient.</param>
        /// <param name="variable">Variable.</param>
        public TermToken(double coefficient, string variable)
        {
            Coefficient = coefficient;
            Variable = variable;
        }
    }
}
