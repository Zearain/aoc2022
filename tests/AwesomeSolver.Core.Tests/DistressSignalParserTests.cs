using AwesomeSolver.Core.Solvers.Day13;
using FluentAssertions;

namespace AwesomeSolver.Core.Tests;

public class DistressSignalParserTests
{
    [Test]
    public void ParseInputPairsShouldReturnExpectedResult()
    {
        var inputPairs = DistressSignalParser.ParseInputPairs(DistressSignalParserTestData.Input);
        inputPairs.Should().BeEquivalentTo(DistressSignalParserTestData.InputPairs);
    }

    [TestCaseSource(typeof(DistressSignalParserTestData), nameof(DistressSignalParserTestData.IsPairInCorrectOrderCases))]
    public bool IsPairInCorrectOrderShouldReturnExpectedResult((string Left, string Right) inputPair)
    {
        return DistressSignalParser.IsPairInCorrectOrder(inputPair);
    }
}

public static class DistressSignalParserTestData
{
    public static string Input => @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]";

    public static IEnumerable<(string Left, string Right)> InputPairs
    {
        get
        {
            yield return (Left: "[1,1,3,1,1]", Right: "[1,1,5,1,1]");
            yield return (Left: "[[1],[2,3,4]]", Right: "[[1],4]");
            yield return (Left: "[9]", Right: "[[8,7,6]]");
            yield return (Left: "[[4,4],4,4]", Right: "[[4,4],4,4,4]");
            yield return (Left: "[7,7,7,7]", Right: "[7,7,7]");
            yield return (Left: "[]", Right: "[3]");
            yield return (Left: "[[[]]]", Right: "[[]]");
            yield return (Left: "[1,[2,[3,[4,[5,6,7]]]],8,9]", Right: "[1,[2,[3,[4,[5,6,0]]]],8,9]");
        }
    }

    public static IDictionary<int, bool> PairIndexIsCorrectOrder = new Dictionary<int, bool>
    {
        {0, true},
        {1, true},
        {2, false},
        {3, true},
        {4, false},
        {5, true},
        {6, false},
        {7, false},
    };

    public static IEnumerable<TestCaseData> IsPairInCorrectOrderCases
    {
        get
        {
            var inputPairs = InputPairs.ToArray();
            for (int i = 0; i < inputPairs.Length; i++)
            {
                yield return new TestCaseData(inputPairs[i]).Returns(PairIndexIsCorrectOrder[i]);
            }
        }
    }
}