using Microsoft.CodeAnalysis.Testing;

namespace Patternify.Abstraction.Tests.Extensions;

internal static class ReferenceAssembliesExtension
{
    internal static ReferenceAssemblies GetPackages(this ReferenceAssemblies referenceAssemblies)
        => referenceAssemblies.WithPackages(
            [
                ..new[]
                {
                    new PackageIdentity("Microsoft.NETCore.App.Ref", "8.0.0"),
                }
            ]);

}