using AwesomeSolver.Core.Services;
using AwesomeSolver.Core.Solvers;
using FluentAssertions;
using Moq;

namespace AwesomeSolver.Core.Tests;

public class DayTwoTests
{
    private readonly string input = @"A Y
B X
C Z";
    private IInputProvider inputProvider = null!;

    private readonly string partOneSolution = "15";
    private readonly string partTwoSolution = "12";

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
        var solver = new DayTwoSolver(inputProvider);
        await solver.InitializeAsync();

        var part1Result = await solver.SolvePartOneAsync();

        part1Result.Should().Be(partOneSolution);
    }

    [Test]
    public async Task Part2()
    {
        var solver = new DayTwoSolver(inputProvider);
        await solver.InitializeAsync();

        var part2Result = await solver.SolvePartTwoAsync();

        part2Result.Should().Be(partTwoSolution);
    }
}