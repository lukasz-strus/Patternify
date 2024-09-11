using Patternify.Singleton;

namespace Patternify.Examples.Singleton.Test
{
    [Singleton]
    public sealed partial class Configuration1
    {
        public string Test { get; set; }
    }

    [Singleton]
    public sealed partial class Configuration2
    {

    }
}
