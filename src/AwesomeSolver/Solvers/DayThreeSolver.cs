using AwesomeSolver.Services;

namespace AwesomeSolver.Solvers;

public sealed class DayThreeSolver
{
    private const int DAY_NUMBER = 3;

    private readonly IInputProvider inputProvider;

    private string input = string.Empty;

    public DayThreeSolver(IInputProvider inputProvider)
    {
        this.inputProvider = inputProvider;
    }

    public async Task<int> SolvePartOne()
    {
        await GetInputIfNotProvided();

        var elfRucksacks = GetElfRucksacks(input);
        var duplicates = elfRucksacks.Where(x => x.FirstCompartmentItems.Intersect(x.SecondCompartmentItems).Any()).Select(x => x.FirstCompartmentItems.Intersect(x.SecondCompartmentItems));

        return duplicates.Sum(x => x.Sum());
    }

    public async Task<int> SolvePartTwo()
    {
        await GetInputIfNotProvided();

        throw new NotImplementedException();
    }

    private async Task GetInputIfNotProvided()
    {
        if (string.IsNullOrEmpty(input))
        {
            input = await inputProvider.GetInputStringAsync(DAY_NUMBER);
        }
    }

    private static int GetCharNumber(char c)
    {
        var number = (int)c % 32;
        return Char.IsUpper(c) ? number + 26 : number;
    }

    private static IEnumerable<ElfRucksack> GetElfRucksacks(string input)
    {
        return input.Split(Environment.NewLine)
            .Select(x => new[] { x[..(x.Length / 2)], x[(x.Length/2)..] })
            .Select(x => new ElfRucksack(x[0].Select(GetCharNumber), x[1].Select(GetCharNumber)));
    }
}

public record ElfRucksack(IEnumerable<int> FirstCompartmentItems, IEnumerable<int> SecondCompartmentItems);