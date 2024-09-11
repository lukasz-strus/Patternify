using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.Abstraction.Generators;
using Patternify.Abstraction.Internal.Extensions;

namespace Patternify.Singleton;

[Generator]
internal class SingletonGenerator : MainGenerator<SingletonSyntaxReceiver>
{
    private static readonly SingletonBuilder Builder = new();

    protected override string GenerateCode(AttributeSyntax attribute)
    {
        var classDeclaration = attribute.GetParent<ClassDeclarationSyntax>();
        
        Builder.SetUsings(classDeclaration);
        Builder.SetNamespace(classDeclaration);
        Builder.SetAccessModifier(classDeclaration);
        Builder.SetClassSignature(classDeclaration);

        var source = Builder.Build();

        Builder.Clear();

        return source;
    }

    protected override string GetNestHintName(AttributeSyntax attribute)
        => attribute.GetParent<ClassDeclarationSyntax>().Identifier.Text + ".g.cs";
}