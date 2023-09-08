using Backups.Tools;
using static System.DateTime;

namespace Backups.Entities;

public class RestorePoint
{
    private const int MinimumAllowableCountOfStorages = 1;
    private List<BackupObject> _backupObjects = new List<BackupObject>();
    private List<Storage> _storages = new List<Storage>();
    public RestorePoint(List<Storage> storages)
    {
        if (storages.Count < MinimumAllowableCountOfStorages)
            throw new BackupException("No files to backup");
        CreationDate = DateTime.Now.ToString("hh-mm-ss-dd-MM-yyyy");
        Name = "RestorePoint" + CreationDate;
        _storages = storages;
    }

    public IReadOnlyList<BackupObject> Objects => _backupObjects;
    public IReadOnlyList<Storage> Storages => _storages;
    public string Name { get; }
    public string CreationDate { get; }

    public void AddStorage(Storage newStorage)
    {
        if (newStorage == null)
            throw new BackupException("Invalid value of Storage");
        _storages.Add(newStorage);
    }

    public void RemoveStorage(Storage oldStorage)
    {
        if (oldStorage == null)
            throw new BackupException("Invalid value of Storage");
        _storages.Remove(oldStorage);
    }
}