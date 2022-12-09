using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

[DaySolver(8)]
public sealed class DayEightSolver : SharedDaySolver
{
    public DayEightSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

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
        var treesToEdge = GetTreesToEdges(trees, posX, posY);

        var treeToCheck = trees[posY][posX];

        return treesToEdge.Left.All(x => x < treeToCheck) ||
            treesToEdge.Right.All(x => x < treeToCheck) ||
            treesToEdge.Up.All(x => x < treeToCheck) ||
            treesToEdge.Down.All(x => x < treeToCheck);
    }

    public static int GetScenicScore(int[][] trees, int posX, int posY)
    {
        var treesToEdge = GetTreesToEdges(trees, posX, posY);
        var walkToEdge = new[] {
            treesToEdge.Left.Reverse(),
            treesToEdge.Right,
            treesToEdge.Up.Reverse(),
            treesToEdge.Down,
        };

        var treeToCheck = trees[posY][posX];

        var directionScores = new List<int>();
        foreach(var treesInDirection in walkToEdge)
        {
            var directionScore = 0;
            foreach(var tree in treesInDirection)
            {
                directionScore++;
                if (tree >= treeToCheck) break;
            }
            directionScores.Add(directionScore);
        }

        return directionScores.Aggregate(1, (a, b) => a * b);
    }

    private static (int[] Left, int[] Right, int[] Up, int[] Down) GetTreesToEdges(int[][] trees, int posX, int posY)
    {
        var treesLeft = trees[posY].Take(posX).ToArray();
        var treesRight = trees[posY].Skip(posX+1).ToArray();
        var treesUp = trees.Take(posY).Select(x => x[posX]).ToArray();
        var treesDown = trees.Skip(posY+1).Select(x => x[posX]).ToArray();

        return (treesLeft, treesRight, treesUp, treesDown);
    }

    public override Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default)
    {
        var trees = ParseInputLinesAsArrays(inputLines);

        var numVisisble = 0;
        for (int y = 0; y < trees.Length; y++)
        {
            for(int x = 0; x < trees[y].Length; x++)
            {
                if (IsTreeVisible(trees, x, y)) numVisisble++;
            }
        }

        return Task.FromResult(numVisisble.ToString());
    }

    public override Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default)
    {
        var trees = ParseInputLinesAsArrays(inputLines);

        var scenicScore = new List<int>();
        for (int y = 0; y < trees.Length; y++)
        {
            for(int x = 0; x < trees[y].Length; x++)
            {
                scenicScore.Add(GetScenicScore(trees, x, y));
            }
        }

        return Task.FromResult(scenicScore.Max().ToString());
    }
}