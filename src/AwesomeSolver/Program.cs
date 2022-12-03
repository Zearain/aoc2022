using AwesomeSolver.Services;
using AwesomeSolver.Solvers;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Advent of Code 2022!");

var defaultInputProvider = new FileInputProvider();

var day1Input = await defaultInputProvider.GetInputStringAsync(1);
var day1Solver = new DayOneSolver();

Console.WriteLine($"DAY 1, PART 1: Most calories = {day1Solver.SolvePartOne(day1Input)}");
Console.WriteLine($"DAY 1, PART 2: Sum calories top 3 elves = {day1Solver.SolvePartTwo(day1Input)}");