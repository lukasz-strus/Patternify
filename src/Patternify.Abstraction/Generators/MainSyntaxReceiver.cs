using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Patternify.Abstraction.Internal.Extensions;

namespace Patternify.Abstraction.Generators;

internal abstract class MainSyntaxReceiver : ISyntaxReceiver
{
    protected abstract string AttributeName { get; }
    internal MemberDeclarationSyntax? MemberDeclarationSyntax { get; private set; }

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax { Name: IdentifierNameSyntax { Identifier.Text: var attributeName } } attribute
            || attributeName == AttributeName) return;
        
        MemberDeclarationSyntax = attribute.GetParent<MemberDeclarationSyntax>();
    }
}