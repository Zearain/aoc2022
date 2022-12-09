using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

[DaySolver(9)]
public sealed class DayNineSolver : SharedDaySolver
{
    private static readonly IDictionary<Direction, RopePosition> MoveAddition = new Dictionary<Direction, RopePosition>
    {
        { Direction.Right, new(1, 0) },
        { Direction.Up, new(0, 1) },
        { Direction.Left, new(-1, 0) },
        { Direction.Down, new(0, -1) },
    };

    private RopePosition headPosition = new(0,0);
    private RopePosition tailPosition = new(0,0);
    private readonly HashSet<RopePosition> tailVisitedPositions = new HashSet<RopePosition>
    {
        new(0,0),
    };

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

    public static RopePosition MoveInDirection(RopePosition start, Direction direction)
    {
        return start + MoveAddition[direction];
    }

    public static bool IsRopePositionsTouching(RopePosition position, RopePosition other)
    {
        var distance = Math.Sqrt(((other.X - position.X) * (other.X - position.X)) +
                                ((other.Y - position.Y) * (other.Y - position.Y)));
        return distance < 2;
    }

    public override Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default)
    {
        Reset();

        foreach(var movement in inputLines.Select(ParseMoveInput))
        {
            var movesRemaining = movement.Moves;
            while(movesRemaining > 0)
            {
                var startHeadPosition = headPosition;
                headPosition = MoveInDirection(headPosition, movement.Direction);
                if (!IsRopePositionsTouching(headPosition, tailPosition))
                {
                    tailPosition = startHeadPosition;
                    tailVisitedPositions.Add(tailPosition);
                }
                movesRemaining--;
            }
        }

        var result = tailVisitedPositions.Count.ToString();
        return Task.FromResult(result);
    }

    public override Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default)
    {
        return base.SolvePartTwoAsync(cancellationToken);
    }

    private void Reset()
    {
        headPosition = new(0,0);
        tailPosition = new(0,0);
        tailVisitedPositions.Clear();
        tailVisitedPositions.Add(tailPosition);
    }
}

public sealed record RopeMovement(Direction Direction, int Moves);
public sealed record RopePosition(int X, int Y)
{
    public static RopePosition operator +(RopePosition a, RopePosition b)
        => new(a.X + b.X, a.Y + b.Y);
}

public enum Direction
{
    Right,
    Up,
    Left,
    Down,
}