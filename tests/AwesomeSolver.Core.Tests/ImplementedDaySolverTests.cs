using AwesomeSolver.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeSolver.Core.Tests;

[TestFixture]
public class ImplementedDaySolverTests
{
    private readonly IServiceProvider serviceProvider;

    private static IEnumerable<TestCaseData> ImplementedDayTestCases
    {
        get
        {
            foreach(var dayTestData in ImplementedDayTestData.TestData)
            {
                yield return new TestCaseData(dayTestData.Key);
            }
        }
    }

    public ImplementedDaySolverTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging()
            .AddAdventOfCodeSolvers()
            .AddScoped<IInputProvider, ImplementedDayTestInputProvider>();

        serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [TestCaseSource(nameof(ImplementedDayTestCases))]
    public async Task DaySolver_SolvePartOneAsync_ShouldReturnExpectedResult(int day)
    {
        if (ImplementedDayTestData.TestData[day].Expected[0].Equals("SKIP_TEST()"))
        {
            Assert.Ignore();
        }

        // Arrange
        using var testScope = serviceProvider.CreateScope();
        var inputProvider = (ImplementedDayTestInputProvider)testScope.ServiceProvider.GetRequiredService<IInputProvider>();
        var daySolverFactory = testScope.ServiceProvider.GetRequiredService<DaySolverFactory>();
        var daySolver = daySolverFactory.GetDaySolver(day);
        
        inputProvider.Part1Test();
        await daySolver.InitializeAsync();

        // Act
        var result = await daySolver.SolvePartOneAsync();

        // Assert
        result.Should().Be(ImplementedDayTestData.TestData[day].Expected[0]);
    }

    [TestCaseSource(nameof(ImplementedDayTestCases))]
    public async Task DaySolver_SolvePartTwoAsync_ShouldReturnExpectedResult(int day)
    {
        if (ImplementedDayTestData.TestData[day].Expected[1].Equals("SKIP_TEST()"))
        {
            Assert.Ignore();
        }

        // Arrange
        using var testScope = serviceProvider.CreateScope();
        var inputProvider = (ImplementedDayTestInputProvider)testScope.ServiceProvider.GetRequiredService<IInputProvider>();
        var daySolverFactory = testScope.ServiceProvider.GetRequiredService<DaySolverFactory>();
        var daySolver = daySolverFactory.GetDaySolver(day);
        
        inputProvider.Part2Test();
        await daySolver.InitializeAsync();

        // Act
        var result = await daySolver.SolvePartTwoAsync();

        // Assert
        result.Should().Be(ImplementedDayTestData.TestData[day].Expected[1]);
    }
}

internal class ImplementedDayTestInputProvider : IInputProvider
{
    private bool isPart2;

    public Task<string> GetInputStringAsync(int day)
    {
        var testData = ImplementedDayTestData.TestData[day];

        if (isPart2 && testData.Inputs.Length > 1)
            return Task.FromResult(testData.Inputs[1]);
        
        return Task.FromResult(testData.Inputs[0]);
    }

    public void Part1Test() => isPart2 = false;
    public void Part2Test() => isPart2 = true;
}

internal static class ImplementedDayTestData
{
    internal static IDictionary<int, (string[] Inputs, string[] Expected)> TestData = new Dictionary<int, (string[] Inputs, string[] Expected)>
    {
        { 
            1, (new[]
            {
                @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000"
        }, new[] { "24000", "45000" })},
        {
            2, (new[] 
            {
                @"A Y
B X
C Z"
            }, new[] { "15", "12" })
        },
        {
            3, (new[]
            {
                @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw"
            }, new[] { "157", "70" })
        },
        {
            4, (new[]
            {
                @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8"
            }, new[] { "2", "4" })
        },
        {
            5, (new[]
            {
                @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2"
            }, new[] {"CMZ", "MCD"})
        },
        {
            6, (new[]
            {
                @"bvwbjplbgvbhsrlpgdmjqwftvncz",
                @"nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg"
            }, new[] { "5", "29" })
        },
        {
            7, (new[]
            {
                @"$ cd /
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
7214296 k"
            }, new[] { "95437", "24933642" })
        },
        {
            8, (new[]
            {
                @"30373
25512
65332
33549
35390"
            }, new[] { "21", "8" })
        },
        {
            9, (new[]
            {
                @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2",
                @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20"
            }, new[] { "13", "36" })
        },
        {
            10, (new[]
            {
                @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop"
            }, new[] { "13140", @"##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######....." })
        },
        {
            11, (new[]
            {
                MonkeyBusinessTestData.Input,
            }, new[] { "10605", "SKIP_TEST()"})//"2713310158" })
        }
    };
}