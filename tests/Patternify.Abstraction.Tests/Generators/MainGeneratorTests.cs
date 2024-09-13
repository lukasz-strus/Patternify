using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.Abstraction.Generators;
using Patternify.Abstraction.Tests.Helpers;

namespace Patternify.Abstraction.Tests.Generators;

public sealed class MainGeneratorTests
{
    [Fact]
    public void MainGenerator_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(Sources.InputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TestGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        output.Should().Be(Sources.OutputSource);
    }

    [Fact]
    public void MainGenerator_ForEmptyAttribute_ShouldNotGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(Sources.EmptyInputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TestGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        output.Should().Be(Sources.EmptyInputSource);
    }

    [Fact]
    public void MainGenerator_ReturnEmptyDiagnostics()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(Sources.InputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TestGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out _, out var diagnostics);

        // Assert
        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public void MainGenerator_ReturnOutputCompilationWithCorrectSyntaxTreesCount()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(Sources.InputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TestGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);

        // Assert
        outputCompilation.SyntaxTrees.Should().HaveCount(2);
    }

    [Fact]
    public void MainGenerator_ReturnDriverResultWithEmptyDiagnostics()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(Sources.InputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TestGenerator());
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out _, out _);

        // Act
        var runResult = driver.GetRunResult();

        // Assert
        runResult.Diagnostics.Should().BeEmpty();
    }

    [Fact]
    public void MainGenerator_ReturnDriverResultWithCorrectGeneratedTreesLength()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(Sources.InputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TestGenerator());
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out _, out _);

        // Act
        var runResult = driver.GetRunResult();

        // Assert
        runResult.GeneratedTrees.Length.Should().Be(1);
    }

    [Fact]
    public void MainGenerator_ReturnResultWithFactoryGenerator()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(Sources.InputSource);
        var generator = new TestGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out _, out _);

        // Act
        var runResult = driver.GetRunResult();
        var generatorResult = runResult.Results[0];

        // Assert
        generatorResult.Generator.Should().Be(generator);
    }

    [Fact]
    public void MainGenerator_ReturnResultWithEmptyDiagnostics()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(Sources.InputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TestGenerator());
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out _, out _);
        var runResult = driver.GetRunResult();

        // Act
        var generatorResult = runResult.Results[0];

        // Assert
        generatorResult.Diagnostics.Should().BeEmpty();
    }

    [Fact]
    public void MainGenerator_ReturnResultWithGeneratedSourcesWithCorrectLength()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(Sources.InputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TestGenerator());
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out _, out _);
        var runResult = driver.GetRunResult();

        // Act
        var generatorResult = runResult.Results[0];

        // Assert
        generatorResult.GeneratedSources.Length.Should().Be(1);
    }

    [Fact]
    public void MainGenerator_NotReturnExceptions()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(Sources.InputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TestGenerator());
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out _, out _);
        var runResult = driver.GetRunResult();

        // Act
        var generatorResult = runResult.Results[0];

        // Assert
        generatorResult.Exception.Should().BeNull();
    }

}

file static class Sources
{
    public const string OutputSource =
        """
        namespace TestNamespace;
        
        public partial class TestClass
        {
            public void TestMethod()
            {
            }
        }
        """;

    public const string InputSource =
        """
        namespace TestNamespace;
        
        [Test]
        public partial class TestClass
        {
        }
        """;

    public const string EmptyInputSource =
        """
        namespace TestNamespace;
        
        public partial class TestClass
        {
        }
        """;

}

file class TestGenerator : MainGenerator<TestSyntaxReceiver>
{
    protected override string GenerateCode(AttributeSyntax attribute)
        => Sources.OutputSource;

    protected override string GetNestHintName(AttributeSyntax attribute) =>
        "TestClass.g.cs";

}

file sealed class TestSyntaxReceiver : MainSyntaxReceiver
{
    protected override string AttributeName => nameof(TestAttribute);
}

[AttributeUsage(AttributeTargets.Class)]
file abstract class TestAttribute : Attribute { }