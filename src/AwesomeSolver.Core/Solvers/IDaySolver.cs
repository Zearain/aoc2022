namespace AwesomeSolver.Core.Solvers;

public interface IDaySolver
{
    Task<string> SolvePartOne();

    Task<string> SolvePartTwo();
} 