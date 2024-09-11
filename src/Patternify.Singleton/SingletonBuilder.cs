﻿using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Patternify.Singleton;

internal sealed class SingletonBuilder
{
    private string _usings = string.Empty;
    private string _namespace = string.Empty;
    private string _accessModifier = string.Empty;
    private string _className = string.Empty;

    private readonly List<string> _standardUsings = new()
    {
        "using System;",
        "using System.Collections.Generic;",
        "using System.IO;",
        "using System.Linq;",
        "using System.Net.Http;",
        "using System.Threading;",
        "using System.Threading.Tasks;",
        "using System.Collections.Generic;",
        "using System.Text.Json;"
    };

    internal void SetUsings(ClassDeclarationSyntax @class)
    {
        var usings = @class
            .FirstAncestorOrSelf<CompilationUnitSyntax>()?
            .DescendantNodesAndSelf()
            .OfType<UsingDirectiveSyntax>()
            .Select(x => $"using {x.Name.ToString()};")
            .Distinct()
            .ToList()!;

        var usingsMap = new HashSet<string>(usings);
        foreach (var systemUsing in _standardUsings.Where(systemUsing => !usingsMap.Contains(systemUsing)))
        {
            usingsMap.Add(systemUsing);
        }

        _usings = string.Join(Environment.NewLine, usingsMap);
    }

    internal void SetNamespace(ClassDeclarationSyntax @class)
    {
        var @namespace = @class.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString();

        _namespace = @namespace is null 
            ? string.Empty
            : $"namespace {@namespace};";
    }

    internal void SetAccessModifier(ClassDeclarationSyntax @class) =>
        _accessModifier = @class.Modifiers.First().Text;

    internal void SetClassName(ClassDeclarationSyntax @class) =>
        _className += @class.Identifier.Text;

    internal string Build() =>
        $$"""
        // <auto-generated/>
        {{_usings}}
        
        {{_namespace}}
        
        {{_accessModifier}} sealed partial class {{_className}}
        {
            private static {{_className}} _instance = null;
            private static object obj = new object();
           
            private {{_className}}()
            {
            }
           
            {{_accessModifier}} static {{_className}} GetInstance()
            {
                lock (obj)
                {
                    if(_instance == null)
                    {
                        _instance = new {{_className}}();
                    }
                }
                
                return _instance;
            }
        }
        """;

    internal void Clear() => _usings = _namespace = _accessModifier = _className = string.Empty;
}