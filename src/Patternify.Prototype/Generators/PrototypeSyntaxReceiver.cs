using Patternify.Abstraction.Generators;

namespace Patternify.Prototype.Generators;

internal sealed class PrototypeSyntaxReceiver : MainSyntaxReceiver
{
    protected override string AttributeName => nameof(PrototypeAttribute);
}