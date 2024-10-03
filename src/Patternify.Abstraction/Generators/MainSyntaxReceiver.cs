using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Patternify.Abstraction.Generators;

internal abstract class MainSyntaxReceiver : ISyntaxReceiver
{
    protected abstract string AttributeName { get; }
    public List<AttributeSyntax> Attributes { get; } = [];

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax
            {
                Name: IdentifierNameSyntax { Identifier.Text: var attributeName }
            } attribute
            || attributeName != AttributeName.Replace(nameof(Attribute), string.Empty)) return;

        Attributes.Add(attribute);
    }
}