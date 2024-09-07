using Microsoft.CodeAnalysis;
using Patternify.Abstraction.Generators;

namespace Patternify.Singleton;

[Generator]
internal class SingletonGenerator : MainGenerator<SingletonSyntaxReceiver>
{
}