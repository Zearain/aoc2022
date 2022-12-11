using System.Numerics;

namespace AwesomeSolver.Core.Solvers.Day11;

public sealed class MonkeyBusinessCalculator
{
    private readonly MonkeyThief[] monkeyThieves;

    public MonkeyBusinessCalculator(string input, bool divideByThreeAfterEval = true)
    {
        monkeyThieves = input.Split(Environment.NewLine + Environment.NewLine)
            .Select(x => new MonkeyThief(x, divideByThreeAfterEval)).ToArray();
    }

    public IEnumerable<MonkeyThief> MonkeyThieves => monkeyThieves;

    public IEnumerable<long> MonkeyActivity => monkeyThieves.Select(x => x.InspectedItemsCount);

    public long GetMonkeyBusinessLevel(int topXMonkeys) => MonkeyActivity.OrderDescending().Take(topXMonkeys).Aggregate((long)1, (a, b) => a * b);

    public void RunRounds(int numberOfRounds)
    {
        for (int i = 0; i < numberOfRounds; i++)
        {
            RunRound();
        }
    }

    private void RunRound()
    {
        for(var i = 0; i < monkeyThieves.Length; i++)
        {
            var monkey = monkeyThieves[i];
            monkey.InspectAndThrowItems(ThrowToMonkey);
        }
    }

    private void ThrowToMonkey(int target, BigInteger item)
    {
        monkeyThieves[target].ReceiveItem(item);
    }
}

public sealed class MonkeyThief
{
    public delegate void ThrowToMonkeyDelegate(int target, BigInteger item);

    private readonly Queue<BigInteger> items = new Queue<BigInteger>();
    private readonly string evalString = string.Empty;
    private readonly MonkeyTargetParameters targetParameters;
    private readonly bool divideByThreeAfterEval = true;

    private long inspectedItemsCount = 0;

    public MonkeyThief(string input, bool divideByThreeAfterEval = true)
    {
        var monkeyDefinitionLines = input.Split(Environment.NewLine);

        // Items
        var startingItems = monkeyDefinitionLines[1].Split(':')[1].Split(',').Select(x => long.Parse(x.Trim()));
        foreach(var item in startingItems) items.Enqueue(item);

        // Eval string
        evalString = monkeyDefinitionLines[2].Split('=')[1].Trim();

        // Target parameters
        targetParameters = new(
            DivisibleBy: int.Parse(monkeyDefinitionLines[3].Split("by")[1].Trim()),
            TrueTarget: int.Parse(monkeyDefinitionLines[4].Split("monkey")[1].Trim()),
            FalseTarget: int.Parse(monkeyDefinitionLines[5].Split("monkey")[1].Trim())
        );

        this.divideByThreeAfterEval = divideByThreeAfterEval;
    }

    public IEnumerable<BigInteger> Items => items;

    public string EvalString => evalString;

    public MonkeyTargetParameters TargetParameters => targetParameters;

    public long InspectedItemsCount => inspectedItemsCount;

    public static BigInteger GetOperationValue(BigInteger currentItem, string opInput)
    {
        if (long.TryParse(opInput.Trim(), out var value))
        {
            return value;
        }

        // Assume it's "old"
        return currentItem;
    }

    public static BigInteger EvaluateItem(BigInteger itemValue, string operation)
    {
        var splitOp = operation.Split(' ');
        var a = GetOperationValue(itemValue, splitOp[0]);
        var b = GetOperationValue(itemValue, splitOp[2]);

        return splitOp[1].Trim() switch
        {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" => a / b,
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        };
    }

    public void ReceiveItem(BigInteger itemValue)
    {
        items.Enqueue(itemValue);
    }

    public void InspectAndThrowItems(ThrowToMonkeyDelegate handler)
    {
        while(items.Count > 0)
        {
            InspectAndThrowItem(items.Dequeue(), handler);
        }
    }

    private void InspectAndThrowItem(BigInteger itemValue, ThrowToMonkeyDelegate handler)
    {
        var evaluatedItem = EvaluateItem(itemValue, evalString);

        // Monkey gets bored
        if (divideByThreeAfterEval)
        {
            evaluatedItem = evaluatedItem / 3;
        }

        if (evaluatedItem % targetParameters.DivisibleBy == 0)
        {
            handler(targetParameters.TrueTarget, evaluatedItem);
        }
        else
        {
            handler(targetParameters.FalseTarget, evaluatedItem);
        }

        inspectedItemsCount += 1;
    }
}

public sealed record MonkeyTargetParameters(int DivisibleBy, int TrueTarget, int FalseTarget);