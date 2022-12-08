using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

[DaySolver(1)]
public sealed class DayOneSolver : SharedDaySolver
{
    public DayOneSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    public override Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default)
    {
        var parsedCaloriesPerElf = GetElfSeparatedCalories(input);
        var result = parsedCaloriesPerElf.Max(x => x.Sum()).ToString();
        return Task.FromResult(result);
    }

    public override Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default)
    {
        var orderedCaloriesPerElf = GetElfSeparatedCalories(input).OrderByDescending(x => x.Sum());
        var result = orderedCaloriesPerElf.Select(x => x.Sum()).Take(3).Sum().ToString();
        return Task.FromResult(result);
    }

    private IEnumerable<IEnumerable<int>> GetElfSeparatedCalories(string input)
    {
        var caloryInputPerElf = input.Split(Environment.NewLine + Environment.NewLine);

        return caloryInputPerElf.Select(x => x.Split(Environment.NewLine).Select(y => int.Parse(y)));
    }
}