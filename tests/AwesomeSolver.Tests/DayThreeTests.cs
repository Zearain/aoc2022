using AwesomeSolver.Services;
using AwesomeSolver.Solvers;
using FluentAssertions;
using Moq;

namespace AwesomeSolver.Tests;

public class DayThreeTests
{
    private readonly string dayTwoInput = @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw";
    private IInputProvider inputProvider = null!;

    private readonly int partOneSolution = 157;
    private readonly int partTwoSolution = 12;

    [SetUp]
    public void Setup()
    {
        var mockInputProvider = new Mock<IInputProvider>();
        mockInputProvider.Setup(x => x.GetInputStringAsync(It.IsAny<int>())).ReturnsAsync(dayTwoInput);
        inputProvider = mockInputProvider.Object;
    }

    [Test]
    public async Task Part1()
    {
        var solver = new DayThreeSolver(inputProvider);

        var part1Result = await solver.SolvePartOne();

        part1Result.Should().Be(partOneSolution);
    }

    [Test]
    public async Task Part2()
    {
        var solver = new DayThreeSolver(inputProvider);

        var part2Result = await solver.SolvePartTwo();

        part2Result.Should().Be(partTwoSolution);
    }
}