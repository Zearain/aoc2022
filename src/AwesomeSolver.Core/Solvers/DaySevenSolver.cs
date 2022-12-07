using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Solvers;

[DaySolver(7)]
public sealed class DaySevenSolver : SharedDaySolver
{
    public DaySevenSolver(IInputProvider inputProvider) : base(inputProvider)
    {
        FileTree = new TreeNode<PathInfo>(new(0, "/"));
        CurrentNode = FileTree;
    }

    public TreeNode<PathInfo> FileTree { get; set; }
    public TreeNode<PathInfo> CurrentNode { get; set; }

    protected override int DayNumber => 7;

    public static bool ParseLineIsUserInput(string line)
    {
        return line.StartsWith('$');
    }

    public static PathInfo ParseOutputLine(string line)
    {
        var splitLine = line.Split(' ');
        var isFile = int.TryParse(splitLine[0], out var size);
        return new(Size: isFile ? size : 0, Name: splitLine[1]);
    }

    public static int CalculateTotalDirectorySize(TreeNode<PathInfo> directoryNode)
    {
        int sum = 0;
        directoryNode.Traverse(x => {
            sum += x.Data.Size;
            return !x.IsLeaf;
        });

        return sum;
    }

    public static IEnumerable<TreeNode<PathInfo>> GetDirectories(TreeNode<PathInfo> directoryNode)
    {
        var directories = new List<TreeNode<PathInfo>>();
        directoryNode.Traverse(x => {
            if (!x.IsLeaf)
                directories.Add(x);
            return true;
        });

        return directories;
    }

    public override async Task<string> SolvePartOne()
    {
        var sums = await GetDirectorySizesAsync();

        return sums.Where(x => x <= 100000).Sum().ToString();
    }

    public override async Task<string> SolvePartTwo()
    {
        var totalDiskSpace = 70000000;
        var requiredDiskSpace = 30000000;

        var directorySizes = await GetDirectorySizesAsync();
        var usedSpace = CalculateTotalDirectorySize(FileTree);

        var unusedSpace = totalDiskSpace - usedSpace;

        return directorySizes.Order().Where(x => (unusedSpace + x) > requiredDiskSpace).First().ToString();
    }

    public void ParseInputCommand(string line)
    {
        var commandLine = line.Substring(2);
        if (!commandLine.StartsWith("cd", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        ExecuteTreeNavigation(commandLine.Substring(3));
    }

    private async Task<IEnumerable<int>> GetDirectorySizesAsync()
    {
        var inputLines = await GetInputLinesAsync();

        PopulatePathTree(inputLines);

        var directories = GetDirectories(FileTree);
        return directories.Select(CalculateTotalDirectorySize);
    }

    private void PopulatePathTree(IEnumerable<string> lines)
    {
        // Reset to default state before starting navigation and population
        CurrentNode = FileTree = new TreeNode<PathInfo>(new(0, "/"));

        foreach(var line in lines)
        {
            if (ParseLineIsUserInput(line))
            {
                ParseInputCommand(line);
            }
            else
            {
                CurrentNode.AddChild(ParseOutputLine(line));
            }
        }
    }

    private void ExecuteTreeNavigation(string toDir)
    {
        TreeNode<PathInfo> newCurrentNode = null!;
        switch (toDir)
        {
            case "/":
                newCurrentNode = FileTree;
                break;
            case "..":
                if (CurrentNode.Parent is null)
                    throw new InvalidOperationException("Can't navigate to parent of orphans");
                newCurrentNode = CurrentNode.Parent;
                break;
            default:
                newCurrentNode = CurrentNode.Children.First(x => x.Data.Name.Equals(toDir, StringComparison.Ordinal));
                break;
        }

        CurrentNode = newCurrentNode;
    }
}

public sealed record PathInfo(int Size, string Name);

public sealed class TreeNode<T>
{
    public delegate bool TraversalNodeDelegate(TreeNode<T> node);

    private readonly T data;
    private readonly TreeNode<T>? parent;
    private readonly List<TreeNode<T>> children;

    public TreeNode(T data)
    {
        this.data = data;
        this.children = new List<TreeNode<T>>();
    }

    public TreeNode(T data, TreeNode<T> parent) : this(data)
    {
        this.parent = parent;
    }

    public int Count => this.children.Count;
    public T Data => this.data;
    public TreeNode<T>? Parent => this.parent;
    public IReadOnlyList<TreeNode<T>> Children => this.children.AsReadOnly();
    public bool IsRoot => this.parent is null;
    public bool IsLeaf => this.Count == 0;

    public TreeNode<T> AddChild(T data)
    {
        var node = new TreeNode<T>(data, this);
        this.children.Add(node);
        return node;
    }

    public TreeNode<T> this[int key] => children[key];

    public void Traverse(TraversalNodeDelegate handler)
    {
        if (!handler(this)) return;
        int i = 0; int l = this.Count;
        for(; i < l; ++i) children[i].Traverse(handler);
    }
}