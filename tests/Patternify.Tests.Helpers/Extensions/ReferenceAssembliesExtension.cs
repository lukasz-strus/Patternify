using Microsoft.CodeAnalysis.Testing;

namespace Patternify.Tests.Helpers.Extensions;

internal static class ReferenceAssembliesExtension
{
    internal static ReferenceAssemblies GetPackages(this ReferenceAssemblies referenceAssemblies)
        => referenceAssemblies.WithPackages(
        [
            ..new[]
            {
                new PackageIdentity("Microsoft.NETCore.App.Ref", "8.0.0"),
                new PackageIdentity("xunit.extensibility.core", "2.4.2")
            }
        ]);
}