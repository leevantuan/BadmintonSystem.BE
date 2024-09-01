using System.Reflection;

namespace BadmintonSystem.Contract;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
