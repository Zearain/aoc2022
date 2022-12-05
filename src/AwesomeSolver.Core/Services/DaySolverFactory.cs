using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Solvers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AwesomeSolver.Core.Services;

public sealed class DaySolverFactory
{
    private readonly IServiceProvider serviceProvider;

    public DaySolverFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IDaySolver GetDaySolver(int day)
    {
        var dayType = typeof(IDaySolver).Assembly.GetTypes()
            .FirstOrDefault(x => typeof(IDaySolver).IsAssignableFrom(x) &&
                !x.IsInterface && 
                !x.IsAbstract &&
                x.GetCustomAttribute<DaySolverAttribute>()?.Day == day);

        if (dayType is null)
            throw new ArgumentOutOfRangeException(nameof(day));
        
        return (IDaySolver)serviceProvider.GetRequiredService(dayType);
    }
}