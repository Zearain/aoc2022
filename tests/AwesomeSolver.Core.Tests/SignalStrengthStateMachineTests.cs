using AwesomeSolver.Core.Solvers;
using FluentAssertions;

namespace AwesomeSolver.Core.Tests;

public class SignalStrengthStateMachineTests
{
    [Test]
    public void ParseInstructionShouldSetExecutionTimeToOneWhenNoop()
    {
        var stateMachine = new SignalStrengthStateMachine();

        stateMachine.ParseInstruction("noop");

        stateMachine.ExectionTime.Should().Be(1);
    }

    [Test]
    public void ParseInstructionShouldSetExecutionTimeToTwoWhenAddx()
    {
        var stateMachine = new SignalStrengthStateMachine();

        stateMachine.ParseInstruction("addx 3");

        stateMachine.ExectionTime.Should().Be(2);
    }

    [Test]
    public void ExecuteInstructionShouldNotIncrementXRegisterWhenNoop()
    {
        var stateMachine = new SignalStrengthStateMachine("noop");

        stateMachine.ExecuteCurrentInstruction();

        stateMachine.XRegister.Should().Be(1);
    }

    [Test]
    public void ExecuteInstructionShouldIncrementXRegisterByValueWhenAddx()
    {
        var stateMachine = new SignalStrengthStateMachine("addx 3");

        stateMachine.ExecuteCurrentInstruction();

        stateMachine.XRegister.Should().Be(4);
    }

    [TestCase("noop")]
    [TestCase("addx 3")]
    public void ExecuteInstructionShouldAlwaysClearCurrentInstruction(string currentInstruction)
    {
        var stateMachine = new SignalStrengthStateMachine(currentInstruction);

        stateMachine.ExecuteCurrentInstruction();

        stateMachine.CurrentInstruction.Should().Be(string.Empty);
    }

    [Test]
    public void RunShouldPopulateXValuesCorrectly()
    {
        var expectedXValues = new[] { 1, 1, 1, 4, 4 };
        var input = new[] { "noop", "addx 3", "addx -5" };

        var stateMachine = new SignalStrengthStateMachine(input);

        stateMachine.Run();

        stateMachine.XValues.Should().BeEquivalentTo(expectedXValues);
    }

    [TestCase(20, 420)]
    [TestCase(60, 1140)]
    [TestCase(100, 1800)]
    [TestCase(140, 2940)]
    [TestCase(180, 2880)]
    [TestCase(220, 3960)]
    public void GetSignalStrengthAtCycleShouldReturnExpectedValueWhenRunWithLargeInput(int cycle, int expected)
    {
        var stateMachine = new SignalStrengthStateMachine(LargeInput.Split(Environment.NewLine));

        stateMachine.Run();

        stateMachine.GetSignalStrengthAtCycle(cycle).Should().Be(expected);
    }

    [Test]
    public void GetScreenImageShouldReturnExpectedResultWhenRunWithLargeInput()
    {
        var stateMachine = new SignalStrengthStateMachine(LargeInput.Split(Environment.NewLine));

        stateMachine.Run();

        stateMachine.GetScreenImage().Should().Be(ExpectedCrtImage);
    }

    private static string LargeInput = @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop";

    private static string ExpectedCrtImage => @"##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######.....";
}