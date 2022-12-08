using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Solvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace AwesomeSolver.Core.Services;

public sealed class DaySolverFactory
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger logger;

    private readonly IDictionary<int, Type> dayTypeMapping;

    public int[] ImplmentedDaySolvers => dayTypeMapping.Keys.ToArray();

    public DaySolverFactory(IServiceProvider serviceProvider, ILogger<DaySolverFactory> logger)
    {
        this.serviceProvider = serviceProvider;
        this.logger = logger;

        dayTypeMapping = typeof(IDaySolver).Assembly.GetTypes()
            .Where(x => typeof(IDaySolver).IsAssignableFrom(x) &&
                !x.IsInterface &&
                !x.IsAbstract &&
                x.GetCustomAttribute<DaySolverAttribute>() is not null)
            .ToDictionary(x => x.GetCustomAttribute<DaySolverAttribute>()?.Day ??
                throw new NullReferenceException("DaySolverAttribute can never be null here"));

        logger.LogInformation("DaySolverFactory initialized. Found {days} implemented IDaySolvers.", ImplmentedDaySolvers.Length);
    }

    public IDaySolver GetDaySolver(int day)
    {
        if (!dayTypeMapping.ContainsKey(day))
            throw new ArgumentOutOfRangeException(nameof(day));

        var dayType = dayTypeMapping[day];

        return (IDaySolver)serviceProvider.GetRequiredService(dayType);
    }
}