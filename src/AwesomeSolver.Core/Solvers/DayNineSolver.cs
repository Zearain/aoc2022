using System.Diagnostics;
using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

[DaySolver(9)]
public sealed class DayNineSolver : SharedDaySolver
{
    private static readonly IDictionary<Direction, KnotPosition> MoveAddition = new Dictionary<Direction, KnotPosition>
    {
        { Direction.Right, new(1, 0) },
        { Direction.Up, new(0, 1) },
        { Direction.Left, new(-1, 0) },
        { Direction.Down, new(0, -1) },
    };

    private KnotPosition[] knotPositions = null!;
    private readonly HashSet<KnotPosition> visitedLastKnotPosistions = new HashSet<KnotPosition>();

    public DayNineSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    public static RopeMovement ParseMoveInput(string inputLine)
    {
        var split = inputLine.Split(' ');
        return split[0] switch
        {
            "R" => new(Direction.Right, int.Parse(split[1])),
            "U" => new(Direction.Up, int.Parse(split[1])),
            "L" => new(Direction.Left, int.Parse(split[1])),
            "D" => new(Direction.Down, int.Parse(split[1])),
            _ => throw new ArgumentOutOfRangeException(nameof(inputLine))
        };
    }

    public static KnotPosition MoveInDirection(KnotPosition start, Direction direction)
    {
        return start + MoveAddition[direction];
    }

    public static bool IsRopePositionsTouching(KnotPosition position, KnotPosition other)
    {
        var distance = Math.Sqrt(((other.X - position.X) * (other.X - position.X)) +
                                ((other.Y - position.Y) * (other.Y - position.Y)));
        return distance < 2;
    }

    public override Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default)
    {
        var result = GetLastKnotPositionsForRopeLength(2).ToString();
        return Task.FromResult(result);
    }

    public override Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default)
    {
        var result = GetLastKnotPositionsForRopeLength(10).ToString();
        return Task.FromResult(result);
    }

    private int GetLastKnotPositionsForRopeLength(int ropeLength)
    {
        CreateRope(ropeLength);

        foreach(var movement in inputLines.Select(ParseMoveInput))
        {
            var movesRemaining = movement.Moves;
            while(movesRemaining > 0)
            {
                ProcessRopeMovement(movement.Direction);
                movesRemaining--;
            }
        }

        return visitedLastKnotPosistions.Count;
    }

    private void CreateRope(int numberOfKnots)
    {
        knotPositions = Enumerable.Repeat<KnotPosition>(new(0,0), numberOfKnots).ToArray();
        visitedLastKnotPosistions.Clear();
        visitedLastKnotPosistions.Add(knotPositions.Last());
    }

    private void ProcessRopeMovement(Direction direction)
    {
        knotPositions[0] = MoveInDirection(knotPositions[0], direction);
        ProcessTrailingKnots(1);
    }

    private void ProcessTrailingKnots(int knotIndex)
    {
        if (knotIndex >= knotPositions.Length) return;

        if (!IsRopePositionsTouching(knotPositions[knotIndex-1], knotPositions[knotIndex]))
        {
            var direction = knotPositions[knotIndex-1] - knotPositions[knotIndex];
            var movement = new KnotPosition(Math.Clamp(direction.X, -1, 1), Math.Clamp(direction.Y, -1, 1));
            knotPositions[knotIndex] = knotPositions[knotIndex] + movement;

            if (knotIndex < knotPositions.Length)
                ProcessTrailingKnots(++knotIndex);
            if (knotIndex == knotPositions.Length)
                visitedLastKnotPosistions.Add(knotPositions.Last());
        }
    }
}

public sealed record RopeMovement(Direction Direction, int Moves);
public sealed record KnotPosition(int X, int Y)
{
    public static KnotPosition operator +(KnotPosition a, KnotPosition b)
        => new(a.X + b.X, a.Y + b.Y);

    public static KnotPosition operator -(KnotPosition a, KnotPosition b)
        => new(a.X - b.X, a.Y - b.Y);

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

public enum Direction
{
    Right,
    Up,
    Left,
    Down,
}