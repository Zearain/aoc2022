using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;
using System.Text.RegularExpressions;

namespace AwesomeSolver.Core.Solvers;

[DaySolver(5)]
public sealed class DayFiveSolver : SharedDaySolver
{
    public DayFiveSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    public override async Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default)
    {
        var sections = await GetInputSectionsAsync();

        var crates = ParseInitialCrateStacks(sections[0]);
        var instructions = ParseInstructionsSection(sections[1]);

        foreach(var instruction in instructions)
        {
            crates = ProcessMoveInstruction(crates, instruction);
        }

        return string.Concat(crates.Select(x => x.Peek()));
    }

    public override async Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default)
    {
        var sections = await GetInputSectionsAsync();

        var crates = ParseInitialCrateStacks(sections[0]);
        var instructions = ParseInstructionsSection(sections[1]);

        foreach(var instruction in instructions)
        {
            crates = ProcessCrateMover9001Move(crates, instruction);
        }

        return string.Concat(crates.Select(x => x.Peek()));
    }

    public Task<string[]> GetInputSectionsAsync()
    {
        return Task.FromResult(input.Split(Environment.NewLine + Environment.NewLine));
    }

    public int ParseCrateStackNumber(string crateSectionInput)
    {
        var inputLines = crateSectionInput.Split(Environment.NewLine);
        return inputLines.Last().Split("  ").Length;
    }

    public static Stack<char>[] ParseInitialCrateStacks(string crateSectionInput)
    {
        var inputLines = crateSectionInput.Split(Environment.NewLine);
        var numStacks = inputLines.Last().Split("  ").Length;

        var stacks = new List<char>[numStacks];
        foreach(var inputLine in inputLines.SkipLast(1))
        {
            for(int i = 0; i < numStacks; i++)
            {
                var crate = inputLine.Skip(i * 4).Take(3).ToArray()[1];
                if (!Char.IsLetter(crate)) continue;
                var crateStack = stacks[i] ?? new List<char>();
                crateStack.Add(crate);
                stacks[i] = crateStack;
            }
        }

        return stacks.Select(x => new Stack<char>(x.Reverse<char>())).ToArray();
    }

    public static IEnumerable<MoveInstruction> ParseInstructionsSection(string instructionSection)
    {
        var inputLine = instructionSection.Split(Environment.NewLine);
        return inputLine.Select(ParseMoveInstruction);
    }

    private static MoveInstruction ParseMoveInstruction(string instructionLine)
    {
        var split = Regex.Split(instructionLine, @"\D+");
        var nonEmpty = split.Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();
        return new MoveInstruction(nonEmpty[0], nonEmpty[1], nonEmpty[2]);
    }

    public static Stack<char>[] ProcessMoveInstruction(Stack<char>[] crateStacks, MoveInstruction move)
    {
        for (int i = 0; i < move.Crates; i++)
        {
            ProcessMove(crateStacks, move.From - 1, move.To - 1);
        }
        return crateStacks;
    }

    private static Stack<char>[] ProcessMove(Stack<char>[] crateStacks, int fromIndex, int toIndex)
    {
        var crateToMove = crateStacks[fromIndex].Pop();
        crateStacks[toIndex].Push(crateToMove);
        return crateStacks;
    }

    public static Stack<char>[] ProcessCrateMover9001Move(Stack<char>[] crateStacks, MoveInstruction move)
    {
        var fromIndex = move.From - 1;
        var toIndex = move.To - 1;

        var grabbedCrates = new List<char>();
        for (int i = 0; i < move.Crates; i++)
        {
            grabbedCrates.Add(crateStacks[fromIndex].Pop());
        }

        foreach(var crate in grabbedCrates.Reverse<char>())
        {
            crateStacks[toIndex].Push(crate);
        }

        return crateStacks;
    }
}

public sealed record MoveInstruction(int Crates, int From, int To);