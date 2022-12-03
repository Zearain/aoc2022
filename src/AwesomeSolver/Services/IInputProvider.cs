namespace AwesomeSolver.Services;

public interface IInputProvider
{
    Task<string> GetInputStringAsync(int day);
}