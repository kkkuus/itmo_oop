using Backups.Entities;
using Backups.Tools;

namespace Backups.Services;

public interface IRepository
{
    void AddObject(BackupObject backupObject);
    void RemoveObject(BackupObject backupObject);
    void AddBackup(BackupTask backup);
    void RemoveBackup(BackupTask backup);
    IReadOnlyList<BackupTask> Backups();
    void Archive(RestorePoint newRestorePoint, int id, string path);
    void Remove(RestorePoint restorePoint, string path);
}