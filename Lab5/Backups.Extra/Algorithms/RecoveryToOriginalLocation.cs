using System.IO.Compression;
using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.Tools;

namespace Backups.Extra;

public class RecoveryToOriginalLocation : IRecovery
{
    public RecoveryToOriginalLocation(BackupTaskExtra backupTask, RestorePoint restorePoint)
    {
        if (restorePoint == null)
            throw new BackupsExtraException("Incorrect value of restore point!");
        if (backupTask == null)
            throw new BackupsExtraException("Incorrect value of backup task!");
        foreach (var storage in restorePoint.Storages)
        {
            foreach (var backupObject in storage.Objects)
            {
                string pathTo = Path.Combine(backupObject.ObjectPath, backupObject.ObjectName);
                string pathFrom = Path.Combine(backupTask.BackupTask.Path, restorePoint.Name, storage.StorageName, backupObject.ObjectName);
                if (Directory.Exists(pathFrom))
                {
                    string zipObjectPath = Path.Combine(backupObject.ObjectPath + ".zip");
                    ZipFile.CreateFromDirectory(pathFrom, zipObjectPath);
                    ZipFile.ExtractToDirectory(zipObjectPath, Path.Combine(backupObject.ObjectPath), true);
                    File.Delete(zipObjectPath);
                }
                else
                {
                    File.Copy(pathFrom, Path.Combine(backupObject.ObjectPath), true);
                }
            }
        }
    }
}