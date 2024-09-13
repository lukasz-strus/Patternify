using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace Patternify.Abstraction.Tests.Helpers;

internal static class SyntaxNodeCreator
{
    internal static IEnumerable<T> GetSyntaxNodes<T>(string context)
        where T : SyntaxNode
        => CSharpSyntaxTree
            .ParseText(context)
            .GetRoot()
            .DescendantNodes()
            .OfType<T>();
}