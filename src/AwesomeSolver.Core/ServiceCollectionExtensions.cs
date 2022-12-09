using AwesomeSolver.Core.Services;
using AwesomeSolver.Core.Solvers;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeSolver.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdventOfCodeSolvers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<DaySolverFactory>();

        // Add all solvers as scoped services
        var implementedSolverTypes = typeof(IDaySolver).Assembly.GetTypes()
            .Where(x => typeof(IDaySolver).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToArray();
        foreach (var solverType in implementedSolverTypes)
        {
            serviceCollection.AddScoped(solverType);
        }

        return serviceCollection;
    }
}