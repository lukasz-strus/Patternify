using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.Abstraction.Resources;

namespace Patternify.Abstraction.Analyzers.ClassMustBePartial;

[ExportCodeFixProvider(LanguageNames.CSharp)]
internal sealed class ClassMustBePartialCodeFix : CodeFixProvider
{
    private readonly string _diagnosticId = PatternifyDescriptors.PF0001_ClassMustBePartial.Id;
    public override ImmutableArray<string> FixableDiagnosticIds => [_diagnosticId];
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        foreach (var diagnostic in context.Diagnostics)
        {
            if (diagnostic.Id != _diagnosticId) continue;
            
            var action = CodeAction.Create(
                Labels.P0001_CODE_FIX_TITLE,
                async token => await AddPartialKeywordAsync(context, diagnostic, token),
                _diagnosticId);

            context.RegisterCodeFix(action, diagnostic);
        }

        return Task.CompletedTask;
    }

    private static async Task<Document> AddPartialKeywordAsync(
        CodeFixContext context,
        Diagnostic diagnostic,
        CancellationToken cancellationToken)
    {
        var root = await context.Document.GetSyntaxRootAsync(cancellationToken);
        if (root is null) return context.Document;

        var classDeclaration = FindClassDeclaration(diagnostic, root);
        var partial = SyntaxFactory.Token(SyntaxKind.PartialKeyword);

        var newDeclaration = classDeclaration.AddModifiers(partial);

        var newRoot = root.ReplaceNode(classDeclaration, newDeclaration);
        var newDocument = context.Document.WithSyntaxRoot(newRoot);

        return newDocument;
    }

    private static ClassDeclarationSyntax FindClassDeclaration(
        Diagnostic diagnostic,
        SyntaxNode root) =>
        root.FindToken(diagnostic.Location.SourceSpan.Start)
            .Parent?.AncestorsAndSelf()
            .OfType<ClassDeclarationSyntax>()
            .First()!;
}