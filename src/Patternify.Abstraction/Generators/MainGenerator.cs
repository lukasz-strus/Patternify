using System.Diagnostics;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Patternify.Singleton")]
namespace Patternify.Abstraction.Generators;

internal abstract class MainGenerator<T> : ISourceGenerator
    where T : MainSyntaxReceiver, new()
{
    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (T?)context.SyntaxReceiver;
        if (receiver?.MemberDeclarationSyntax is null) return;
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new T());
#if DEBUG
        if (!Debugger.IsAttached) Debugger.Launch();
#endif
    }
}