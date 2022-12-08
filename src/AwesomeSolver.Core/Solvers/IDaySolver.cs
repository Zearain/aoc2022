namespace AwesomeSolver.Core.Solvers;

public interface IDaySolver
{
    void Initialize();

    Task InitializeAsync(CancellationToken cancellationToken = default(CancellationToken));

    string SolvePartOne();

    Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default(CancellationToken));

    string SolvePartTwo();

    Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default(CancellationToken));
} 