using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.Abstraction.Generators;
using Patternify.Abstraction.Internal.Extensions;

namespace Patternify.NullObject.Generators;

[Generator]
internal class NullObjectGenerator : MainGenerator<NullObjectSyntaxReceiver>
{
    private static readonly NullObjectBuilder Builder = new();

    protected override string GenerateCode(AttributeSyntax attribute)
    {
        var interfaceDeclaration = attribute.GetFirstParent<InterfaceDeclarationSyntax>();
        Builder.SetUsings(interfaceDeclaration);
        Builder.SetNamespace(interfaceDeclaration);
        Builder.SetAccessModifier(interfaceDeclaration);
        Builder.SetClassName(interfaceDeclaration);
        Builder.SetInterfaceName(interfaceDeclaration);
        Builder.SetProperties(interfaceDeclaration);
        Builder.SetMethods(interfaceDeclaration);

        var source = Builder.Build();

        Builder.Clear();

        return source;
    }

    protected override string GetNestHintName(AttributeSyntax attribute)
    {
        var sb = new StringBuilder();

        var interfaceName = attribute.GetFirstParent<InterfaceDeclarationSyntax>().Identifier.Text;
        if (interfaceName.StartsWith("I") &&
            interfaceName.Length > 1 &&
            char.IsUpper(interfaceName[1]))
        {
            sb.Append(interfaceName.Substring(1));
        }
        else
        {
            sb.Append(interfaceName);
        }

        sb.Append(".g.cs");

        return sb.ToString();
    }
}