using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Patternify.Abstraction.Generators;

internal abstract class MainBuilder
{
    private readonly List<string> _standardUsings =
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

    protected string Usings = string.Empty;
    protected string Namespace = string.Empty;
    protected string AccessModifier = string.Empty;
    protected string ClassName = string.Empty;


    internal void SetUsings(ClassDeclarationSyntax @class)
    {
        var usings = @class
            .FirstAncestorOrSelf<CompilationUnitSyntax>()?
            .DescendantNodesAndSelf()
            .OfType<UsingDirectiveSyntax>()
            .Select(x => $"using {x.Name?.ToString()};")
            .Distinct()
            .ToList() ?? [];

        var usingsMap = new HashSet<string>(usings);
        foreach (var systemUsing in _standardUsings.Where(systemUsing => !usingsMap.Contains(systemUsing)))
        {
            usingsMap.Add(systemUsing);
        }

        Usings = string.Join("\n", usingsMap);
    }

    internal void SetNamespace(ClassDeclarationSyntax @class)
    {
        var @namespace = @class.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString()
                         ?? @class.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();

        Namespace = @namespace is null
            ? string.Empty
            : $"namespace {@namespace};";
    }

    internal void SetAccessModifier(ClassDeclarationSyntax @class) =>
        AccessModifier = @class.Modifiers.First().Text;

    internal void SetClassName(ClassDeclarationSyntax @class) =>
        ClassName += @class.Identifier.Text;
    internal void Clear() => Usings = Namespace = AccessModifier = ClassName = string.Empty;

    internal string Build() => IsEmpty() ? string.Empty : BuildSource();

    private bool IsEmpty() => string.IsNullOrWhiteSpace(Usings) && string.IsNullOrWhiteSpace(Namespace) &&
                              string.IsNullOrWhiteSpace(AccessModifier) && string.IsNullOrWhiteSpace(ClassName);

    protected abstract string BuildSource();
}