using AwesomeSolver.Core.Services;
using AwesomeSolver.Core.Solvers;
using FluentAssertions;
using Moq;

namespace AwesomeSolver.Core.Tests;

public class DayEightTests
{
    private IInputProvider inputProvider = null!;

    [SetUp]
    public void Setup()
    {
        var mockInputProvider = new Mock<IInputProvider>();
        mockInputProvider.Setup(x => x.GetInputStringAsync(It.IsAny<int>())).ReturnsAsync(DayEightTestData.Input);
        inputProvider = mockInputProvider.Object;
    }

    [TestCaseSource(typeof(DayEightTestData), nameof(DayEightTestData.ParseLineAsArrayTestCases))]
    public int[] ParseLineAsArrayShouldReturnExpectedResult(string line)
    {
        return DayEightSolver.ParseLineAsArray(line);
    }

    [Test]
    public void ParseInputLinesAsArraysShouldReturnExpectedResult()
    {
        var expected = new[] {
            new[] {3,0,3,7,3},
            new[] {2,5,5,1,2},
            new[] {6,5,3,3,2},
            new[] {3,3,5,4,9},
            new[] {3,5,3,9,0},
        };

        var result = DayEightSolver.ParseInputLinesAsArrays(DayEightTestData.Input.Split(Environment.NewLine));

        result.Should().BeEquivalentTo(expected);
    }

    [TestCaseSource(typeof(DayEightTestData), nameof(DayEightTestData.IsTreeVisibleTestCases))]
    public bool IsTreeVisibleShouldReturnExpectedResult(int posX, int posY)
    {
        var trees = DayEightTestData.InputAsArrays;

        return DayEightSolver.IsTreeVisible(trees, posX, posY);
    }

    [Test]
    public async Task SolvePartOneShouldReturnExpectedResult()
    {
        var solver = new DayEightSolver(inputProvider);

        var result = await solver.SolvePartOne();

        result.Should().Be("21");
    }
}

internal static class DayEightTestData
{
    public static string Input => @"30373
25512
65332
33549
35390";

    public static int[][] InputAsArrays => new[] {
            new[] {3,0,3,7,3},
            new[] {2,5,5,1,2},
            new[] {6,5,3,3,2},
            new[] {3,3,5,4,9},
            new[] {3,5,3,9,0},
        };

    public static IEnumerable<TestCaseData> ParseLineAsArrayTestCases
    {
        get
        {
            yield return new TestCaseData("30373").Returns(new[] {3,0,3,7,3});
            yield return new TestCaseData("25512").Returns(new[] {2,5,5,1,2});
            yield return new TestCaseData("65332").Returns(new[] {6,5,3,3,2});
            yield return new TestCaseData("33549").Returns(new[] {3,3,5,4,9});
            yield return new TestCaseData("35390").Returns(new[] {3,5,3,9,0});
        }
    }

    public static IEnumerable<TestCaseData> IsTreeVisibleTestCases
    {
        get
        {
            yield return new TestCaseData(1, 1).Returns(true);
            yield return new TestCaseData(2, 1).Returns(true);
            yield return new TestCaseData(3, 1).Returns(false);
            yield return new TestCaseData(1, 2).Returns(true);
            yield return new TestCaseData(2, 2).Returns(false);
            yield return new TestCaseData(3, 2).Returns(true);
            yield return new TestCaseData(1, 3).Returns(false);
            yield return new TestCaseData(2, 3).Returns(true);
            yield return new TestCaseData(3, 3).Returns(false);
        }
    }
}