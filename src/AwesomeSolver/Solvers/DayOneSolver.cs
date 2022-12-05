using AwesomeSolver.Services;

namespace AwesomeSolver.Solvers;

public sealed class DayOneSolver : BaseSolver
{
    public DayOneSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    protected override int DayNumber => 1;

    public override async Task<string> SolvePartOne()
    {
        await GetInputIfNotProvided();
        var parsedCaloriesPerElf = GetElfSeparatedCalories(input);

        return parsedCaloriesPerElf.Max(x => x.Sum()).ToString();
    }

    public override async Task<string> SolvePartTwo()
    {
        await GetInputIfNotProvided();
        var orderedCaloriesPerElf = GetElfSeparatedCalories(input).OrderByDescending(x => x.Sum());

        return orderedCaloriesPerElf.Select(x => x.Sum()).Take(3).Sum().ToString();
    }

    private IEnumerable<IEnumerable<int>> GetElfSeparatedCalories(string input)
    {
        var caloryInputPerElf = input.Split(Environment.NewLine + Environment.NewLine);

        return caloryInputPerElf.Select(x => x.Split(Environment.NewLine).Select(y => int.Parse(y)));
    }
}