namespace AwesomeSolver.Core.Solvers.Day11;

public sealed class MonkeyBusinessCalculator
{
    private readonly MonkeyThief[] monkeyThieves;

    public MonkeyBusinessCalculator(string input)
    {
        monkeyThieves = input.Split(Environment.NewLine + Environment.NewLine)
            .Select(x => new MonkeyThief(x)).ToArray();
    }

    public IEnumerable<MonkeyThief> MonkeyThieves => monkeyThieves;

    public IEnumerable<int> MonkeyActivity => monkeyThieves.Select(x => x.InspectedItemsCount);

    public int GetMonkeyBusinessLevel(int topXMonkeys) => MonkeyActivity.OrderDescending().Take(topXMonkeys).Aggregate(1, (a, b) => a * b);

    public void RunRounds(int numberOfRounds)
    {
        for (int i = 0; i < numberOfRounds; i++)
        {
            RunRound();
        }
    }

    private void RunRound()
    {
        foreach(var monkey in monkeyThieves)
        {
            monkey.InspectAndThrowItems(ThrowToMonkey);
        }
    }

    private void ThrowToMonkey(int target, int item)
    {
        monkeyThieves[target].ReceiveItem(item);
    }
}

public sealed class MonkeyThief
{
    public delegate void ThrowToMonkeyDelegate(int target, int item);

    private readonly Queue<int> items = new Queue<int>();
    private readonly string evalString = string.Empty;
    private readonly MonkeyTargetParameters targetParameters;

    private int inspectedItemsCount = 0;

    public MonkeyThief(string input)
    {
        var monkeyDefinitionLines = input.Split(Environment.NewLine);

        // Items
        var startingItems = monkeyDefinitionLines[1].Split(':')[1].Split(',').Select(x => int.Parse(x.Trim()));
        foreach(var item in startingItems) items.Enqueue(item);

        // Eval string
        evalString = monkeyDefinitionLines[2].Split('=')[1].Trim();

        // Target parameters
        targetParameters = new(
            DivisibleBy: int.Parse(monkeyDefinitionLines[3].Split("by")[1].Trim()),
            TrueTarget: int.Parse(monkeyDefinitionLines[4].Split("monkey")[1].Trim()),
            FalseTarget: int.Parse(monkeyDefinitionLines[5].Split("monkey")[1].Trim())
        );
    }

    public IEnumerable<int> Items => items;

    public string EvalString => evalString;

    public MonkeyTargetParameters TargetParameters => targetParameters;

    public int InspectedItemsCount => inspectedItemsCount;

    public static int GetOperationValue(int currentItem, string opInput)
    {
        if (int.TryParse(opInput.Trim(), out var value))
        {
            return value;
        }

        // Assume it's "old"
        return currentItem;
    }

    public static int EvaluateItem(int itemValue, string operation)
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

    public void ReceiveItem(int itemValue)
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

    private void InspectAndThrowItem(int itemValue, ThrowToMonkeyDelegate handler)
    {
        var evaluatedItem = EvaluateItem(itemValue, evalString);

        // Monkey gets bored
        evaluatedItem /= 3;

        if (evaluatedItem % targetParameters.DivisibleBy == 0)
        {
            handler(targetParameters.TrueTarget, evaluatedItem);
        }
        else
        {
            handler(targetParameters.FalseTarget, evaluatedItem);
        }

        inspectedItemsCount++;
    }
}

public sealed record MonkeyTargetParameters(int DivisibleBy, int TrueTarget, int FalseTarget);