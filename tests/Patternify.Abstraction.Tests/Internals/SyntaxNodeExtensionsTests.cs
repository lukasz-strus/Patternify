using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.Abstraction.Internal.Exceptions;
using Patternify.Abstraction.Internal.Extensions;

namespace Patternify.Abstraction.Tests.Internals;

public sealed class SyntaxNodeExtensionsTests
{
    [Fact]
    public void GetParent_ShouldReturnParent()
    {
        // Arrange
        var syntaxTree = CSharpSyntaxTree.ParseText(TestFile);
        var root = syntaxTree.GetRoot();
        var methodDeclaration = root.DescendantNodes().OfType<MethodDeclarationSyntax>().Single();

        // Act
        var actual = methodDeclaration.GetFirstParent<ClassDeclarationSyntax>();

        // Assert
        actual.Should().Be(root.DescendantNodes().OfType<ClassDeclarationSyntax>().Single());
    }

    [Fact]
    public void GetParent_ShouldThrowException_WhenParentIsNull()
    {
        // Arrange
        var syntaxTree = CSharpSyntaxTree.ParseText(TestFile);
        var root = syntaxTree.GetRoot();
        var methodDeclaration = root.DescendantNodes().OfType<MethodDeclarationSyntax>().Single();

        // Act
        var exception = Record.Exception(() => methodDeclaration.GetFirstParent<NamespaceDeclarationSyntax>());

        // Assert
        exception.Should().BeOfType<ParentNotFountException<NamespaceDeclarationSyntax>>();
        exception!.Message.Should().Be("Parent NamespaceDeclarationSyntax not found");
    }

    private const string TestFile =
        """
        namespace TestNamespace;

        public class TestClass
        {
            public void TestMethod()
            {
            }
        }
        """;
}