using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WordsToNumbers
{
    /// <summary>
    /// A simple strategy for converting words to numbers.
    /// </summary>
    public class SimpleReplacementStrategy : IWordToNumberStrategy
    {
        const int MaximumOrdinalNumber = 299;

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
            var normalWords = words.ToLower().Trim();

            foreach (var replacer in PreReplacements)
                normalWords = normalWords.Replace(replacer.Key, replacer.Value);

            var splitWords = normalWords.Split();
            var currentNumberWords = new List<string>();

            foreach (var word in splitWords)
            {
                if (word == "and" && currentNumberWords.Any())
                    continue;

                var isOrdinal = OrdinalNumbers.ContainsKey(word);

                if (NumericalWords.Any(w => w == word))
                {
                    currentNumberWords.Add(word);
                    if (!isOrdinal)  // stop if this is an ordinal (ie. final) word
                        continue;
                }

                if (currentNumberWords.Any())
                {
                    var numberWord = ParseNumber(currentNumberWords);
                    resultWords += $"{numberWord} ";
                    currentNumberWords.Clear();
                }
            
                if (!isOrdinal)
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

        private static string ParseNumber(List<string> currentNumberWords, bool allowOrdinals = true)
        {
            string numerals = string.Empty;
            var ordinalNumber = 0;
            var actualNumber = 0;

            var ordinal = currentNumberWords.SingleOrDefault(w => OrdinalNumbers.ContainsKey(w));
            if (ordinal is not null && allowOrdinals)
            {
                var ordinalIndex = currentNumberWords.IndexOf(ordinal);
                List<string> unusedWords = new();
                
                for (int startIndex = 0; startIndex <= ordinalIndex; startIndex++)
                {
                    var currentOrdinalWords = currentNumberWords.GetRangeByIndices(startIndex, ordinalIndex + 1);

                    if (!currentOrdinalWords.Any(w => NonOrdinalNumbers.ContainsKey(w)))
                    {
                        var ordinalCandidate = ParseNumber(currentOrdinalWords, false);
                        ordinalNumber = int.Parse(ordinalCandidate);
                    }

                    if (startIndex > 0)
                        unusedWords = currentNumberWords.GetRangeByIndices(0, startIndex);
                    if (ordinalNumber > 0 && ordinalNumber <= MaximumOrdinalNumber)
                        break;
                }

                currentNumberWords = unusedWords;
            }

            foreach (var suffix in MultiplierNumbers.OrderByDescending(n => n.Value))
            {
                var suffixIndex = currentNumberWords.IndexOf(suffix.Key);
                if (suffixIndex < 0)
                    continue;

                // if the suffix word is first, we assume an implicit "one"
                int number = 1;

                if (suffixIndex >= 1)
                {
                    var suffixWords = currentNumberWords.GetRangeByIndices(0, suffixIndex);
                    (number, _) = ComputeNumber(suffixWords);
                }

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

                if (currentNumberWords.Count <= unusedWords.Count)
                    break;  // we're not consuming words as expected

                currentNumberWords = unusedWords;
            }

            if (actualNumber > 0)
                numerals = actualNumber.ToString();
            if (ordinalNumber > 0)
                numerals += " " + ordinalNumber.ToString();

            return numerals.Trim();
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
                        if (DecimalNumbers.TryGetValue(nextWord, out int ones) || OrdinalNumbers.TryGetValue(nextWord, out ones))
                        {
                            number += ones;
                            return (number, numberWords.GetRangeToEnd(i + 2));
                        }
                    }
                    return (number, numberWords.GetRangeToEnd(i + 1));
                }

                if (ComputeNumbers.TryGetValue(word, out number))
                    return (number, numberWords.GetRangeToEnd(i + 1));
            }

            return (number, numberWords);
        }

        static readonly IReadOnlyDictionary<string, string> PreReplacements = new Dictionary<string, string>
        {
            ["a hundred"] = "hundred",
            ["to hundred"] = "two hundred",
            ["too hundred"] = "two hundred",
            ["a thousand"] = "thousand",
            ["to thousand"] = "two thousand",
            ["too thousand"] = "two thousand",
        };

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

        static readonly IReadOnlyDictionary<string, int> MultiplierNumbers = new Dictionary<string, int>
        {
            ["hundred"] = 100,
            ["hundredth"] = 100,
            ["thousand"] = 1000,
        };

        static readonly IReadOnlyDictionary<string, int> OrdinalNumbers = new Dictionary<string, int>
        {
            ["first"] = 1,
            ["second"] = 2,
            ["third"] = 3,
            ["fourth"] = 4,
            ["fifth"] = 5,
            ["sixth"] = 6,
            ["seventh"] = 7,
            ["eighth"] = 8,
            ["ninth"] = 9,
            ["tenth"] = 10,
            ["eleventh"] = 11,
            ["twelfth"] = 12,
            ["thirteenth"] = 13,
            ["fourteenth"] = 14,
            ["fifteenth"] = 15,
            ["sixteenth"] = 16,
            ["seventeenth"] = 17,
            ["eighteenth"] = 18,
            ["nineteenth"] = 19,
            ["twentieth"] = 20,
            ["thirtieth"] = 30,
            ["fortieth"] = 40,
            ["fiftieth"] = 50,
            ["sixtieth"] = 60,
            ["seventieth"] = 70,
            ["eightieth"] = 80,
            ["ninetieth"] = 90,
            ["hundredth"] = 100,
        };

        static readonly IReadOnlyDictionary<string, int> NonOrdinalNumbers = SoloNumbers
           .Concat(DecimalNumbers).Where(d => d.Key != "one" && d.Key != "two")
           .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        static readonly IReadOnlyDictionary<string, int> ComputeNumbers = DecimalNumbers
            .Concat(SoloNumbers)
            .Concat(OrdinalNumbers)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        static readonly IEnumerable<string> NumericalWords = DecimalNumbers
            .Concat(SoloNumbers)
            .Concat(PrefixNumbers)
            .Concat(MultiplierNumbers)
            .Concat(OrdinalNumbers)
            .Select(kvp => kvp.Key);
    }
}