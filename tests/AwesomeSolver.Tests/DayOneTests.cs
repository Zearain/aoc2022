using AwesomeSolver.Services;
using AwesomeSolver.Solvers;
using FluentAssertions;
using Moq;

namespace AwesomeSolver.Tests;

public class DayOneTests
{
    private readonly string input = @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000";

    private IInputProvider inputProvider = null!;

    private readonly string partOneSolution = "24000";
    private readonly string partTwoSolution = "45000";

    [SetUp]
    public void Setup()
    {
        var mockInputProvider = new Mock<IInputProvider>();
        mockInputProvider.Setup(x => x.GetInputStringAsync(It.IsAny<int>())).ReturnsAsync(input);
        inputProvider = mockInputProvider.Object;
    }

    [Test]
    public async Task Part1()
    {
        var solver = new DayOneSolver(inputProvider);

        var part1Result = await solver.SolvePartOne();

        part1Result.Should().Be(partOneSolution);
    }

    [Test]
    public async Task Part2()
    {
        var solver = new DayOneSolver(inputProvider);

        var part2Result = await solver.SolvePartTwo();

        part2Result.Should().Be(partTwoSolution);
    }
}