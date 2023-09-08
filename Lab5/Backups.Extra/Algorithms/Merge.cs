using Backups.Algorithms;
using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.Logging;
using Backups.Extra.Tools;

namespace Backups.Extra;

public class Merge
{
    public RestorePoint MergePoint(BackupTaskExtra backupTask, RestorePoint newRestorePoint, RestorePoint oldRestorePoint)
    {
        if (backupTask == null)
            throw new BackupsExtraException("Incorrect value of backup task!");
        backupTask.Logger.PrintLog("Trying to merge restore points");
        if (newRestorePoint == null || oldRestorePoint == null)
            throw new BackupsExtraException("Incorrect value of restore points!");
        if (backupTask.BackupTask.Algorithm is SingleStorage)
        {
            backupTask.RemoveRestorePoint(oldRestorePoint);
        }
        else
        {
            List<BackupObject> objectsInNewPoint = new List<BackupObject>();
            List<BackupObject> objectsInOldPoint = new List<BackupObject>();
            List<BackupObject> backupObjects = new List<BackupObject>();
            foreach (var storage in oldRestorePoint.Storages)
                objectsInOldPoint.Add(storage.Objects[0]);
            foreach (var storage in newRestorePoint.Storages)
                objectsInNewPoint.Add(storage.Objects[0]);

            foreach (var backupObject in objectsInOldPoint)
            {
                if (!objectsInNewPoint.Contains(backupObject))
                    backupObjects.Add(backupObject);
            }

            foreach (var backupObject in objectsInNewPoint)
                backupObjects.Add(backupObject);
            backupTask.Logger.PrintLog($"Creating new restore point with {backupObjects.Count} backup objects");
            backupTask.Logger.PrintLog("Try to remove old restore points");
            backupTask.RemoveRestorePoint(oldRestorePoint);
            backupTask.RemoveRestorePoint(newRestorePoint);
            backupTask.CreateRestorePoint(backupObjects);
            backupTask.Logger.PrintLog("Old restore point successfully removed");
        }

        backupTask.Logger.PrintLog("Merge completed successfully");
        return backupTask.BackupTask.Backup.RestorePoints.Last();
    }
}