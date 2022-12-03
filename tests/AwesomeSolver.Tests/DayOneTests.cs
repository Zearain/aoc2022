using AwesomeSolver.Solvers;
using FluentAssertions;

namespace AwesomeSolver.Tests;

public class DayOneTests
{
    private readonly string dayOneInput = @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000";

    private readonly int partOneSolution = 24000;
    private readonly int partTwoSolution = 45000;

    [Test]
    public void Part1()
    {
        var solver = new DayOneSolver();

        var part1Result = solver.SolvePartOne(dayOneInput);

        part1Result.Should().Be(partOneSolution);
    }

    [Test]
    public void Part2()
    {
        var solver = new DayOneSolver();

        var part2Result = solver.SolvePartTwo(dayOneInput);

        part2Result.Should().Be(partTwoSolution);
    }
}