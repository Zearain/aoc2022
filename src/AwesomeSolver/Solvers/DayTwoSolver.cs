using AwesomeSolver.Services;

namespace AwesomeSolver.Solvers;

public sealed class DayTwoSolver
{
    private const int DAY_NUMBER = 2;

    private readonly IInputProvider inputProvider;

    private string input = string.Empty;

    public DayTwoSolver(IInputProvider inputProvider)
    {
        this.inputProvider = inputProvider;
    }

    public async Task<int> SolvePartOne()
    {
        await GetInputIfNotProvided();

        var rpsRounds = ParseRPSRounds(input);

        return rpsRounds.Sum(x => x.GetPlayerRoundScore);
    }

    private async Task GetInputIfNotProvided()
    {
        if (string.IsNullOrEmpty(input))
        {
            input = await inputProvider.GetInputStringAsync(DAY_NUMBER);
        }
    }

    private static IEnumerable<RPSRound> ParseRPSRounds(string input)
    {
        return input.Split(Environment.NewLine).Select(x => x.Split(' ')).Select(x => ParseRPSRound(x[0], x[1]));
    }

    private static RPSRound ParseRPSRound(string opponent, string player)
    {
        return new RPSRound(ParseRPSHand(opponent), ParseRPSHand(player));
    }

    private static RPSHand ParseRPSHand(string opponentOrPlayerHand)
    {
        switch (opponentOrPlayerHand)
        {
            case "A":
            case "X":
                return RPSHand.Rock;
            case "B":
            case "Y":
                return RPSHand.Paper;
            case "C":
            case "Z":
                return RPSHand.Scissors;
            default:
                throw new ArgumentOutOfRangeException(nameof(opponentOrPlayerHand));
        }
    }
}

public enum RPSHand
{
    None = 0,
    Rock = 1,
    Paper = 2,
    Scissors = 3,
}

public enum RPSRoundResult
{
    Lose = 0,
    Draw = 3,
    Win = 6,
}

public record RPSRound(RPSHand Opponent, RPSHand Player)
{
    private int OpponentHandScore => GetHandScore(Opponent);

    private int PlayerHandScore => GetHandScore(Player);

    private RPSRoundResult RoundResult => GetRoundResult(Opponent, Player);

    public int GetPlayerRoundScore => PlayerHandScore + (int)RoundResult;

    private static int GetHandScore(RPSHand playedHand) => (int)playedHand;

    private static RPSRoundResult GetRoundResult(RPSHand opponent, RPSHand player)
    {
        if (opponent == player)
        {
            return RPSRoundResult.Draw;
        }

        switch (opponent, player)
        {
            case (RPSHand.Rock, RPSHand.Paper):
            case (RPSHand.Paper, RPSHand.Scissors):
            case (RPSHand.Scissors, RPSHand.Rock):
                return RPSRoundResult.Win;
            default:
                return RPSRoundResult.Lose;
        }
    }
}