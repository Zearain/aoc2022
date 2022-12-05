using AwesomeSolver.Services;

namespace AwesomeSolver.Solvers;

public abstract class BaseSolver
{
    protected readonly IInputProvider inputProvider;

    protected BaseSolver(IInputProvider inputProvider)
    {
        this.inputProvider = inputProvider;
    }

    protected abstract int DayNumber { get; }

    protected string input = string.Empty;

    private async Task GetInputIfNotProvided()
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
}