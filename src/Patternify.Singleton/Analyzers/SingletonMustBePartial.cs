using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using Patternify.Abstraction.Analyzers;

namespace Patternify.Singleton.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class SingletonMustBePartial : ClassMustBePartial
{
    protected override string AttributeName => nameof(SingletonAttribute);
}