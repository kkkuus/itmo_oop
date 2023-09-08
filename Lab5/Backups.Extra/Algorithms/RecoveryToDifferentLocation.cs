using System.IO.Compression;
using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.Tools;
namespace Backups.Extra;

public class RecoveryToDifferentLocation : IRecovery
{
    public RecoveryToDifferentLocation(BackupTaskExtra backupTask, RestorePoint restorePoint, string newPath)
    {
        if (restorePoint == null)
            throw new BackupsExtraException("Incorrect value of restore point!");
        if (backupTask == null)
            throw new BackupsExtraException("Incorrect value of backup task!");
        if (string.IsNullOrWhiteSpace(newPath))
            throw new BackupsExtraException("Incorrect value of new path!");
        foreach (var storage in restorePoint.Storages)
        {
            foreach (var backupObject in storage.Objects)
            {
                string pathFrom = Path.Combine(backupTask.BackupTask.Path, restorePoint.Name, storage.StorageName, backupObject.ObjectName);
                if (Directory.Exists(pathFrom))
                {
                    string zipObjectPath = Path.Combine(newPath,  storage.StorageName + ".zip");
                    ZipFile.CreateFromDirectory(pathFrom, zipObjectPath);
                    ZipFile.ExtractToDirectory(zipObjectPath, Path.Combine(newPath, backupObject.ObjectName), true);
                    File.Delete(zipObjectPath);
                }
                else
                {
                    File.Copy(pathFrom, Path.Combine(newPath, backupObject.ObjectName), true);
                }
            }
        }
    }
}