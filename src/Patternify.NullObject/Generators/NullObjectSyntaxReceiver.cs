using Patternify.Abstraction.Generators;

namespace Patternify.NullObject.Generators;

internal sealed class NullObjectSyntaxReceiver : MainSyntaxReceiver
{
    protected override string AttributeName => nameof(NullObjectAttribute);
}