using AwesomeSolver.Services;
using AwesomeSolver.Solvers;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Advent of Code 2022!");

Console.WriteLine("Choose your day number");

var chosenDayString = Console.ReadLine();
var chosenDay = int.Parse(chosenDayString ?? string.Empty);

var defaultInputProvider = new FileInputProvider();

switch (chosenDay)
{
    case 1:
        var day1Input = await defaultInputProvider.GetInputStringAsync(1);
        var day1Solver = new DayOneSolver();

        Console.WriteLine($"DAY 1, PART 1: Most calories = {day1Solver.SolvePartOne(day1Input)}");
        Console.WriteLine($"DAY 1, PART 2: Sum calories top 3 elves = {day1Solver.SolvePartTwo(day1Input)}");
        break;
    case 2:
        var day2Solver = new DayTwoSolver(defaultInputProvider);
        var day2Part1Solution = await day2Solver.SolvePartOne();

        Console.WriteLine($"DAY 2, PART 1: Total cheating player score = {day2Part1Solution}");
        break;
    default:
        throw new ArgumentOutOfRangeException(nameof(chosenDay));
}