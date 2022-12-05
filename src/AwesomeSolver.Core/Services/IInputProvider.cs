namespace AwesomeSolver.Core.Services;

public interface IInputProvider
{
    Task<string> GetInputStringAsync(int day);
}