using System.Text.Json;

namespace AwesomeSolver.Core.Solvers.Day13;

public sealed class DistressSignalParser
{
    public static IEnumerable<(string Left, string Right)> ParseInputPairs(string input)
    {
        return input.Split(Environment.NewLine + Environment.NewLine)
            .Select(x => {
                var p = x.Split(Environment.NewLine);
                return (Left: p[0], Right: p[1]);
            });
    }

    public static bool IsPairInCorrectOrder((string Left, string Right) inputPair)
    {
        var left = JsonSerializer.Deserialize<JsonElement>(inputPair.Left);
        var right = JsonSerializer.Deserialize<JsonElement>(inputPair.Right);

        return CompareArray(left, right) < 0;
    }

    public static int ComparePackets(string left, string right)
    {
        return CompareArray(JsonSerializer.Deserialize<JsonElement>(left), JsonSerializer.Deserialize<JsonElement>(right));
    }

    public static IEnumerable<int> CorrectlyOrderedPairIndeces(IEnumerable<(string Left, string Right)> inputPairs)
    {
        var result = new HashSet<int>();
        var inputPairsArr = inputPairs.ToArray();
        for (int i = 0; i < inputPairsArr.Length; i++)
        {
            if (IsPairInCorrectOrder(inputPairsArr[i]))
            {
                result.Add(i);
            }
        }
        return result;
    }

    private static int CompareArray(JsonElement left, JsonElement right)
    {
        int result;
        var leftEnumerator = left.EnumerateArray();
        var rightEnumerator = right.EnumerateArray();
        while(leftEnumerator.MoveNext() && rightEnumerator.MoveNext())
        {
            if ((result = CompareElement(leftEnumerator.Current, rightEnumerator.Current)) != 0)
            {
                return result;
            }
        }

        return left.GetArrayLength() - right.GetArrayLength();
    }

    private static int CompareElement(JsonElement left, JsonElement right)
    {
        return (left.ValueKind, right.ValueKind) switch
        {
            (JsonValueKind.Number, JsonValueKind.Number) => left.GetInt32() - right.GetInt32(),
            (_, JsonValueKind.Number) => CompareArray(left, JsonSerializer.Deserialize<JsonElement>($"[{right.GetInt32()}]")),
            (JsonValueKind.Number, _) => CompareArray(JsonSerializer.Deserialize<JsonElement>($"[{left.GetInt32()}]"), right),
            _ => CompareArray(left, right),
        };
    }
}