using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace Patternify.Abstraction.Analyzers.ClassMustBePartial;

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

    private void AnalyzeNode(SymbolAnalysisContext context)
    {
        var type = (INamedTypeSymbol)context.Symbol;

        foreach (var declaringSyntaxReference in type.DeclaringSyntaxReferences)
        {
            if (declaringSyntaxReference.GetSyntax() is not ClassDeclarationSyntax classDeclaration
                || !ContainAttribute(classDeclaration.AttributeLists.SelectMany(x => x.Attributes))
                || IsPartial(classDeclaration)) continue;

            var error = Diagnostic.Create(
                PatternifyDescriptors.PF0001_ClassMustBePartial,
                classDeclaration.Identifier.GetLocation(),
                type.Name);

            context.ReportDiagnostic(error);
        }
    }

    private bool ContainAttribute(IEnumerable<AttributeSyntax> attributes) =>
        attributes.Any(a => a.Name.ToString() == AttributeName.Replace(nameof(Attribute), string.Empty));

    private static bool IsPartial(ClassDeclarationSyntax classDeclaration) =>
        classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword);
}