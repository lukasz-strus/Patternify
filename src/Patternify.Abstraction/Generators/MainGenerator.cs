using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Patternify.Abstraction.Generators;

internal abstract class MainGenerator<T> : ISourceGenerator
    where T : MainSyntaxReceiver, new()
{
    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (T?)context.SyntaxReceiver;
        if (receiver is null) return;

        foreach (var attribute in receiver.Attributes)
        {
            var source = GenerateCode(attribute);
            var hintName = GetNestHintName(attribute);
            context.AddSource(hintName, SourceText.From(source.Normalize(), Encoding.UTF8));
        }
    }

    protected abstract string GenerateCode(AttributeSyntax attribute);
    protected abstract string GetNestHintName(AttributeSyntax attribute);

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new T());
#if DEBUG
        //if (!Debugger.IsAttached) Debugger.Launch();
#endif
    }
}