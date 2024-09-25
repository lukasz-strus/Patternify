using Patternify.NullObject;

namespace Patternify.Examples.NullObject;

[NullObject]
public interface ILog
{
    public int Id { get; }
    public string Login(string username, string password);
    public void Info(string message);
    public void Warn(string message);
}