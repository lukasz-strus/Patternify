using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Patternify.Abstraction.Analyzers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.Abstraction.Internal.Extensions;

namespace Patternify.Singleton.Analyzers;


[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class SingletonMustBePartial : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [PatternifyDescriptors.PF0001_ClassMustBePartial];


    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);


        context.RegisterSyntaxNodeAction(CheckAttribute, SyntaxKind.Attribute);
    }

    private static void CheckAttribute(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not AttributeSyntax { Name: IdentifierNameSyntax { Identifier.Text: "Singleton" } } attr) return;

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