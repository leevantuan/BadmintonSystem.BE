using System.Reflection;

namespace BadmintonSystem.Infrastructure.Bus;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
