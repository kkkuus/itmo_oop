using Backups.Services;
using Backups.Tools;

namespace Backups.Entities;

public class BackupTask
{
    private List<BackupObject> _backupObjects = new List<BackupObject>();
    private Backup _backup;
    private int _numberOfBackup = 0;
    public BackupTask(string name, IRepository repository, IAlgorithm algorithm, string path)
    {
        if (repository == null || algorithm == null)
            throw new BackupException("Invalid data");
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(path))
            throw new BackupException("Invalid value of name or path");
        Name = name;
        Repository = repository;
        Algorithm = algorithm;
        Path = path;
        _backup = new Backup(path);
    }

    public string Name { get; }
    public IRepository Repository { get; }
    public IAlgorithm Algorithm { get; }
    public Backup Backup => _backup;
    public string Path { get; }
    public IReadOnlyList<BackupObject> BackupObjects => _backupObjects;
    public void AddBackupObject(BackupObject backupObject)
    {
        if (backupObject == null)
            throw new BackupException("Invalid data");
        _backupObjects.Add(backupObject);
    }

    public void RemoveBackupObject(BackupObject backupObject)
    {
        if (backupObject == null)
            throw new BackupException("Invalid data");
        _backupObjects.Remove(backupObject);
    }

    public void CreateRestorePoint(List<BackupObject> backupObjects)
    {
        if (backupObjects.Count == 0)
            throw new BackupException("Invalid data");
        ++_numberOfBackup;
        object storages = Algorithm.CreateStorage(backupObjects, _numberOfBackup, Path);
        RestorePoint newRestorePoint = new RestorePoint((List<Storage>)storages);
        _backup.AddRestorePoint(newRestorePoint);
        Repository.Archive(newRestorePoint, _numberOfBackup, Path);
    }

    public void RemoveRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint == null)
            throw new BackupException("Invalid data");
        _backup.RemoveRestorePoint(restorePoint);
    }
}