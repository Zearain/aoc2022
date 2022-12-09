using AwesomeSolver.Core.Services;
using AwesomeSolver.Core.Solvers;
using FluentAssertions;
using Moq;

namespace AwesomeSolver.Tests;

public class DayNineTests
{
    private IInputProvider inputProvider = null!;

    [SetUp]
    public void Setup()
    {
        var mockInputProvider = new Mock<IInputProvider>();
        mockInputProvider.Setup(x => x.GetInputStringAsync(It.IsAny<int>())).ReturnsAsync(DayNineTestData.Input);
        inputProvider = mockInputProvider.Object;
    }

    [TestCaseSource(typeof(DayNineTestData), nameof(DayNineTestData.ParseMoveInputTestCases))]
    public RopeMovement ParseMoveInputShouldReturnExpectedResult(string inputLine)
    {
        return DayNineSolver.ParseMoveInput(inputLine);
    }

    [TestCaseSource(typeof(DayNineTestData), nameof(DayNineTestData.MoveInDirectionTestCases))]
    public KnotPosition MoveInDirectionShouldReturnExpectedResult(KnotPosition start, Direction direction)
    {
        return DayNineSolver.MoveInDirection(start, direction);
    }

    [TestCaseSource(typeof(DayNineTestData), nameof(DayNineTestData.IsRopePositionsTouchingTestCases))]
    public bool IsRopePositionsTouchingShouldReturnExpectedResult(KnotPosition position, KnotPosition other)
    {
        return DayNineSolver.IsRopePositionsTouching(position, other);
    }

    [Test]
    public async Task SolvePartOneAsyncShouldReturnExpectedResult()
    {
        var solver = new DayNineSolver(inputProvider);
        await solver.InitializeAsync();

        var result = await solver.SolvePartOneAsync();
        result.Should().Be("13");
    }

    [Test]
    public async Task SolvePartTwoAsyncShouldReturnExpectedResult()
    {
        var mockInputProvider = new Mock<IInputProvider>();
        mockInputProvider.Setup(x => x.GetInputStringAsync(It.IsAny<int>())).ReturnsAsync(@"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20");

        var solver = new DayNineSolver(mockInputProvider.Object);
        await solver.InitializeAsync();

        var result = solver.SolvePartTwo();
        result.Should().Be("36");
    }
}

internal static class DayNineTestData
{
    public static string Input = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";

    public static IEnumerable<TestCaseData> ParseMoveInputTestCases
    {
        get
        {
            yield return new TestCaseData("R 4").Returns(new RopeMovement(Direction.Right, 4));
            yield return new TestCaseData("U 4").Returns(new RopeMovement(Direction.Up, 4));
            yield return new TestCaseData("L 3").Returns(new RopeMovement(Direction.Left, 3));
            yield return new TestCaseData("D 1").Returns(new RopeMovement(Direction.Down, 1));
        }
    }

    public static IEnumerable<TestCaseData> MoveInDirectionTestCases
    {
        get
        {
            yield return new TestCaseData(new KnotPosition(10, 10), Direction.Right).Returns(new KnotPosition(11, 10));
            yield return new TestCaseData(new KnotPosition(10, 10), Direction.Up).Returns(new KnotPosition(10, 11));
            yield return new TestCaseData(new KnotPosition(10, 10), Direction.Left).Returns(new KnotPosition(9, 10));
            yield return new TestCaseData(new KnotPosition(10, 10), Direction.Down).Returns(new KnotPosition(10, 9));
        }
    }

    public static IEnumerable<TestCaseData> IsRopePositionsTouchingTestCases
    {
        get
        {
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(10, 10)).Returns(true);
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(11, 10)).Returns(true);
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(10, 11)).Returns(true);
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(9, 10)).Returns(true);
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(10, 9)).Returns(true);
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(11, 11)).Returns(true);
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(9, 9)).Returns(true);
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(9, 11)).Returns(true);
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(11, 9)).Returns(true);
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(12, 10)).Returns(false);
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(10, 12)).Returns(false);
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(8, 10)).Returns(false);
            yield return new TestCaseData(new KnotPosition(10, 10), new KnotPosition(10, 8)).Returns(false);
        }
    }
}