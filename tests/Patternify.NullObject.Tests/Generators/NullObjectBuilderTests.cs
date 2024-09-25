using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.NullObject.Generators;
using Patternify.Tests.Helpers.Creators;

namespace Patternify.NullObject.Tests.Generators;

public sealed class NullObjectBuilderTests
{
    [Fact]
    internal async Task Build_ForSource_ReturnGeneratedSource()
    {
        // Arrange
        var classSyntax = SyntaxNodeCreator.GetSyntaxNodes<InterfaceDeclarationSyntax>(Source).First();
        var builder = new NullObjectBuilder();

        // Act
        builder.SetUsings(classSyntax);
        builder.SetNamespace(classSyntax);
        builder.SetAccessModifier(classSyntax);
        builder.SetClassName(classSyntax);
        builder.SetInterfaceName(classSyntax);
        builder.SetProperties(classSyntax);
        builder.SetMethods(classSyntax);
        var result = builder.Build();

        // Assert
        await Verify(result);
    }

    [Fact]
    internal async Task Clear_WhenCalled_ClearsAllProperties()
    {
        // Arrange
        var classSyntax = SyntaxNodeCreator.GetSyntaxNodes<InterfaceDeclarationSyntax>(Source).First();
        var builder = new NullObjectBuilder();
        // Act
        builder.SetUsings(classSyntax);
        builder.SetNamespace(classSyntax);
        builder.SetAccessModifier(classSyntax);
        builder.SetClassName(classSyntax);
        builder.SetInterfaceName(classSyntax);
        builder.SetProperties(classSyntax);
        builder.SetMethods(classSyntax);
        builder.Clear();
        var result = builder.Build();

        // Assert
        await Verify(result);
    }

    private const string Source =
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