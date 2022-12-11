using AwesomeSolver.Core.Solvers.Day11;
using FluentAssertions;

namespace AwesomeSolver.Core.Tests;

[TestFixture]
public class MonkeyThiefTests
{
    private static string input = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3";

    [Test]
    public void CtorShouldPopulateItemsCorrectly()
    {
        var monkey = new MonkeyThief(input);

        monkey.Items.Should().BeEquivalentTo(new[] {79, 98});
    }

    [TestCase(79, "old", 79)]
    [TestCase(19, "19", 19)]
    public void GetOperationValueShouldReturnExpectedResult(int currentItem, string opInput, int expected)
    {
        var result = MonkeyThief.GetOperationValue(currentItem, opInput);

        result.Should().Be(expected);
    }

    [TestCase(79, "old * 19", 1501)]
    [TestCase(54, "old + 6", 60)]
    [TestCase(79, "old * old", 6241)]
    public void EvaluateItemShouldReturnExpectedResult(int currentItem, string evalOperation, int expected)
    {
        var result = MonkeyThief.EvaluateItem(currentItem, evalOperation);

        result.Should().Be(expected);
    }

    [Test]
    public void CtorShouldSetCorrectEvalString()
    {
        var monkey = new MonkeyThief(input);

        monkey.EvalString.Should().Be("old * 19");
    }

    [Test]
    public void CtorShouldSetCorrectTestParameters()
    {
        var monkey = new MonkeyThief(input);

        monkey.TargetParameters.DivisibleBy.Should().Be(23);
        monkey.TargetParameters.TrueTarget.Should().Be(2);
        monkey.TargetParameters.FalseTarget.Should().Be(3);
    }

    [Test]
    public void ReceiveItemShouldEnqueueReceivedItem()
    {
        var monkey = new MonkeyThief(input);

        monkey.ReceiveItem(99);

        monkey.Items.Should().Contain(99);
        monkey.Items.Should().BeEquivalentTo(new[] {79, 98, 99});
    }

    [Test]
    public void InspectAndThrowItemsShouldCallThrowItemDelegateWithExpectedValues()
    {
        var expected = new[] { (3, 500), (3, 620) };
        var result = new List<(int, int)>();

        var monkey = new MonkeyThief(input);
        
        monkey.InspectAndThrowItems((target, item) => result.Add((target, item)));

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void InspectAndThrowItemsShouldIncrementInspectedItemsCountWithExpectedAmount()
    {
        var monkey = new MonkeyThief(input);
        
        monkey.InspectAndThrowItems((target, item) => Console.WriteLine($"throw: {target}, {item}"));

        monkey.InspectedItemsCount.Should().Be(2);
    }
}

[TestFixture]
public class MonkeyBusinessCalculatorTests
{
    [Test]
    public void CtorShouldPopulateMonkeyThievesWithCorrectCount()
    {
        var calculator = new MonkeyBusinessCalculator(MonkeyBusinessTestData.Input);

        calculator.MonkeyThieves.Should().HaveCount(4);
    }

    [Test]
    public void MonkeyActivityShouldReturnExpectedResultAfter20Rounds()
    {
        var calculator = new MonkeyBusinessCalculator(MonkeyBusinessTestData.Input);

        calculator.RunRounds(20);

        calculator.MonkeyActivity.Should().BeEquivalentTo(new[] {101, 95, 7,105});
    }

    [Test]
    public void MonkeyBusinessLevelShouldReturnExpectedResultAfter20Rounds()
    {
        var calculator = new MonkeyBusinessCalculator(MonkeyBusinessTestData.Input);

        calculator.RunRounds(20);

        calculator.GetMonkeyBusinessLevel(2).Should().Be(10605);
    }
}

internal static class MonkeyBusinessTestData
{
    public static string Input => @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1";
}
