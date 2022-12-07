using AwesomeSolver.Core.Services;
using AwesomeSolver.Core.Solvers;
using FluentAssertions;
using Moq;

namespace AwesomeSolver.Core.Tests;

public class DaySevenTests
{
    private IInputProvider inputProvider = null!;

    [SetUp]
    public void Setup()
    {
        var mockInputProvider = new Mock<IInputProvider>();
        mockInputProvider.Setup(x => x.GetInputStringAsync(It.IsAny<int>())).ReturnsAsync(DaySevenTestData.Input);
        inputProvider = mockInputProvider.Object;
    }

    [TestCaseSource(typeof(DaySevenTestData), nameof(DaySevenTestData.ParseLineIsUserInputTestCases))]
    public bool ParseLineIsUserInputShouldReturnExpectedResult(string line)
    {
        return DaySevenSolver.ParseLineIsUserInput(line);
    }

    [TestCaseSource(typeof(DaySevenTestData), nameof(DaySevenTestData.ParseOutputLineTestCases))]
    public PathInfo ParseOutputLineShouldReturnExpectedTreeNodeData(string line)
    {
        return DaySevenSolver.ParseOutputLine(line);
    }

    [TestCaseSource(typeof(DaySevenTestData), nameof(DaySevenTestData.ParseInputCommandNavigationTestCases))]
    public PathInfo ParseInputCommandShouldReturnExpectedCurrentNode(string line, string currentDirName)
    {

        var solver = new DaySevenSolver(inputProvider);
        solver.FileTree = DaySevenTestData.root;
        solver.CurrentNode = DaySevenTestData.dirs.First(x => x.Data.Name == currentDirName);

        solver.ParseInputCommand(line);

        return solver.CurrentNode.Data;
    }

    [TestCaseSource(typeof(DaySevenTestData), nameof(DaySevenTestData.CalculateDirectoryTotalSizeTestCases))]
    public int CalculateDirectoryTotalSizeShouldReturnExpectedResult(TreeNode<PathInfo> directoryNode)
    {
        return DaySevenSolver.CalculateTotalDirectorySize(directoryNode);
    }

    [Test]
    public void GetTreeDirectoriesShouldReturnExpectedList()
    {
        var expectedDirectories = DaySevenTestData.dirs.Select(x => x.Data).ToArray();

        var result = DaySevenSolver.GetDirectories(DaySevenTestData.root);

        result.Should().HaveSameCount(expectedDirectories);
    }

    [Test]
    public async Task SolvePartOneShouldReturnExpectedResult()
    {
        var solver = new DaySevenSolver(inputProvider);

        var result = await solver.SolvePartOne();

        result.Should().Be("95437");
    }

    [Test]
    public async Task SolvePartTwoShouldReturnExpectedResult()
    {
        var solver = new DaySevenSolver(inputProvider);

        var result = await solver.SolvePartTwo();

        result.Should().Be("24933642");
    }
}

internal static class DaySevenTestData
{
    public static string Input => @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";

    public static IEnumerable<TestCaseData> ParseLineIsUserInputTestCases
    {
        get 
        {
            yield return new TestCaseData("$ cd /").Returns(true);
            yield return new TestCaseData("$ ls").Returns(true);
            yield return new TestCaseData("dir a").Returns(false);
            yield return new TestCaseData("14848514 b.txt").Returns(false);
            yield return new TestCaseData("8504156 c.dat").Returns(false);
            yield return new TestCaseData("dir d").Returns(false);
            yield return new TestCaseData("$ cd a").Returns(true);
            yield return new TestCaseData("dir e").Returns(false);
            yield return new TestCaseData("29116 f").Returns(false);
            yield return new TestCaseData("2557 g").Returns(false);
            yield return new TestCaseData("62596 h.lst").Returns(false);
            yield return new TestCaseData("$ cd e").Returns(true);
            yield return new TestCaseData("583 i").Returns(false);
            yield return new TestCaseData("$ cd ..").Returns(true);
            yield return new TestCaseData("$ cd d").Returns(true);
            yield return new TestCaseData("4060174 j").Returns(false);
            yield return new TestCaseData("8033020 d.log").Returns(false);
            yield return new TestCaseData("5626152 d.ext").Returns(false);
            yield return new TestCaseData("7214296 k").Returns(false);
        }
    }

    public static IEnumerable<TestCaseData> ParseOutputLineTestCases
    {
        get
        {
            yield return new TestCaseData("dir a").Returns(new PathInfo(0, "a"));
            yield return new TestCaseData("14848514 b.txt").Returns(new PathInfo(14848514, "b.txt"));
            yield return new TestCaseData("8504156 c.dat").Returns(new PathInfo(8504156, "c.dat"));
            yield return new TestCaseData("dir d").Returns(new PathInfo(0, "d"));
            yield return new TestCaseData("dir e").Returns(new PathInfo(0, "e"));
            yield return new TestCaseData("29116 f").Returns(new PathInfo(29116, "f"));
            yield return new TestCaseData("2557 g").Returns(new PathInfo(2557, "g"));
            yield return new TestCaseData("62596 h.lst").Returns(new PathInfo(62596, "h.lst"));
            yield return new TestCaseData("583 i").Returns(new PathInfo(583, "i"));
            yield return new TestCaseData("4060174 j").Returns(new PathInfo(4060174, "j"));
            yield return new TestCaseData("8033020 d.log").Returns(new PathInfo(8033020, "d.log"));
            yield return new TestCaseData("5626152 d.ext").Returns(new PathInfo(5626152, "d.ext"));
            yield return new TestCaseData("7214296 k").Returns(new PathInfo(7214296, "k"));
        }
    }

    
    // - / (dir)
    //   - a (dir)
    //     - e (dir)
    //       - i (file, size=584)
    //     - f (file, size=29116)
    //     - g (file, size=2557)
    //     - h.lst (file, size=62596)
    //   - b.txt (file, size=14848514)
    //   - c.dat (file, size=8504156)
    //   - d (dir)
    //     - j (file, size=4060174)
    //     - d.log (file, size=8033020)
    //     - d.ext (file, size=5626152)
    //     - k (file, size=7214296)
    internal static TreeNode<PathInfo> root = new TreeNode<PathInfo>(new(0, "/"));
    internal static TreeNode<PathInfo> dirA = root.AddChild(new(0, "a"));
    internal static TreeNode<PathInfo> dirD = root.AddChild(new(0, "d"));
    internal static TreeNode<PathInfo> dirE = dirA.AddChild(new(0, "e"));
    internal static TreeNode<PathInfo>[] dirs = new[] { root, dirA, dirD, dirE };

    private static bool FilesAddedToTree;

    private static void AddFilesToTree()
    {
        if (FilesAddedToTree) return;

        root.AddChild(new(14848514, "b"));
        root.AddChild(new(8504156, "c"));
        dirA.AddChild(new(29116, "f"));
        dirA.AddChild(new(2557, "g"));
        dirA.AddChild(new(62596, "h.lst"));
        dirE.AddChild(new(584, "i"));
        dirD.AddChild(new(4060174, "j"));
        dirD.AddChild(new(8033020, "d.log"));
        dirD.AddChild(new(5626152, "d.ext"));
        dirD.AddChild(new(7214296, "k"));

        FilesAddedToTree = true;
    }

    public static IEnumerable<TestCaseData> ParseInputCommandNavigationTestCases
    {
        get
        {
            yield return new TestCaseData("$ cd /", "/").Returns(new PathInfo(0, "/"));
            // ls should not perform navigation so we expect the return to be same as input
            yield return new TestCaseData("$ ls", "/").Returns(new PathInfo(0, "/"));
            yield return new TestCaseData("$ cd a", "/").Returns(new PathInfo(0, "a"));
            yield return new TestCaseData("$ cd e", "a").Returns(new PathInfo(0, "e"));
            yield return new TestCaseData("$ cd ..", "e").Returns(new PathInfo(0, "a"));
            yield return new TestCaseData("$ cd ..", "a").Returns(new PathInfo(0, "/"));
            yield return new TestCaseData("$ cd d", "/").Returns(new PathInfo(0, "d"));
        }
    }

    public static IEnumerable<TestCaseData> CalculateDirectoryTotalSizeTestCases
    {
        get
        {
            AddFilesToTree();
            yield return new TestCaseData(root).Returns(48381165);
            yield return new TestCaseData(dirA).Returns(94853);
            yield return new TestCaseData(dirE).Returns(584);
            yield return new TestCaseData(dirD).Returns(24933642);
        }
    }
}