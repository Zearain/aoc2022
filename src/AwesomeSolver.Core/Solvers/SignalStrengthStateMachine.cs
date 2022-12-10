namespace AwesomeSolver.Core.Solvers;

public sealed class SignalStrengthStateMachine
{
    const int crtHeight = 6;
    const int crtWidth = 40;

    private Stack<string> remainingInstructions = new Stack<string>();
    private string currentInstruction = string.Empty;
    private byte executionTime = 0;

    private int xRegister = 1;
    private List<int> xValues = new List<int>();
    private int currentCycle = 0;

    private char[,] crtScreen = new char[crtHeight,crtWidth];

    public SignalStrengthStateMachine()
    {
    }

    public SignalStrengthStateMachine(
        string currentInstruction,
        byte executionTime = 0,
        int xRegister = 1,
        int currentCycle = 0)
    {
        this.currentInstruction = currentInstruction;
        this.executionTime = executionTime;
        this.xRegister = xRegister;
        this.currentCycle = currentCycle;
    }

    public SignalStrengthStateMachine(IEnumerable<string> instructions)
    {
        remainingInstructions = new Stack<string>(instructions.Reverse());
    }

    public string CurrentInstruction => currentInstruction;
    public byte ExectionTime => executionTime;
    public int XRegister => xRegister;

    public IReadOnlyList<int> XValues => xValues.AsReadOnly();

    public void ExecuteCurrentInstruction()
    {
        var split = currentInstruction.Split(' ');
        currentInstruction = string.Empty;
        if (split.Length < 2)
        {
            return;
        }

        xRegister += int.Parse(split[1]);
    }

    public void ParseInstruction(string instruction)
    {
        if (instruction.StartsWith("addx", StringComparison.OrdinalIgnoreCase))
            executionTime = 2;
        else
            executionTime = 1;
        currentInstruction = instruction;
    }

    public void Run()
    {
        while (true)
        {
            if (currentInstruction == string.Empty && remainingInstructions.Count < 1) return;
            if (currentInstruction == string.Empty) ParseInstruction(remainingInstructions.Pop());

            
            // CRT Drawing
            var xCoverValues = new[] { xRegister, xRegister+1, xRegister-1 };
            var row = (int)(currentCycle / crtWidth);
            var column = currentCycle % crtWidth;
            crtScreen[row, column] = xCoverValues.Contains(column) ? '#' : '.';

            // The cycle
            currentCycle++;
            xValues.Add(XRegister);
            executionTime--;

            if (executionTime < 1) ExecuteCurrentInstruction();
        }
    }

    public int GetSignalStrengthAtCycle(int cycle)
    {
        return xValues[cycle-1] * cycle;
    }

    public string GetScreenImage()
    {
        var rows = new string[crtHeight];
        for (int y = 0; y < crtHeight; y++)
        {
            var columns = new char[crtWidth];
            for (int x = 0; x < crtWidth; x++)
            {
                columns[x] = crtScreen[y,x];
            }
            rows[y] = columns is null ? string.Empty : new string(columns);
        }
        return string.Join(Environment.NewLine, rows);
    }
}