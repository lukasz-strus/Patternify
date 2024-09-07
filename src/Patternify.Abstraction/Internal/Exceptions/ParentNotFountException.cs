namespace Patternify.Abstraction.Internal.Exceptions;

internal sealed class ParentNotFountException<T> : PatternifyException
{
    public ParentNotFountException()
        : base($"Parent {typeof(T).Name} not found")
    {
    }
}