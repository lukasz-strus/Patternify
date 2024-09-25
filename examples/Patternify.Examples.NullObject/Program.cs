using Patternify.Examples.NullObject;

Console.WriteLine("Hello, World!");

var log = new ConsoleLog();
var logNullObject = new LogNullObject();

log.Warn("Test"); // Write to console "WARNING: Test"
logNullObject.Warn("Test"); // Do nothing