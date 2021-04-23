using NUnit.Framework;
using System.Collections.Generic;

namespace WordsToNumbers.Tests
{
    [TestFixture]
    public class SimpleReplacementStrategyTests
    {
        IWordToNumberStrategy Strategy { get; } = new SimpleReplacementStrategy();

        [Test]
        public void ConvertWordsToNumbers([ValueSource(nameof(SuccessCases))] KeyValuePair<string, string> testCase)
        {
            var output = Strategy.ConvertWordsToNumbers(testCase.Key);
            Assert.That(output, Is.EqualTo(testCase.Value));
        }

        [Test]
        public void ConvertWordsToNumbers_NoNumberReturnsOriginalString([Values(null, "not a number")] string input)
        {
            var output = Strategy.ConvertWordsToNumbers(input);
            Assert.That(output, Is.EqualTo(input));
        }

        static readonly Dictionary<string, string> SuccessCases = new()
        {
            ["zero"] = "0",
            ["ZeRo"] = "0",
            ["ten"] = "10",
            ["eleven"] = "11",
            ["twelve"] = "12",
            ["thirteen"] = "13",
            ["fourteen"] = "14",
            ["fifteen"] = "15",
            ["sixteen"] = "16",
            ["seventeen"] = "17",
            ["eighteen"] = "18",
            ["nineteen"] = "19",
            ["twenty"] = "20",
            ["twenty one"] = "21",
            ["thirty two"] = "32",
            ["forty three"] = "43",
            ["fifty four"] = "54",
            ["sixty five"] = "65",
            ["seventy six"] = "76",
            ["eighty seven"] = "87",
            ["ninety eight"] = "98",
            ["one hundred and two"] = "102",
            ["one hundred and seventy two"] = "172",
            ["one thousand two hundred"] = "1200",
            ["eighty ten"] = "8010",
            ["thirty four hundred"] = "3400",
            ["forty four eleven"] = "4411",
            ["forty four ninety one"] = "4491",
            ["one two three four five six seven eight nine"] = "123456789",
            ["one fifteen main street"] = "115 main street",
            ["thirty eight west elm"] = "38 west elm",
        };
    }
}
