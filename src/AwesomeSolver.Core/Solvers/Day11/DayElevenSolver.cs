using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers.Day11;

[DaySolver(11)]
public sealed class DayElevenSolver : SharedDaySolver
{
    public DayElevenSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    public override Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default)
    {
        var monkeyBusinessCalculator = new MonkeyBusinessCalculator(input);

        monkeyBusinessCalculator.RunRounds(20);

        var monkeyBusiness = monkeyBusinessCalculator.GetMonkeyBusinessLevel(2);

        return Task.FromResult(monkeyBusiness.ToString());
    }

    public override Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default)
    {
        var monkeyBusinessCalculator = new MonkeyBusinessCalculator(input, false);

        monkeyBusinessCalculator.RunRounds(10000);

        var monkeyBusiness = monkeyBusinessCalculator.GetMonkeyBusinessLevel(2);

        return Task.FromResult(monkeyBusiness.ToString());
    }
}