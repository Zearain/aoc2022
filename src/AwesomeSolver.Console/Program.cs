using AwesomeSolver.Console.Services;
using AwesomeSolver.Core;
using AwesomeSolver.Core.Services;
using AwesomeSolver.Core.Solvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Register services
var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging();
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

// Initialize input
await daySolver.InitializeAsync();

var part1Solution = await daySolver.SolvePartOneAsync();
Console.WriteLine($"DAY {chosenDay}, PART 1: {part1Solution}");

var part2Solution = await daySolver.SolvePartTwoAsync();
Console.WriteLine($"DAY {chosenDay}, PART 2: {part2Solution}");