using AwesomeSolver.Core.Solvers;

namespace AwesomeSolver.Core.Tests;

public class DayNineTests
{
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
}

internal static class DayNineTestData
{
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