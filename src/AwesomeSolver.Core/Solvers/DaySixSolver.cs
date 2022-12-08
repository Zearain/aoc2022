using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

[DaySolver(6)]
public sealed class DaySixSolver : SharedDaySolver
{
    public DaySixSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

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

    public override Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ParseStartPacketIndex(input).ToString());
    }

    public override Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ParsePacketIndex(input, 14).ToString());
    }
}