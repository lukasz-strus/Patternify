namespace Patternify.NullObject;

/// <summary>
/// Represents an attribute that marks an interface as a Null Object.
/// 
/// This attribute should be applied to an interface that is intended to 
/// implement the Null Object design pattern. The marked interface will 
/// indicate that a concrete implementation should provide a default 
/// instance that can be used safely in place of a null reference. 
/// By using this attribute, you help to promote cleaner code and 
/// reduce the need for null checks throughout your application.
/// </summary>
[AttributeUsage(AttributeTargets.Interface)]
public class NullObjectAttribute : Attribute;