using Patternify.Examples.Singleton.Test;

var conf = Configuration1.GetInstance();
var conf2 = Configuration2.GetInstance();
conf.Test = "Hello World";
Console.WriteLine(conf.Test);

