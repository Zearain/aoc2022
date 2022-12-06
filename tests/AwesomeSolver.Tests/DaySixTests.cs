using AwesomeSolver.Core.Services;
using AwesomeSolver.Core.Solvers;
using FluentAssertions;
using Moq;

namespace AwesomeSolver.Core.Tests;

public class DaySixTests
{
    private IInputProvider inputProvider = null!;

    [SetUp]
    public void Setup()
    {
        var mockInputProvider = new Mock<IInputProvider>();
        // mockInputProvider.Setup(x => x.GetInputStringAsync(It.IsAny<int>())).ReturnsAsync(DaySixTestData.Input);
        inputProvider = mockInputProvider.Object;
    }

    [TestCaseSource(typeof(DaySixTestData), nameof(DaySixTestData.StartPacketIndexTestCases))]
    public int ParseStartPacketIndexShouldReturnExpectedResult(string input)
    {
        return DaySixSolver.ParseStartPacketIndex(input);
    }

    [TestCaseSource(typeof(DaySixTestData), nameof(DaySixTestData.DynamicPacketIndexTestCases))]
    public int ParsePacketIndexShouldReturnExpectedResult(string input, int uniqueChars)
    {
        return DaySixSolver.ParsePacketIndex(input, uniqueChars);
    }
}

public static class DaySixTestData
{
    // bvwbjplbgvbhsrlpgdmjqwftvncz: first marker after character 5
    // nppdvjthqldpwncqszvftbrmjlhg: first marker after character 6
    // nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg: first marker after character 10
    // zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw: first marker after character 11
    public static IEnumerable<TestCaseData> StartPacketIndexTestCases
    {
        get
        {
            yield return new TestCaseData("mjqjpqmgbljsphdztnvjfqwrcgsmlb").Returns(7);
            yield return new TestCaseData("bvwbjplbgvbhsrlpgdmjqwftvncz").Returns(5);
            yield return new TestCaseData("nppdvjthqldpwncqszvftbrmjlhg").Returns(6);
            yield return new TestCaseData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg").Returns(10);
            yield return new TestCaseData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw").Returns(11);
        }
    }

    // mjqjpqmgbljsphdztnvjfqwrcgsmlb: first marker after character 19
    // bvwbjplbgvbhsrlpgdmjqwftvncz: first marker after character 23
    // nppdvjthqldpwncqszvftbrmjlhg: first marker after character 23
    // nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg: first marker after character 29
    // zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw: first marker after character 26
    public static IEnumerable<TestCaseData> DynamicPacketIndexTestCases
    {
        get
        {
            yield return new TestCaseData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 14).Returns(19);
            yield return new TestCaseData("bvwbjplbgvbhsrlpgdmjqwftvncz", 14).Returns(23);
            yield return new TestCaseData("nppdvjthqldpwncqszvftbrmjlhg", 14).Returns(23);
            yield return new TestCaseData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 14).Returns(29);
            yield return new TestCaseData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 14).Returns(26);
        }
    }
}