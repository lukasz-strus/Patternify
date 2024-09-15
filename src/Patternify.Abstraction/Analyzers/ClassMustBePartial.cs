using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using Patternify.Abstraction.Internal.Extensions;

namespace Patternify.Abstraction.Analyzers;

internal abstract class ClassMustBePartial : DiagnosticAnalyzer
{
    protected abstract string AttributeName { get; }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [PatternifyDescriptors.PF0001_ClassMustBePartial];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);


        context.RegisterSyntaxNodeAction(CheckAttribute, SyntaxKind.Attribute);
    }

    private void CheckAttribute(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not AttributeSyntax { Name: IdentifierNameSyntax { Identifier.Text: var attributeName } } attr
            || attributeName != AttributeName.Replace(nameof(Attribute), string.Empty)) return;

        var @class = attr.GetFirstParent<ClassDeclarationSyntax>();

        if (@class.Modifiers.Any(SyntaxKind.PartialKeyword)) return;

        var classSymbol = context.SemanticModel.GetDeclaredSymbol(@class);

        var diagnostic = Diagnostic.Create(
            PatternifyDescriptors.PF0001_ClassMustBePartial,
            attr.GetLocation(),
            classSymbol?.Name);

        context.ReportDiagnostic(diagnostic);
    }
}