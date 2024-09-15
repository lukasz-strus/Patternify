using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using Patternify.Abstraction.Analyzers.DiagnosticAnalyzers;

namespace Patternify.Singleton.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class SingletonMustBePartialAnalyzer : ClassMustBePartialAnalyzer
{
    protected override string AttributeName => nameof(SingletonAttribute);
}