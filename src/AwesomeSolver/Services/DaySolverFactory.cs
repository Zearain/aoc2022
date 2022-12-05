using AwesomeSolver.Solvers;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeSolver.Services;

public sealed class DaySolverFactory
{
    private readonly IServiceProvider serviceProvider;

    public DaySolverFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IDaySolver GetDaySolver(int day)
    {
        return day switch
        {
            1 => (IDaySolver)serviceProvider.GetRequiredService<DayOneSolver>(),
            2 => (IDaySolver)serviceProvider.GetRequiredService<DayTwoSolver>(),
            3 => (IDaySolver)serviceProvider.GetRequiredService<DayThreeSolver>(),
            4 => (IDaySolver)serviceProvider.GetRequiredService<DayFourSolver>(),
            5 => (IDaySolver)serviceProvider.GetRequiredService<DayFiveSolver>(),
            _ => throw new ArgumentOutOfRangeException(nameof(day))
        };
    }
}