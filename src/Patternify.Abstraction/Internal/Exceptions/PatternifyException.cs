using System.Diagnostics.CodeAnalysis;

namespace Patternify.Abstraction.Internal.Exceptions;

[ExcludeFromCodeCoverage]
public abstract class PatternifyException(string message) : Exception(message);