using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers.Day12;

[DaySolver(12)]
public sealed class DayTwelveSolver : SharedDaySolver
{
    public DayTwelveSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    public override Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default)
    {
        var heightMap = new HeightMap(inputLines);

        var path = heightMap.FindDefaultPath();

        var pathLength = path?.Count() ?? 0;
        
        // Remove 1 because my A* path includes the end.
        pathLength--;

        return Task.FromResult(pathLength.ToString());
    }

    public override Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default)
    {
        var heightMap = new HeightMap(inputLines);

        var path = heightMap.FindShortestLowToEndPath();

        var pathLength = path?.Count() ?? 0;
        
        // Remove 1 because my A* path includes the end.
        pathLength--;

        return Task.FromResult(pathLength.ToString());
    }
}