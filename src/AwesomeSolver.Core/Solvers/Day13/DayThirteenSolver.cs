using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers.Day13;

[DaySolver(13)]
public sealed class DayThirteenSolver : SharedDaySolver
{
    public DayThirteenSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    public override Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default)
    {
        var inputPairs = DistressSignalParser.ParseInputPairs(input);
        var correctlyOrderedPairs = DistressSignalParser.CorrectlyOrderedPairIndeces(inputPairs);

        var sumOfIndeces = correctlyOrderedPairs.Select(x => x+1).Sum();

        return Task.FromResult(sumOfIndeces.ToString());
    }

    public override Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default)
    {
        var dividerPackets = new[] {"[[2]]","[[6]]"};
        var allPackets = inputLines.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        allPackets.AddRange(dividerPackets);

        var sortedPackets = allPackets.Order(new PacketComparer()).ToList();

        var firstDividerIndex = sortedPackets.IndexOf(dividerPackets[0]) + 1;
        var secondDividerIndex = sortedPackets.IndexOf(dividerPackets[1]) + 1;

        var decoderKey = firstDividerIndex * secondDividerIndex;
        return Task.FromResult(decoderKey.ToString());
    }
}

public class PacketComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        return DistressSignalParser.ComparePackets(x, y);
    }
}