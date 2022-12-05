using AwesomeSolver.Services;
using AwesomeSolver.Solvers;
using FluentAssertions;
using Moq;

namespace AwesomeSolver.Tests;

public class BaseSolverTests
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
        var testBaseSolver = new TestBaseSolver(inputProvider);

        var inputLines = await testBaseSolver.GetLinesAsync();

        inputLines.Should().HaveCount(6);
    }
}

internal class TestBaseSolver : BaseSolver
{
    public TestBaseSolver(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    protected override int DayNumber => 0;

    public async Task<IEnumerable<string>> GetLinesAsync() => await GetInputLinesAsync();
}