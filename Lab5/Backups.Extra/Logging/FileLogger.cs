using System.IO;
using System.Text;
using Backups.Extra.Tools;

namespace Backups.Extra.Logging;

public class FileLogger : ILogger
{
    private string _path;

    public FileLogger(string path, bool dateFlag)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupsExtraException("Incorrect value of logger file path!");
        _path = path;
        DateFlag = dateFlag;
    }

    public string Path => _path;
    public bool DateFlag { get; }
    public void PrintLog(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new BackupsExtraException("Incorrect message in log!");
        if (DateFlag)
            File.AppendAllText(Path, DateTime.Now + "   " + message + "\n");
        else
            File.AppendAllText(Path, message + "\n");
    }
}