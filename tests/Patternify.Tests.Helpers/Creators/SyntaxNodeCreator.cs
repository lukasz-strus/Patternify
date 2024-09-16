using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Patternify.Tests.Helpers.Creators;

public static class SyntaxNodeCreator
{
    public static IEnumerable<T> GetSyntaxNodes<T>(string context)
        where T : SyntaxNode
        => CSharpSyntaxTree
            .ParseText(context)
            .GetRoot()
            .DescendantNodes()
            .OfType<T>();
}