using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace Patternify.Abstraction.Generators;

internal abstract class MainBuilder
{
    private static readonly List<string> StandardUsings =
    [
        "using System;",
        "using System.Collections.Generic;",
        "using System.IO;",
        "using System.Linq;",
        "using System.Net.Http;",
        "using System.Threading;",
        "using System.Threading.Tasks;",
        "using System.Collections.Generic;",
        "using System.Text.Json;"
    ];

    protected StringBuilder Usings { get; set; } = new();
    protected StringBuilder Namespace { get; set; } = new();
    protected StringBuilder AccessModifier { get; set; } = new();
    protected StringBuilder ClassName { get; set; } = new();
    protected StringBuilder InterfaceName { get; set; } = new();

    internal void SetUsings(TypeDeclarationSyntax @class)
    {
        var usings = @class
            .FirstAncestorOrSelf<CompilationUnitSyntax>()?
            .DescendantNodesAndSelf()
            .OfType<UsingDirectiveSyntax>()
            .Select(x => $"using {x.Name};")
            .Distinct()
            .ToList() ?? [];

        var usingsSet = new HashSet<string>(usings);
        foreach (var systemUsing in StandardUsings.Where(systemUsing => !usingsSet.Contains(systemUsing)))
        {
            usingsSet.Add(systemUsing);
        }

        Usings.Clear();
        Usings.Append(string.Join("\n", usingsSet));
    }

    internal void SetNamespace(TypeDeclarationSyntax @class)
    {
        var @namespace = @class.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString()
                         ?? @class.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();

        Namespace.Clear();
        if (@namespace is null) return;
        Namespace.Append($"""namespace {@namespace};""");
    }

    internal void SetAccessModifier(TypeDeclarationSyntax @class)
    {
        AccessModifier.Clear();
        AccessModifier.Append(@class.Modifiers.First().Text);
    }

    internal void SetClassName(ClassDeclarationSyntax @class)
    {
        ClassName.Clear();
        ClassName.Append(@class.Identifier.Text);
    }

    internal virtual void SetClassName(InterfaceDeclarationSyntax @interface)
    {
        ClassName.Clear();

        var interfaceName = @interface.Identifier.Text;
        if (interfaceName.StartsWith("I") &&
            interfaceName.Length > 1 &&
            char.IsUpper(interfaceName[1]))
        {
            ClassName.Append(interfaceName.Substring(1));
        }
        else
        {
            ClassName.Append(interfaceName);
        }
    }

    internal void SetInterfaceName(InterfaceDeclarationSyntax @interface)
    {
        InterfaceName.Clear();
        InterfaceName.Append(@interface.Identifier.Text);
    }

    internal virtual void Clear()
    {
        Usings.Clear();
        Namespace.Clear();
        AccessModifier.Clear();
        ClassName.Clear();
        InterfaceName.Clear();
    }

    internal string Build() => IsEmpty() ? string.Empty : BuildSource();

    protected abstract bool IsEmpty();

    protected abstract string BuildSource();
}