using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Patternify.NullObject.Generators.Helpers;

internal static class MethodGeneratorHelper
{
    internal static IEnumerable<string> GetVoidMethodsSource(InterfaceDeclarationSyntax @interface)
    {
        var voidMethods = GetMethods(@interface, IsVoidMethod);

        return voidMethods.Select(WriteVoidMethodSource);
    }

    internal static IEnumerable<string> GetNotVoidMethodsSource(InterfaceDeclarationSyntax @interface)
    {
        var notVoidMethods = GetMethods(@interface, IsNotVoidMethod);

        return notVoidMethods.Select(WriteNotVoidMethodSource);
    }

    private static string WriteNotVoidMethodSource(MethodDeclarationSyntax method)
    {
        var parametersSource = GetParametersSource(method);

        return $$"""
                     public {{method.ReturnType}} {{method.Identifier.Text}}({{string.Join(", ", parametersSource)}}) 
                     {
                         return default({{method.ReturnType}});
                     }
                 """;
    }

    private static string WriteVoidMethodSource(MethodDeclarationSyntax method)
    {
        var parametersSource = GetParametersSource(method);

        return $$"""
                 public void {{method.Identifier.Text}}({{string.Join(", ", parametersSource)}}) { }
                 """;
    }

    private static IEnumerable<MethodDeclarationSyntax> GetMethods(
        InterfaceDeclarationSyntax @interface,
        Func<MethodDeclarationSyntax, bool> predicate) =>
        @interface.Members
            .OfType<MethodDeclarationSyntax>()
            .Where(predicate)
            .Distinct();

    private static bool IsVoidMethod(MethodDeclarationSyntax method) =>
        method.ReturnType
            .ToString()
            .Contains("void");

    private static bool IsNotVoidMethod(MethodDeclarationSyntax method) =>
        !IsVoidMethod(method);

    private static IEnumerable<string> GetParametersSource(MethodDeclarationSyntax method)
    {
        var parameters = GetParameters(method);
        return parameters.Select(WriteParameterSource);
    }

    private static string WriteParameterSource(ParameterSyntax parameter) =>
        $"{parameter.Type} {parameter.Identifier.Text}";

    private static IEnumerable<ParameterSyntax> GetParameters(MethodDeclarationSyntax method) =>
        method.ParameterList.Parameters;
}