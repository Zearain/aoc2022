using AwesomeSolver.Core.Services;
using AwesomeSolver.Core.Solvers;
using FluentAssertions;
using Moq;

namespace AwesomeSolver.Core.Tests;

public class DayFiveTests
{
    private IInputProvider inputProvider = null!;

    [SetUp]
    public void Setup()
    {
        var mockInputProvider = new Mock<IInputProvider>();
        mockInputProvider.Setup(x => x.GetInputStringAsync(It.IsAny<int>())).ReturnsAsync(DayFiveTestData.Input);
        inputProvider = mockInputProvider.Object;
    }

    [Test]
    public async Task GetInputSectionsAsyncShouldReturnTwoStrings()
    {
        var solver = new DayFiveSolver(inputProvider);
        await solver.InitializeAsync();

        var sections = await solver.GetInputSectionsAsync();
        sections.Should().HaveCount(2);

        sections.First().Should().Be(DayFiveTestData.InputCraneSection);
        sections.Last().Should().Be(DayFiveTestData.InputInstructionSection);
    }

    [Test]
    public void ParseNumberOfCrateStacksShouldReturnThree()
    {
        var result = DayFiveSolver.ParseCrateStackNumber(DayFiveTestData.InputCraneSection);
        result.Should().Be(3);
    }

    [Test]
    public void ParseCrateSectionShouldReturnExpectedStackArray()
    {
        var result = DayFiveSolver.ParseInitialCrateStacks(DayFiveTestData.InputCraneSection);

        result.Should().HaveCount(3);
        result[0].Should().BeEquivalentTo(DayFiveTestData.InitialStacks[0]);
        result[1].Should().BeEquivalentTo(DayFiveTestData.InitialStacks[1]);
        result[2].Should().BeEquivalentTo(DayFiveTestData.InitialStacks[2]);
    }

    [Test]
    public void ParseInstructionsSectionShouldReturnExpectedCollection()
    {
        var result = DayFiveSolver.ParseInstructionsSection(DayFiveTestData.InputInstructionSection);
        result.Should().BeEquivalentTo(DayFiveTestData.MoveInstructions);
    }

    [TestCaseSource(typeof(DayFiveTestData), nameof(DayFiveTestData.ProcessMoveInstructionTestCases))]
    public Stack<char>[] ProcessMoveInstructionShouldReturnExpectedStacks(Stack<char>[] initialStacks, MoveInstruction moveInstruction)
    {
        return DayFiveSolver.ProcessMoveInstruction(initialStacks, moveInstruction);
    }

    [TestCaseSource(typeof(DayFiveTestData), nameof(DayFiveTestData.CrateMover9001MoveCases))]
    public Stack<char>[] ProcessCrateMover9001MoveShouldReturnExpectedStacks(Stack<char>[] initialStacks, MoveInstruction moveInstruction)
    {
        return DayFiveSolver.ProcessCrateMover9001Move(initialStacks, moveInstruction);
    }
}

internal static class DayFiveTestData
{
    public static string Input => @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";

    public static string InputCraneSection => @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 ";

    public static Stack<char>[] InitialStacks => new[] {
        new Stack<char>(new[] {'Z', 'N'}),
        new Stack<char>(new[] {'M', 'C', 'D'}),
        new Stack<char>(new[] {'P'}),
    };
    
    public static string InputInstructionSection => @"move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";

    public static MoveInstruction[] MoveInstructions => new[] {
        new MoveInstruction(1, 2, 1),
        new MoveInstruction(3, 1, 3),
        new MoveInstruction(2, 2, 1),
        new MoveInstruction(1, 1, 2),
    };

    public static Stack<char>[][] ProcessMoveInstructionResults => new[] {
        new[] {
            new Stack<char>(new[] {'Z', 'N', 'D'}),
            new Stack<char>(new[] {'M', 'C'}),
            new Stack<char>(new[] {'P'}),
        },
        new[] {
            new Stack<char>(),
            new Stack<char>(new[] {'M', 'C'}),
            new Stack<char>(new[] {'P', 'D', 'N', 'Z'}),
        },
        new[] {
            new Stack<char>(new[] {'C', 'M'}),
            new Stack<char>(),
            new Stack<char>(new[] {'P', 'D', 'N', 'Z'}),
        },
        new[] {
            new Stack<char>(new[] {'C'}),
            new Stack<char>(new[] {'M'}),
            new Stack<char>(new[] {'P', 'D', 'N', 'Z'}),
        },
    };

    public static IEnumerable<TestCaseData> ProcessMoveInstructionTestCases
    {
        get
        {
            yield return new TestCaseData(InitialStacks, MoveInstructions[0]).Returns(ProcessMoveInstructionResults[0]);
            yield return new TestCaseData(ProcessMoveInstructionResults[0], MoveInstructions[1]).Returns(ProcessMoveInstructionResults[1]);
            yield return new TestCaseData(ProcessMoveInstructionResults[1], MoveInstructions[2]).Returns(ProcessMoveInstructionResults[2]);
            yield return new TestCaseData(ProcessMoveInstructionResults[2], MoveInstructions[3]).Returns(ProcessMoveInstructionResults[3]);
        }
    }

    public static Stack<char>[][] CrateMover9001MoveResults => new[] {
        new[] {
            new Stack<char>(new[] {'Z', 'N', 'D'}),
            new Stack<char>(new[] {'M', 'C'}),
            new Stack<char>(new[] {'P'}),
        },
        new[] {
            new Stack<char>(),
            new Stack<char>(new[] {'M', 'C'}),
            new Stack<char>(new[] {'P', 'Z', 'N', 'D'}),
        },
        new[] {
            new Stack<char>(new[] {'M', 'C'}),
            new Stack<char>(),
            new Stack<char>(new[] {'P', 'Z', 'N', 'D'}),
        },
        new[] {
            new Stack<char>(new[] {'M'}),
            new Stack<char>(new[] {'C'}),
            new Stack<char>(new[] {'P', 'Z', 'N', 'D'}),
        },
    };

    public static IEnumerable<TestCaseData> CrateMover9001MoveCases
    {
        get
        {
            yield return new TestCaseData(InitialStacks, MoveInstructions[0]).Returns(CrateMover9001MoveResults[0]);
            yield return new TestCaseData(CrateMover9001MoveResults[0], MoveInstructions[1]).Returns(CrateMover9001MoveResults[1]);
            yield return new TestCaseData(CrateMover9001MoveResults[1], MoveInstructions[2]).Returns(CrateMover9001MoveResults[2]);
            yield return new TestCaseData(CrateMover9001MoveResults[2], MoveInstructions[3]).Returns(CrateMover9001MoveResults[3]);
        }
    }
}