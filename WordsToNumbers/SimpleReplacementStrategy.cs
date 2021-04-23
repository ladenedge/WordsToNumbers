using System.Collections.Generic;
using System.Linq;

namespace WordsToNumbers
{
    /// <summary>
    /// A simple strategy for converting words to numbers.
    /// </summary>
    public class SimpleReplacementStrategy : IWordToNumberStrategy
    {
        /// <summary>
        /// Convert numeric words to numbers.
        /// </summary>
        /// <param name="words">A sentence with numeric words.</param>
        /// <returns>The input sentence with the numeric words replaced by numerals.</returns>
        public string ConvertWordsToNumbers(string words)
        {
            if (words is null)
                return words;

            var resultWords = string.Empty;
            var splitWords = words.ToLower().Trim().Split();
            var currentNumberWords = new List<string>();

            foreach (var word in splitWords)
            {
                if (word == "and" && currentNumberWords.Any())
                    continue;

                if (NumericalWords.Any(w => w == word))
                {
                    currentNumberWords.Add(word);
                    continue;
                }

                if (currentNumberWords.Any())
                {
                    var numberWord = ParseNumber(currentNumberWords);
                    resultWords += $"{numberWord} ";
                    currentNumberWords.Clear();
                }
            
                resultWords += $"{word} ";
            }

            if (currentNumberWords.Any())
            {
                var numberWord = ParseNumber(currentNumberWords);
                resultWords += numberWord;
            }

            resultWords = resultWords.TrimEnd();
            return resultWords;
        }

        private static string ParseNumber(List<string> currentNumberWords)
        {
            string numerals = string.Empty;
            var actualNumber = 0;

            foreach (var suffix in SuffixNumbers.OrderByDescending(n => n.Value))
            {
                var suffixIndex = currentNumberWords.IndexOf(suffix.Key);
                if (suffixIndex < 1)
                    continue;
                var suffixWords = currentNumberWords.GetRangeByIndices(0, suffixIndex);
                
                var (number, _) = ComputeNumber(suffixWords);
                actualNumber += number * suffix.Value;
                currentNumberWords = currentNumberWords.GetRangeToEnd(suffixIndex + 1);
            }

            while (currentNumberWords.Any())
            {
                var (number, unusedWords) = ComputeNumber(currentNumberWords);
                if (actualNumber > 0)
                    actualNumber += number;
                else
                    numerals += number.ToString();
                currentNumberWords = unusedWords;
            }

            if (actualNumber > 0)
                numerals = actualNumber.ToString();

            return numerals;
        }

        private static (int, List<string>) ComputeNumber(List<string> numberWords)
        {
            var number = 0;

            for (int i = 0; i < numberWords.Count; i++)
            {
                var word = numberWords[i];
                var hasNextWord = i + 1 < numberWords.Count;

                if (PrefixNumbers.TryGetValue(word, out int tens))
                {
                    number += tens;
                    if (hasNextWord)
                    {
                        var nextWord = numberWords[i + 1];
                        if (DecimalNumbers.TryGetValue(nextWord, out int ones))
                        {
                            number += ones;
                            return (number, numberWords.GetRangeToEnd(i + 2));
                        }
                    }
                    return (number, numberWords.GetRangeToEnd(i + 1));
                }

                if (DecimalNumbers.TryGetValue(word, out number))
                    return (number, numberWords.GetRangeToEnd(i + 1));
                if (SoloNumbers.TryGetValue(word, out number))
                    return (number, numberWords.GetRangeToEnd(i + 1));
            }

            return (number, numberWords);
        }

        static readonly IReadOnlyDictionary<string, int> DecimalNumbers = new Dictionary<string, int>
        {
            ["oh"] = 0,
            ["zero"] = 0,
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9,
        };

        static readonly IReadOnlyDictionary<string, int> SoloNumbers = new Dictionary<string, int>
        {
            ["ten"] = 10,
            ["eleven"] = 11,
            ["twelve"] = 12,
            ["thirteen"] = 13,
            ["fourteen"] = 14,
            ["fifteen"] = 15,
            ["sixteen"] = 16,
            ["seventeen"] = 17,
            ["eighteen"] = 18,
            ["nineteen"] = 19,
        };

        static readonly IReadOnlyDictionary<string, int> PrefixNumbers = new Dictionary<string, int>
        {
            ["twenty"] = 20,
            ["thirty"] = 30,
            ["forty"] = 40,
            ["fifty"] = 50,
            ["sixty"] = 60,
            ["seventy"] = 70,
            ["eighty"] = 80,
            ["ninety"] = 90,
        };

        static readonly IReadOnlyDictionary<string, int> SuffixNumbers = new Dictionary<string, int>
        {
            ["hundred"] = 100,
            ["thousand"] = 1000,
        };

        static readonly IEnumerable<string> NumericalWords = 
            DecimalNumbers.Concat(SoloNumbers).Concat(PrefixNumbers).Concat(SuffixNumbers).Select(kvp => kvp.Key);
    }
}