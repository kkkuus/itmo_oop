using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Entities;

public class Storage
{
    private const int MinimumAllowedStorageNumber = 0;
    private List<BackupObject> _objects = new List<BackupObject>();
    public Storage(string name, int number)
    {
        if (number < MinimumAllowedStorageNumber)
            throw new BackupException("Invalid number");
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupException("Invalid value of name or path");
        StorageName = name + string.Format("{0:_#}", number);
    }

    [JsonProperty("name")]
    public string StorageName { get; }

    public IReadOnlyList<BackupObject> Objects => _objects;
    public void AddObject(BackupObject backupObject)
    {
        if (backupObject == null)
            throw new BackupException("No files for backup");
        _objects.Add(backupObject);
    }
}