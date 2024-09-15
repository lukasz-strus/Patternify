using Microsoft.CodeAnalysis;
using Patternify.Abstraction.Resources;

// ReSharper disable InconsistentNaming

namespace Patternify.Abstraction.Analyzers;

internal class PatternifyDescriptors
{
    public static readonly DiagnosticDescriptor PF0001_ClassMustBePartial =
        new(id: "PF0001",
            title: Labels.PF0001_TITLE,
            messageFormat: Labels.PF0001_MESSAGE,
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            helpLinkUri: "https://github.com/lukasz-strus/Patternify/wiki/Patternify-messages#pf0001");
}