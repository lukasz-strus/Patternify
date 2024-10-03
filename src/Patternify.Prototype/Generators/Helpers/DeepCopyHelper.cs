using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Patternify.Prototype.Generators.Helpers;

internal static class DeepCopyHelper
{
    internal static string WriteObjectFieldsClone(
        ClassDeclarationSyntax group,
        ICollection<ClassDeclarationSyntax> allClassGroups)
    {
        var typeNames = allClassGroups.Select(y => y.Identifier.Text);

        var properties = group.Members
            .OfType<PropertyDeclarationSyntax>()
            .Distinct()
            .Where(property => typeNames
                .Any(typeName => property.Type.ToString().TrimEnd('?').Contains(typeName)));

        return $"{string.Join("\n\n\t\t",
            properties.Select(p => $"clone.{p.Identifier.Text} = {WriteNewObject(allClassGroups, p)}"))}";
    }

    private static string WriteNewObject(
        IEnumerable<ClassDeclarationSyntax> allClasses,
        PropertyDeclarationSyntax property)
    {
        var typeName = property.Type.ToString();

        var classDeclaration = allClasses
            .FirstOrDefault(@class => @class.Identifier.Text.Contains(typeName.TrimEnd('?')));

        return classDeclaration is null
            ? string.Empty
            : $$"""
                new {{classDeclaration.Identifier.Text}}()
                        {
                            {{WriteAssignFields(classDeclaration, property)}}
                        };
                """;
    }

    private static string WriteAssignFields(ClassDeclarationSyntax classDeclaration,
        PropertyDeclarationSyntax propertyObject)
    {
        var properties = classDeclaration.Members.OfType<PropertyDeclarationSyntax>();

        return string.Join(",\n\t\t\t", properties.Select(p => WriteAssign(p, propertyObject)));
    }

    private static string WriteAssign(PropertyDeclarationSyntax propertyToAssign,
        PropertyDeclarationSyntax propertyObject)
        => $"{propertyToAssign.Identifier.Text} = {propertyObject.Identifier.Text}.{propertyToAssign.Identifier.Text}";
}