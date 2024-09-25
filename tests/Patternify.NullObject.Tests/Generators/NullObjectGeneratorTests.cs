using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Patternify.NullObject.Generators;
using Patternify.Tests.Helpers.Creators;

namespace Patternify.NullObject.Tests.Generators;

public sealed class NullObjectGeneratorTests
{
    [Fact]
    public async Task NullObjectGenerator_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new NullObjectGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSource =
        """
        using Patternify.NullObject;

        namespace Test;

        [NullObject]
        public interface ILog
        {
            public int Id { get; }
            public string Name { get; }
            public string Login(string username, string password);
            public string Logout();
            public void Info(string message);
            public void Warn(string message);
        }
        """;
}