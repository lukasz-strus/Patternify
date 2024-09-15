using Patternify.Abstraction.Generators;

namespace Patternify.Singleton.Generators;

internal class SingletonSyntaxReceiver : MainSyntaxReceiver
{
    protected override string AttributeName => nameof(SingletonAttribute);
}