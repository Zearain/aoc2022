using AwesomeSolver.Console.Services;
using AwesomeSolver.Core;
using AwesomeSolver.Core.Services;
using AwesomeSolver.Core.Solvers;
using Microsoft.Extensions.DependencyInjection;

// Register services
var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<IInputProvider, FileInputProvider>();
serviceCollection.AddAdventOfCodeSolvers();

var serviceProvider = serviceCollection.BuildServiceProvider();

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Advent of Code 2022!");

Console.WriteLine("Choose your day number");

var chosenDayString = Console.ReadLine();
var chosenDay = int.Parse(chosenDayString ?? string.Empty);

using var scope = serviceProvider.CreateScope();

var solverFactory = scope.ServiceProvider.GetRequiredService<DaySolverFactory>();
var daySolver = solverFactory.GetDaySolver(chosenDay);

var part1Solution = await daySolver.SolvePartOne();
Console.WriteLine($"DAY {chosenDay}, PART 1: {part1Solution}");

var part2Solution = await daySolver.SolvePartTwo();
Console.WriteLine($"DAY {chosenDay}, PART 2: {part2Solution}");