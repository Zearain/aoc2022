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
    public RopePosition MoveInDirectionShouldReturnExpectedResult(RopePosition start, Direction direction)
    {
        return DayNineSolver.MoveInDirection(start, direction);
    }

    [TestCaseSource(typeof(DayNineTestData), nameof(DayNineTestData.IsRopePositionsTouchingTestCases))]
    public bool IsRopePositionsTouchingShouldReturnExpectedResult(RopePosition position, RopePosition other)
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
            yield return new TestCaseData(new RopePosition(10, 10), Direction.Right).Returns(new RopePosition(11, 10));
            yield return new TestCaseData(new RopePosition(10, 10), Direction.Up).Returns(new RopePosition(10, 11));
            yield return new TestCaseData(new RopePosition(10, 10), Direction.Left).Returns(new RopePosition(9, 10));
            yield return new TestCaseData(new RopePosition(10, 10), Direction.Down).Returns(new RopePosition(10, 9));
        }
    }

    public static IEnumerable<TestCaseData> IsRopePositionsTouchingTestCases
    {
        get
        {
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(10, 10)).Returns(true);
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(11, 10)).Returns(true);
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(10, 11)).Returns(true);
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(9, 10)).Returns(true);
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(10, 9)).Returns(true);
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(11, 11)).Returns(true);
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(9, 9)).Returns(true);
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(9, 11)).Returns(true);
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(11, 9)).Returns(true);
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(12, 10)).Returns(false);
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(10, 12)).Returns(false);
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(8, 10)).Returns(false);
            yield return new TestCaseData(new RopePosition(10, 10), new RopePosition(10, 8)).Returns(false);
        }
    }
}