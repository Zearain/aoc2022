using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

[DaySolver(10)]
public sealed class DayTenSolver : SharedDaySolver
{
    public DayTenSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    public override Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default)
    {
        var stateMachine = new SignalStrengthStateMachine(inputLines);

        stateMachine.Run();

        var signalStrengths = new List<int>();
        for (int i = 20; i < 221; i += 40)
        {
            signalStrengths.Add(stateMachine.GetSignalStrengthAtCycle(i));
        }

        var result = signalStrengths.Sum();
        return Task.FromResult(result.ToString());
    }

    public override Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default)
    {
        return base.SolvePartTwoAsync(cancellationToken);
    }
}