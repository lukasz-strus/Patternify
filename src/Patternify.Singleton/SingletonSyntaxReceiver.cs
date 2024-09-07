using Patternify.Abstraction.Generators;

namespace Patternify.Singleton;

internal class SingletonSyntaxReceiver : MainSyntaxReceiver
{
    protected override string AttributeName => nameof(SingletonAttribute);
}