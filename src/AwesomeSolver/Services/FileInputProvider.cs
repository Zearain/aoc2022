namespace AwesomeSolver.Services;

public sealed class FileInputProvider : IInputProvider
{
    private const string INPUT_BASE_PATH = @"./inputs/";
    private const string INPUT_FILE_NAME = "input.txt";

    public async Task<string> GetInputStringAsync(int day)
    {
        var inputPath = Path.Combine(INPUT_BASE_PATH, day.ToString(), INPUT_FILE_NAME);
        var input = await File.ReadAllTextAsync(inputPath);

        return input.Trim().ReplaceLineEndings();
    }
}