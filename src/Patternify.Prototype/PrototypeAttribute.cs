namespace Patternify.Prototype;

/// <summary>
/// Represents an attribute that marks a class as a Prototype.
/// 
/// This attribute should be applied to a class that is intended to implement 
/// the Prototype design pattern. The marked class must be a partial class, 
/// allowing the source generator to inject the necessary cloning methods. 
/// The generator will automatically create methods for shallow and deep 
/// copying of objects, making it easier to clone instances of the class.
/// 
/// The generated class will include:
/// - `ShallowCopy()`: Creates a shallow copy of the object.
/// - `DeepCopy()`: Creates a deep copy of the object, ensuring that complex types are cloned correctly.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class PrototypeAttribute : Attribute;