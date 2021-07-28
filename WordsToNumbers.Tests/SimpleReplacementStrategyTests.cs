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
            ["two ninety eight"] = "298",
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
            ["one hundredth avenue"] = "100 avenue",
            ["two hundred fifteenth place"] = "215 place",
            ["twelve ninety nine east thirty fourth street"] = "1299 east 34 street",
            ["five thirty three west one hundred and nineteenth street"] = "533 west 119 street",
            ["fifth street"] = "5 street",
            ["four seven four seven west one thirty fourth street"] = "4747 west 134 street",
            ["five one six six thirty fourth street"] = "5166 34 street",
            ["eight twenty west forty eighth street"] = "820 west 48 street",
            ["six six zero west one fifty seventh street"] = "660 west 157 street",
            ["one two zero fourth street"] = "120 4 street",
            ["seventeen fourteen seventh avenue"] = "1714 7 avenue",
            ["thirteen nineteen"] = "1319",
            ["three four eight eleventh street"] = "348 11 street",
            ["five twenty five"] = "525",
            ["seven two eight hundred and eighteenth place"] = "728 118 place",
            ["nineteen sixty three evergreen"] = "1963 evergreen",
            ["one five two three west one hundred and tenth place"] = "1523 west 110 place",
            ["1317 for you did hundred newt avenue"] = "1317 for you did 100 newt avenue",
            ["who hundred west company"] = "who 100 west company",
            ["fifty twenty eight to hundredth avenue"] = "5028 200 avenue",
            ["ninety two a hundred mill avenue nine one seven"] = "9200 mill avenue 917",
        };
    }
}
