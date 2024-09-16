using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Patternify.Singleton.Generators;
using Patternify.Tests.Helpers.Creators;

namespace Patternify.Singleton.Tests.Generators;

public sealed class SingletonGeneratorTests
{
    [Fact]
    public async Task SingletonGenerator_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new SingletonGenerator());
        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();
        // Assert
        await Verify(output);
    }

    private const string InputSource =
        """
        using Patternify.Singleton;

        namespace TestNamespace;

        [Singleton]
        public partial class TestClass
        {
        }
        """;
}