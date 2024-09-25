using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Patternify.NullObject.Generators.Helpers;

internal static class PropertyGeneratorHelper
{
    internal static IEnumerable<string> GetPropertiesSource(InterfaceDeclarationSyntax @interface)
    {
        var propertySyntaxes = GetPropertySyntaxes(@interface).ToList();
        return propertySyntaxes.Select(WritePropertySource);
    }

    private static IEnumerable<PropertyDeclarationSyntax> GetPropertySyntaxes(InterfaceDeclarationSyntax group) =>
        group.Members.OfType<PropertyDeclarationSyntax>()
            .Distinct();

    private static string WritePropertySource(PropertyDeclarationSyntax property) =>
        $$"""
           public {{property.Type}} {{property.Identifier.Text}} { {{property.AccessorList?.Accessors}} }
          """;
}