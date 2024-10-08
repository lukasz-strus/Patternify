﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.Abstraction.Generators;
using Patternify.Abstraction.Internal.Extensions;

namespace Patternify.Singleton.Generators;

[Generator]
internal class SingletonGenerator : MainGenerator<SingletonSyntaxReceiver>
{
    private static readonly SingletonBuilder Builder = new();

    protected override string GenerateCode(AttributeSyntax attribute)
    {
        var classDeclaration = attribute.GetFirstParent<ClassDeclarationSyntax>();
        if (!classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword)) return string.Empty;

        Builder.SetUsings(classDeclaration);
        Builder.SetNamespace(classDeclaration);
        Builder.SetAccessModifier(classDeclaration);
        Builder.SetClassName(classDeclaration);

        var source = Builder.Build();

        Builder.Clear();

        return source;
    }

    protected override string GetNestHintName(AttributeSyntax attribute) =>
        attribute.GetFirstParent<ClassDeclarationSyntax>().Identifier.Text + ".g.cs";
}