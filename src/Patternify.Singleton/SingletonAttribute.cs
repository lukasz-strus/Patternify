namespace Patternify.Singleton;

/// <summary>
/// Represents an attribute that marks a class as a Singleton.
/// 
/// This attribute should be applied to a class that is intended to implement 
/// the Singleton design pattern. The marked class must be a partial class, 
/// allowing the source generator to inject the necessary Singleton 
/// implementation. By using this attribute, you ensure that the class has 
/// only one instance and provides a global point of access to that instance.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute : Attribute;