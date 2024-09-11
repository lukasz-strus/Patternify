// See https://aka.ms/new-console-template for more information

using Patternify.Singleton;

var conf = Configuration1.GetInstance();
var conf2 = Configuration2.GetInstance();
conf.Test = "Hello World";
Console.WriteLine(conf.Test);

[Singleton]
public sealed partial class Configuration1
{
    public string Test { get; set; }
}

[Singleton]
public sealed partial class Configuration2
{

}