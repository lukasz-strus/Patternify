using Patternify.Singleton;

namespace Patternify.Examples.Singleton.Configurations;

[Singleton]
internal partial class InternalConfiguration
{
    public string Property { get; set; } = string.Empty;
}