using AwesomeSolver.Core.Services;
using AwesomeSolver.Core.Solvers;
using FluentAssertions;
using Moq;

namespace AwesomeSolver.Core.Tests;

public class DayFourTests
{
    private IInputProvider inputProvider = null!;

    private readonly string partOneSolution = "2";
    private readonly string partTwoSolution = "4";

    [SetUp]
    public void Setup()
    {
        var mockInputProvider = new Mock<IInputProvider>();
        mockInputProvider.Setup(x => x.GetInputStringAsync(It.IsAny<int>())).ReturnsAsync(DayFourTestData.Input);
        inputProvider = mockInputProvider.Object;
    }

    [Test]
    public async Task Part1()
    {
        var solver = new DayFourSolver(inputProvider);

        var part1Result = await solver.SolvePartOne();

        part1Result.Should().Be(partOneSolution);
    }

    [Test]
    public async Task Part2()
    {
        var solver = new DayFourSolver(inputProvider);

        var part2Result = await solver.SolvePartTwo();

        part2Result.Should().Be(partTwoSolution);
    }

    [TestCaseSource(typeof(DayFourTestData), nameof(DayFourTestData.ParseElfAssignementCases))]
    public (System.Range FirstElfAssignment, System.Range SecondElfAssignemnt) ParseElfPairAssignementsShouldReturnExpectedResult(string elfPairLine)
    {
        var solver = new DayFourSolver(inputProvider);
        return solver.ParseElfPairAssignment(elfPairLine);
    }

    [TestCaseSource(typeof(DayFourTestData), nameof(DayFourTestData.IsRangeFullOverlapCases))]
    public bool IsFullOverlapShouldReturnExpectedResult(System.Range firstRange, System.Range secondRange)
    {
        var solver = new DayFourSolver(inputProvider);
        return solver.IsFullOverlap(firstRange, secondRange);
    }

    [TestCaseSource(typeof(DayFourTestData), nameof(DayFourTestData.IsRangeAnyOverlapCases))]
    public bool IsAnyOverlapShouldReturnExpectedResult(System.Range firstRange, System.Range secondRange)
    {
        var solver = new DayFourSolver(inputProvider);
        return solver.IsAnyOverlap(firstRange, secondRange);
    }
}

internal class DayFourTestData
{
    public static string Input => @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8";

    public static IEnumerable<TestCaseData> ParseElfAssignementCases
    {
        get
        {
            yield return new TestCaseData("2-4,6-8").Returns((FirstElfAssignment: 2..4, SecondElfAssignment: 6..8));
            yield return new TestCaseData("2-3,4-5").Returns((FirstElfAssignment: 2..3, SecondElfAssignment: 4..5));
            yield return new TestCaseData("5-7,7-9").Returns((FirstElfAssignment: 5..7, SecondElfAssignment: 7..9));
        }
    }

    public static IEnumerable<TestCaseData> IsRangeFullOverlapCases
    {
        get
        {
            yield return new TestCaseData(2..4, 6..8).Returns(false);
            yield return new TestCaseData(2..3, 4..5).Returns(false);
            yield return new TestCaseData(5..7, 7..9).Returns(false);
            yield return new TestCaseData(2..8, 3..7).Returns(true);
            yield return new TestCaseData(6..6, 4..6).Returns(true);
            yield return new TestCaseData(2..6, 4..8).Returns(false);
        }
    }

    public static IEnumerable<TestCaseData> IsRangeAnyOverlapCases
    {
        get
        {
            yield return new TestCaseData(2..4, 6..8).Returns(false);
            yield return new TestCaseData(2..3, 4..5).Returns(false);
            yield return new TestCaseData(5..7, 7..9).Returns(true);
            yield return new TestCaseData(2..8, 3..7).Returns(true);
            yield return new TestCaseData(6..6, 4..6).Returns(true);
            yield return new TestCaseData(2..6, 4..8).Returns(true);
        }
    }
}