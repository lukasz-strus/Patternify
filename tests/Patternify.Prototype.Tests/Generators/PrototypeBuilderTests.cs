using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.Prototype.Generators;
using Patternify.Tests.Helpers.Creators;

namespace Patternify.Prototype.Tests.Generators;

public sealed class PrototypeBuilderTests
{
    [Fact]
    internal async Task Build_ForSource_ReturnGeneratedSource()
    {
        // Arrange
        var allClasses = SyntaxNodeCreator.GetSyntaxNodes<ClassDeclarationSyntax>(Source).ToList();

        var classSyntax = allClasses
            .First(x => x.AttributeLists
                .SelectMany(y => y.Attributes)
                .Any(z => z.Name.ToString() == "Prototype"));

        var builder = new PrototypeBuilder();

        // Act
        builder.SetUsings(classSyntax);
        builder.SetNamespace(classSyntax);
        builder.SetAccessModifier(classSyntax);
        builder.SetClassName(classSyntax);
        builder.SetShallowCopyMethod(classSyntax);
        builder.SetDeepCopyMethod(classSyntax, allClasses);
        var result = builder.Build();
        // Assert
        await Verify(result);
    }

    private const string Source =
        """
        using Patternify.NullObject;

        namespace Test;

        [Prototype]
        public partial class Person
        {
            public string? Name { get; set; }
            public string? LastName { get; set; }
            public Address? PersonAddress { get; set; }
            public Contacts? PersonContacts { get; set; }
        }

        public class Address
        {
            public string? HouseNumber { get; set; }
            public string? Street { get; set; }
            public string? City { get; set; }
        }

        public class Contacts
        {
            public string? PhoneNumber { get; set; }
            public string? Email { get; set; }
        }
        """;
}