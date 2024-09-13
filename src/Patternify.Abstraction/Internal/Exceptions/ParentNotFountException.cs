namespace Patternify.Abstraction.Internal.Exceptions;

internal sealed class ParentNotFountException<T>() : PatternifyException($"Parent {typeof(T).Name} not found");