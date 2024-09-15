using Patternify.Singleton;

namespace Patternify.Examples.Singleton.Configurations;

[Singleton] 
public class PublicConfiguration
{
    public string Property { get; set; } = string.Empty;
}