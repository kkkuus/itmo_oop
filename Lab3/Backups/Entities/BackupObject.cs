using System.Text.Json.Serialization;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Entities;

public class BackupObject
{
    public BackupObject(string name, string path)
    {
        if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(name))
            throw new BackupException("Invalid path or value of name");
        ObjectPath = path;
        ObjectName = name;
    }

    [JsonProperty("name")]
    public string ObjectName { get; }

    [JsonProperty("path")]
    public string ObjectPath { get; }
}