namespace AwesomeSolver.Solvers;

public sealed class DayOneSolver
{
    public int SolvePartOne(string input)
    {
        var parsedCaloriesPerElf = GetElfSeparatedCalories(input);

        return parsedCaloriesPerElf.Max(x => x.Sum());
    }

    public int SolvePartTwo(string input)
    {
        var orderedCaloriesPerElf = GetElfSeparatedCalories(input).OrderByDescending(x => x.Sum());

        return orderedCaloriesPerElf.Select(x => x.Sum()).Take(3).Sum();
    }

    private IEnumerable<IEnumerable<int>> GetElfSeparatedCalories(string input)
    {
        var caloryInputPerElf = input.Split(Environment.NewLine + Environment.NewLine);

        return caloryInputPerElf.Select(x => x.Split(Environment.NewLine).Select(y => int.Parse(y)));
    }
}