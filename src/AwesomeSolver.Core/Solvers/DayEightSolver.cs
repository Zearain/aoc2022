using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

[DaySolver(8)]
public sealed class DayEightSolver : SharedDaySolver
{
    public DayEightSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    protected override int DayNumber => 8;

    public static int[] ParseLineAsArray(string line)
    {
        return line.Select(x => int.Parse(x.ToString())).ToArray();
    }

    public static int[][] ParseInputLinesAsArrays(IEnumerable<string> inputLines)
    {
        return inputLines.Select(ParseLineAsArray).ToArray();
    }

    public static bool IsTreeVisible(int[][] trees, int posX, int posY)
    {
        var edgeY = trees.Length - 1;
        var edgeX = trees[posY].Length - 1;
        if (new[] { 0, edgeX}.Contains(posX) || new[] {0, edgeY}.Contains(posY))
            return true;

        var treeToCheck = trees[posY][posX];

        var treesLeft = trees[posY].Take(posX);
        var treesRight = trees[posY].Skip(posX+1);
        var treesUp = trees.Take(posY).Select(x => x[posX]);
        var treesDown = trees.Skip(posY+1).Select(x => x[posX]);

        return treesLeft.All(x => x < treeToCheck) ||
            treesRight.All(x => x < treeToCheck) || 
            treesUp.All(x => x < treeToCheck) || 
            treesDown.All(x => x < treeToCheck);
    }

    public override async Task<string> SolvePartOne()
    {
        var inputLines = await GetInputLinesAsync();

        var trees = ParseInputLinesAsArrays(inputLines);

        var numVisisble = 0;
        for (int y = 0; y < trees.Length; y++)
        {
            for(int x = 0; x < trees[y].Length; x++)
            {
                if (IsTreeVisible(trees, x, y)) numVisisble++;
            }
        }

        return numVisisble.ToString();
    }

    public override Task<string> SolvePartTwo()
    {
        throw new NotImplementedException();
    }
}