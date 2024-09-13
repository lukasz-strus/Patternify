using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.Tests.Helpers;

namespace Patternify.Singleton.Tests;

public sealed class SingletonBuilderTests
{
    [Fact]
    internal async Task Build_ForSource_ReturnGeneratedSource()
    {
        // Arrange
        var classSyntax = SyntaxNodeCreator.GetSyntaxNodes<ClassDeclarationSyntax>(Source).First();
        var builder = new SingletonBuilder();

        // Act
        builder.SetUsings(classSyntax);
        builder.SetNamespace(classSyntax);
        builder.SetAccessModifier(classSyntax);
        builder.SetClassName(classSyntax);
        var result = builder.Build();

        // Assert
        await Verify(result);
    }

    private const string Source = """
        using Patternify.Singleton;
        
        namespace Test;
        
        [Singleton]
        public partial class TestSingleton
        {
            public string Property { get; set; } = string.Empty;
        }
        """;

}