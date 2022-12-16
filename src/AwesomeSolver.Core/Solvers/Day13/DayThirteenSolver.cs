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
}