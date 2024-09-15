using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.Abstraction.Analyzers;
using Patternify.Abstraction.Resources;

namespace Patternify.Singleton.CodeFixProviders;

[ExportCodeFixProvider(LanguageNames.CSharp)]
internal sealed class SingletonMustBePartialCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds =>
        [PatternifyDescriptors.PF0001_ClassMustBePartial.Id];

    public override FixAllProvider GetFixAllProvider() =>
        WellKnownFixAllProviders.BatchFixer;

    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        foreach (var diagnostic in context.Diagnostics)
        {
            if (diagnostic.Id != PatternifyDescriptors.PF0001_ClassMustBePartial.Id)
                continue;

            var title = Labels.PF0001_TITLE;

            var action = CodeAction.Create(
                title,
                token => AddPartialKeywordAsync(context, diagnostic, token),
                title);
            context.RegisterCodeFix(action, diagnostic);
        }

        return Task.CompletedTask;
    }

    private static async Task<Document> AddPartialKeywordAsync(
        CodeFixContext context,
        Diagnostic makePartial,
        CancellationToken cancellationToken)
    {
        var root = await context.Document.GetSyntaxRootAsync(cancellationToken);

        if (root is null) return context.Document;

        var classDeclaration = FindClassDeclaration(makePartial, root);

        var partial = SyntaxFactory.Token(SyntaxKind.PartialKeyword);
        var newDeclaration = classDeclaration.AddModifiers(partial);

        var newRoot = root.ReplaceNode(classDeclaration, newDeclaration);
        var newDoc = context.Document.WithSyntaxRoot(newRoot);

        return newDoc;
    }

    private static ClassDeclarationSyntax FindClassDeclaration(
        Diagnostic makePartial,
        SyntaxNode root)
    {
        var diagnosticSpan = makePartial.Location.SourceSpan;

        return root.FindToken(diagnosticSpan.Start)
            .Parent?.AncestorsAndSelf()
            .OfType<ClassDeclarationSyntax>()
            .First()!;
    }
}