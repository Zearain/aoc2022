using AwesomeSolver.Core.Services;

namespace AwesomeSolver.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DaySolverAttribute : Attribute
{
    public int Day { get; }

    public DaySolverAttribute(int day)
    {
        Day = day;
    }
}