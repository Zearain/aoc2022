using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

public abstract class SharedDaySolver : IDaySolver
{
    protected readonly IInputProvider inputProvider;

    protected SharedDaySolver(IInputProvider inputProvider)
    {
        this.inputProvider = inputProvider;
    }

    protected abstract int DayNumber { get; }

    protected string input = string.Empty;

    protected async Task GetInputIfNotProvided()
    {
        if (string.IsNullOrEmpty(input))
        {
            input = await inputProvider.GetInputStringAsync(DayNumber);
        }
    }

    protected async Task<IEnumerable<string>> GetInputLinesAsync()
    {
        await GetInputIfNotProvided();

        return input.Split(Environment.NewLine);
    }

    public abstract Task<string> SolvePartOne();
    public abstract Task<string> SolvePartTwo();
}