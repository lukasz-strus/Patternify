using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using Patternify.Abstraction.Internal.Extensions;

namespace Patternify.Abstraction.Analyzers;

internal abstract class ClassMustBePartialAnalyzer : DiagnosticAnalyzer
{
    protected abstract string AttributeName { get; }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
        [PatternifyDescriptors.PF0001_ClassMustBePartial];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);


        context.RegisterSymbolAction(AnalyzeNode, SymbolKind.NamedType);
    }

    private static void AnalyzeNode(SymbolAnalysisContext context)
    {
        var type = (INamedTypeSymbol)context.Symbol;

        foreach (var declaringSyntaxReference in type.DeclaringSyntaxReferences)
        {
            if (declaringSyntaxReference.GetSyntax() is not ClassDeclarationSyntax classDeclaration 
                || classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword))) continue;

            var error = Diagnostic.Create(
                PatternifyDescriptors.PF0001_ClassMustBePartial,
                classDeclaration.Identifier.GetLocation(),
                type.Name);

            context.ReportDiagnostic(error);
        }
    }
}