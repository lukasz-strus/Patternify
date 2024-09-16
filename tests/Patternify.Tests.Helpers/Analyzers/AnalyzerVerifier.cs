using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Patternify.Tests.Helpers.Extensions;

namespace Patternify.Tests.Helpers.Analyzers;

public static class AnalyzerVerifier<TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
    public static DiagnosticResult Diagnostic(string diagnosticId) =>
        CSharpAnalyzerVerifier<TAnalyzer, DefaultVerifier>
            .Diagnostic(diagnosticId);

    public static async Task VerifyAnalyzerAsync(
        string source,
        Type attributeType,
        params DiagnosticResult[] expected)
    {
        var test = new AnalyzerTest(source, attributeType, expected);
        await test.RunAsync(CancellationToken.None);
    }

    private class AnalyzerTest : CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
    {
        public AnalyzerTest(
            string source,
            Type attributeType,
            params DiagnosticResult[] expected)
        {
            TestCode = source;
            ExpectedDiagnostics.AddRange(expected);
            ReferenceAssemblies = ReferenceAssemblies.GetPackages();
            TestState.AdditionalReferences.Add(attributeType.Assembly);
        }
    }
}