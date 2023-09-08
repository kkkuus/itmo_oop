using Backups.Services;
using Backups.Tools;

namespace Backups.Entities;

public class AbstractRepository : IRepository
{
    private List<BackupObject> _backupObjects = new List<BackupObject>();
    private List<BackupTask> _backups = new List<BackupTask>();
    public void AddObject(BackupObject backupObject)
    {
        if (backupObject == null)
            throw new BackupException("No files for add");
        _backupObjects.Add(backupObject);
    }

    public void RemoveObject(BackupObject backupObject)
    {
        if (backupObject == null)
            throw new BackupException("No files for delete");
        _backupObjects.Remove(backupObject);
    }

    public void AddBackup(BackupTask backup)
    {
        if (backup == null)
            throw new BackupException("No backup");
        _backups.Add(backup);
    }

    public void RemoveBackup(BackupTask backup)
    {
        if (backup == null)
            throw new BackupException("No backup");
        _backups.Remove(backup);
    }

    public IReadOnlyList<BackupTask> Backups() => _backups;

    public void Archive(RestorePoint newRestorePoint, int id, string path) { }
    public void Remove(RestorePoint restorePoint, string path) { }
}