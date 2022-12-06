using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

[DaySolver(6)]
public sealed class DaySixSolver : BaseSolver
{
    public DaySixSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    protected override int DayNumber => 6;

    public static int ParseStartPacketIndex(string input)
    {
        return ParsePacketIndex(input, 4);
    }

    public static int ParsePacketIndex(string input, int uniqueChars)
    {
        for (int i = 0; i < input.Length; i++)
        {
            var packet = input.Skip(i).Take(uniqueChars).Distinct();
            if (packet.Count() == uniqueChars)
            return i + uniqueChars;
        }

        return -1;
    }

    public override async Task<string> SolvePartOne()
    {
        await GetInputIfNotProvided();

        return ParseStartPacketIndex(input).ToString();
    }

    public override async Task<string> SolvePartTwo()
    {
        await GetInputIfNotProvided();

        return ParsePacketIndex(input, 14).ToString();
    }
}