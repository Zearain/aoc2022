using AwesomeSolver.Core.Attributes;
using AwesomeSolver.Core.Services;
using AwesomeSolver.Core.Solvers;
using FluentAssertions;
using Moq;

namespace AwesomeSolver.Core.Tests;

public class SharedDaySolverTests
{
    private readonly string input = @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8";
    private IInputProvider inputProvider = null!;

    [SetUp]
    public void Setup()
    {
        var mockInputProvider = new Mock<IInputProvider>();
        mockInputProvider.Setup(x => x.GetInputStringAsync(It.IsAny<int>())).ReturnsAsync(input);
        inputProvider = mockInputProvider.Object;
    }

    [Test]
    public async Task GetInputLinesAsyncShouldReturnSixLines()
    {
        var testBaseSolver = new TestSolver(inputProvider);

        var inputLines = await testBaseSolver.GetLinesAsync();

        inputLines.Should().HaveCount(6);
    }
}

[DaySolver(999)]
internal class TestSolver : SharedDaySolver
{
    public TestSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    public async Task<IEnumerable<string>> GetLinesAsync()
    {
        await InitializeAsync();

        return inputLines;
    }

    public override Task<string> SolvePartOneAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<string> SolvePartTwoAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}