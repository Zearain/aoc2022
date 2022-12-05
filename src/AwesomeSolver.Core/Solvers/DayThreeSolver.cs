using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

[DaySolver(3)]
public sealed class DayThreeSolver : BaseSolver
{
    public DayThreeSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    protected override int DayNumber => 3;

    public override async Task<string> SolvePartOne()
    {
        var elfRucksacks = GetElfRucksacks(await GetInputLinesAsync());
        var duplicates = elfRucksacks.Where(x => x.FirstCompartmentItems.Intersect(x.SecondCompartmentItems).Any()).Select(x => x.FirstCompartmentItems.Intersect(x.SecondCompartmentItems));

        return duplicates.Sum(x => x.Sum()).ToString();
    }

    public override async Task<string> SolvePartTwo()
    {
        var elfRucksacks = GetElfRucksacks(await GetInputLinesAsync()).ToArray();
        var badges = new List<int>();
        for (int i = 0; i < elfRucksacks.Length; i += 3)
        {
            var elfGroup = elfRucksacks.Skip(i).Take(3);
            badges.Add(elfGroup.SelectMany(x => x.AllItems).GroupBy(x => x).First(x => x.Count() == 3).Key);
        }

        return badges.Sum().ToString();
    }

    private static int GetCharNumber(char c)
    {
        var number = (int)c % 32;
        return Char.IsUpper(c) ? number + 26 : number;
    }

    private static IEnumerable<ElfRucksack> GetElfRucksacks(IEnumerable<string> inputLines)
    {
        return inputLines
            .Select(x => new[] { x[..(x.Length / 2)], x[(x.Length/2)..] })
            .Select(x => new ElfRucksack(x[0].Select(GetCharNumber), x[1].Select(GetCharNumber)));
    }
}

public sealed record ElfRucksack(IEnumerable<int> FirstCompartmentItems, IEnumerable<int> SecondCompartmentItems)
{
    public IEnumerable<int> AllItems => FirstCompartmentItems.Union(SecondCompartmentItems);
};