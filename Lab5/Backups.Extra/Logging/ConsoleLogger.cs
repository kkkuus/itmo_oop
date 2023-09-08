using Backups.Extra.Tools;

namespace Backups.Extra.Logging;

public class ConsoleLogger : ILogger
{
    public void PrintLog(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new BackupsExtraException("Incorrect message in log!");
        Console.WriteLine(message, "\n");
    }
}