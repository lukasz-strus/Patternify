using Patternify.Abstraction.Generators;

namespace Patternify.Singleton.Generators;

internal sealed class SingletonSyntaxReceiver : MainSyntaxReceiver
{
    protected override string AttributeName => nameof(SingletonAttribute);
}