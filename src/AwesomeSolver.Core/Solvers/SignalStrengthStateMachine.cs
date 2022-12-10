namespace AwesomeSolver.Core.Solvers;

public sealed class SignalStrengthStateMachine
{
    private Stack<string> remainingInstructions = new Stack<string>();
    private string currentInstruction = string.Empty;
    private byte executionTime = 0;

    private int xRegister = 1;
    private List<int> xValues = new List<int>();
    private int currentCycle = 0;

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
}