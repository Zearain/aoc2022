using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

[DaySolver(4)]
public sealed class DayFourSolver : SharedDaySolver
{
    protected override int DayNumber => 4;

    public DayFourSolver(IInputProvider inputProvider)
        : base(inputProvider)
    {
    }

    public override async Task<string> SolvePartOne()
    {
        var elfPairLines = await this.GetInputLinesAsync();
        return elfPairLines.Select(ParseElfPairAssignment).Where(x => IsFullOverlap(x.FirstElfAssignment, x.SecondElfAssignment)).Count().ToString();
    }

    public override async Task<string> SolvePartTwo()
    {
        var elfPairLines = await this.GetInputLinesAsync();
        return elfPairLines.Select(ParseElfPairAssignment).Where(x => IsAnyOverlap(x.FirstElfAssignment, x.SecondElfAssignment)).Count().ToString();
    }

    public (Range FirstElfAssignment, Range SecondElfAssignment) ParseElfPairAssignment(string elfPairLine)
    {
        var elfPairStartEnds = elfPairLine.Split(',').Select(x => x.Split('-').Select(int.Parse).ToArray()).ToArray();
        return (FirstElfAssignment: elfPairStartEnds[0][0]..elfPairStartEnds[0][1], SecondElfAssignment: elfPairStartEnds[1][0]..elfPairStartEnds[1][1]);
    }

    public bool IsAnyOverlap(Range first, Range second)
    {
        var firstVals = first.ToEnumerable().ToArray();
        var secondVals = second.ToEnumerable().ToArray();

        return firstVals.Any(secondVals.Contains) || secondVals.Any(firstVals.Contains);
    }

    public bool IsFullOverlap(Range first, Range second)
    {
        var firstVals = first.ToEnumerable().ToArray();
        var secondVals = second.ToEnumerable().ToArray();

        return firstVals.All(secondVals.Contains) || secondVals.All(firstVals.Contains);
    }
}

public static class RangeEnumerableExtensions
{
    public static IEnumerable<int> ToEnumerable(this Range range)
    {
        for(int i = range.Start.Value; i <= range.End.Value; i++)
        {
            yield return i;
        }
    }
}
