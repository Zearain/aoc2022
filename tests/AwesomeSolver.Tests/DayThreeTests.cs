using AwesomeSolver.Core.Services;
using AwesomeSolver.Core.Solvers;
using FluentAssertions;
using Moq;

namespace AwesomeSolver.Core.Tests;

public class DayThreeTests
{
    private readonly string input = @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw";
    private IInputProvider inputProvider = null!;

    private readonly string partOneSolution = "157";
    private readonly string partTwoSolution = "70";

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