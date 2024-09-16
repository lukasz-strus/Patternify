using Patternify.Abstraction.Analyzers;
using Verify = Patternify.Tests.Helpers.Analyzers.AnalyzerVerifier<
    Patternify.Singleton.Analyzers.SingletonMustBePartialAnalyzer>;

namespace Patternify.Singleton.Tests.Analyzers;

public sealed class SingletonMustBePartialAnalyzer
{
    [Fact]
    public async Task Analyzer_ClassMustBePartial_ShouldThrowError()
    {
        var expected = Verify
            .Diagnostic(PatternifyDescriptors.PF0001_ClassMustBePartial.Id)
            .WithLocation(6, 14)
            .WithArguments("TestClass");

        await Verify.VerifyAnalyzerAsync(InputSourceWithAttributeAndWithoutPartialKeyword, typeof(SingletonAttribute),
            expected);
    }

    [Fact]
    public async Task Analyzer_ClassMustBePartial_ShouldNotThrowError()
    {
        await Verify.VerifyAnalyzerAsync(InputSourceWithAttributeAndWithPartialKeyword, typeof(SingletonAttribute));
    }


    private const string InputSourceWithAttributeAndWithPartialKeyword =
        """
        using Patternify.Singleton;

        namespace TestNamespace;

        [Singleton]
        public partial class TestClass
        {
        }
        """;

    private const string InputSourceWithAttributeAndWithoutPartialKeyword =
        """
        using Patternify.Singleton;

        namespace TestNamespace;

        [Singleton]
        public class TestClass
        {
        }
        """;
}