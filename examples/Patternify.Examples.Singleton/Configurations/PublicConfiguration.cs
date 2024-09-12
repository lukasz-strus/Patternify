using Patternify.Singleton;

namespace Patternify.Examples.Singleton.Configurations;

[Singleton]
public sealed partial class PublicConfiguration
{
    public string Property { get; set; } = string.Empty;
}