using Patternify.Abstraction.Analyzers;
using Verify = Patternify.Tests.Helpers.Analyzers.AnalyzerAndCodeFixVerifier<
    Patternify.Singleton.Analyzers.SingletonMustBePartialAnalyzer,
    Patternify.Abstraction.Analyzers.ClassMustBePartial.ClassMustBePartialCodeFix>;

namespace Patternify.Singleton.Tests.Analyzers;

public sealed class SingletonMustBePartialCodeFixVerifier
{
    [Fact]
    public async Task Analyzer_ClassMustBePartial_ShouldThrowError()
    {
        var expected = Verify
            .Diagnostic(PatternifyDescriptors.PF0001_ClassMustBePartial.Id)
            .WithLocation(6, 14)
            .WithArguments("TestClass");

        await Verify.VerifyCodeFixAsync(
            Source,
            FixedSource,
            typeof(SingletonAttribute),
            expected);
    }

    private const string FixedSource =
        """
        using Patternify.Singleton;

        namespace TestNamespace;

        [Singleton]
        public partial class TestClass
        {
        }
        """;

    private const string Source =
        """
        using Patternify.Singleton;

        namespace TestNamespace;

        [Singleton]
        public class TestClass
        {
        }
        """;
}