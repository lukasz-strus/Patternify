namespace Patternify.Examples.NullObject;

public class ConsoleLog : ILog
{
    public int Id { get; set; }

    public void Info(string message)
    {
        Console.WriteLine(message);
    }

    public string Login(string username, string password)
    {
        Console.WriteLine($"Login... {username}");
        return password;
    }

    public void Warn(string message)
    {
        Console.WriteLine($"WARNING: {message}");
    }
}