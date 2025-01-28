using System.Reflection;

namespace ProjectManagementSystem.Api.Helpers;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}