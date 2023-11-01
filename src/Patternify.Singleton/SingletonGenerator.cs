using Microsoft.CodeAnalysis;

namespace Patternify.Singleton;

[Generator]
internal sealed class SingletonGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }
}