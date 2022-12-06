using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Blazor.Services;

internal sealed class HttpInputProvider : IInputProvider
{
    private readonly HttpClient httpClient;

    public HttpInputProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<string> GetInputStringAsync(int day)
    {
        return (await httpClient.GetStringAsync($"inputs/{day}/input.txt"))?.Trim().ReplaceLineEndings() ?? string.Empty;
    }
}