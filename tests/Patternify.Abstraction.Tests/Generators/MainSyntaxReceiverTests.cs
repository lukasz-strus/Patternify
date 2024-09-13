using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.Abstraction.Generators;
using Patternify.Tests.Helpers;

namespace Patternify.Abstraction.Tests.Generators;

public sealed class MainSyntaxReceiverTests
{
    [Fact]
    public void OnVisitSyntaxNode_ForTestAttribute_AddAttributeToList()
    {
        //Arrange
        var syntaxNode = SyntaxNodeCreator
            .GetSyntaxNodes<AttributeSyntax>(Sources.InputSource)
            .First();

        var syntaxReceiver = new TestSyntaxReceiver();

        //Act
        syntaxReceiver.OnVisitSyntaxNode(syntaxNode);

        //Assert
        syntaxReceiver.Attributes.Should().HaveCount(1);
    }

    [Fact]
    public void OnVisitSyntaxNode_ForNonTestAttribute_NotAddAttributeToList()
    {
        //Arrange
        var syntaxNode = SyntaxNodeCreator
            .GetSyntaxNodes<AttributeSyntax>(Sources.EmptyInputSource)
            .First();

        var syntaxReceiver = new TestSyntaxReceiver();

        //Act
        syntaxReceiver.OnVisitSyntaxNode(syntaxNode);

        //Assert
        syntaxReceiver.Attributes.Should().BeEmpty();
    }
}

file static class Sources
{
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
        
        [NonTest]
        public partial class TestClass
        {
        }
        """;

}

file sealed class TestSyntaxReceiver : MainSyntaxReceiver
{
    protected override string AttributeName => nameof(TestAttribute);
}

[AttributeUsage(AttributeTargets.Class)]
file abstract class TestAttribute : Attribute { }