using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

public abstract class SharedDaySolver : IDaySolver
{
    protected readonly IInputProvider inputProvider;

    protected SharedDaySolver(IInputProvider inputProvider)
    {
        this.inputProvider = inputProvider;
    }

    protected int DayNumber
    {
        get
        {
            var dayAttribute = Attribute.GetCustomAttribute(this.GetType(), typeof(DaySolverAttribute)) as DaySolverAttribute;
            if (dayAttribute is null)
                throw new NotImplementedException("Day attribute missing");
            return dayAttribute.Day;
        }
    }

    protected string input = string.Empty;
    protected IEnumerable<string> inputLines = Array.Empty<string>();

    private async Task GetInputIfNotProvided(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(input))
        {
            input = await inputProvider.GetInputStringAsync(DayNumber);
            inputLines = input.Split(Environment.NewLine);
        }
    }

    public void Initialize()
    {
        InitializeAsync().GetAwaiter().GetResult();
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await GetInputIfNotProvided(cancellationToken);
    }

    public virtual string SolvePartOne()
    {
        return SolvePartOneAsync().GetAwaiter().GetResult();
    }

    public virtual Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult("Not Implemented");
    }

    public virtual string SolvePartTwo()
    {
        return SolvePartTwoAsync().GetAwaiter().GetResult();
    }

    public virtual Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult("Not Implemented");
    }
}