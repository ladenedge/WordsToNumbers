namespace WordsToNumbers
{
    /// <summary>
    /// Represents a strategy for converting words to numbers.
    /// </summary>
    public interface IWordToNumberStrategy
    {
        /// <summary>
        /// Convert numeric words to numbers.
        /// </summary>
        /// <param name="words">A sentence with numeric words.</param>
        /// <returns>The input sentence with the numeric words replaced by numerals.</returns>
        string ConvertWordsToNumbers(string words);
    }
}