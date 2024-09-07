using System.Diagnostics.CodeAnalysis;

namespace Patternify.Abstraction.Internal.Exceptions;

[ExcludeFromCodeCoverage]
public abstract class PatternifyException : Exception
{
    protected PatternifyException(string message) : base(message)
    {
    }
}