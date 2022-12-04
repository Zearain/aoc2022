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

    public async Task<int> SolvePartTwo()
    {
        await GetInputIfNotProvided();

        var rpsRounds = ParseRPSRounds(input, true);

        return rpsRounds.Sum(x => x.GetPlayerRoundScore);
    }

    private async Task GetInputIfNotProvided()
    {
        if (string.IsNullOrEmpty(input))
        {
            input = await inputProvider.GetInputStringAsync(DAY_NUMBER);
        }
    }

    private static IEnumerable<RPSRound> ParseRPSRounds(string input, bool secondColumnResult = false)
    {
        return input.Split(Environment.NewLine).Select(x => x.Split(' ')).Select(x => secondColumnResult ? ParseOpponentResultRound(x[0], x[1]) : ParseOpponentPlayerRound(x[0], x[1]));
    }

    private static RPSRound ParseOpponentPlayerRound(string opponent, string player)
    {
        return new RPSRound(ParseRPSHand(opponent), ParseRPSHand(player));
    }

    private static RPSRound ParseOpponentResultRound(string opponent, string optimalResult)
    {
        return new RPSRound(ParseRPSHand(opponent), ParseRPSRoundResult(optimalResult));
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

    private static RPSRoundResult ParseRPSRoundResult(string optimalRoundResult)
    {
        return optimalRoundResult switch
        {
            "X" => RPSRoundResult.Lose,
            "Y" => RPSRoundResult.Draw,
            "Z" => RPSRoundResult.Win,
            _ => throw new ArgumentOutOfRangeException(nameof(optimalRoundResult))
        };
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

public record RPSRound
{
    public RPSHand Opponent { get; init; }
    public RPSHand Player { get; init; }

    public RPSRound(RPSHand Opponent, RPSHand Player)
    {
        this.Opponent = Opponent;
        this.Player = Player;
    }

    public RPSRound(RPSHand Opponent, RPSRoundResult desiredResult)
    {
        this.Opponent = Opponent;
        this.Player = RPSCombinations.First(x => x.Key.opponent == Opponent && x.Value == desiredResult).Key.player;
    }

    private int OpponentHandScore => GetHandScore(Opponent);

    private int PlayerHandScore => GetHandScore(Player);

    private RPSRoundResult RoundResult => GetRoundResult(Opponent, Player);

    public int GetPlayerRoundScore => PlayerHandScore + (int)RoundResult;

    private static int GetHandScore(RPSHand playedHand) => (int)playedHand;

    private static RPSRoundResult GetRoundResult(RPSHand opponent, RPSHand player)
    {
        if (RPSCombinations.TryGetValue((opponent, player), out var result))
        {
            return result;
        }

        throw new ArgumentOutOfRangeException();
    }

    private static Dictionary<(RPSHand opponent, RPSHand player), RPSRoundResult> RPSCombinations => new Dictionary<(RPSHand opponent, RPSHand player), RPSRoundResult>
    {
        { (RPSHand.Rock, RPSHand.Rock), RPSRoundResult.Draw },
        { (RPSHand.Paper, RPSHand.Paper), RPSRoundResult.Draw },
        { (RPSHand.Scissors, RPSHand.Scissors), RPSRoundResult.Draw },
        { (RPSHand.Rock, RPSHand.Paper), RPSRoundResult.Win },
        { (RPSHand.Paper, RPSHand.Scissors), RPSRoundResult.Win },
        { (RPSHand.Scissors, RPSHand.Rock), RPSRoundResult.Win },
        { (RPSHand.Rock, RPSHand.Scissors), RPSRoundResult.Lose },
        { (RPSHand.Paper, RPSHand.Rock), RPSRoundResult.Lose },
        { (RPSHand.Scissors, RPSHand.Paper), RPSRoundResult.Lose },
    };
}