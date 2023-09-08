using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.Tools;

namespace Backups.Extra;

public class RemoveProcess
{
    private const int MinimumСountOfRestorePoints = 0;
    public RemoveProcess(BackupTaskExtra backupTask, IReadOnlyList<RestorePoint> restorePoints)
    {
        if (backupTask == null)
            throw new BackupsExtraException("Incorrect value of backup task!");
        if (restorePoints.Count <= MinimumСountOfRestorePoints)
            throw new BackupsExtraException("Incorrect count of restore points!");
        foreach (var restorePoint in restorePoints)
            backupTask.RemoveRestorePoint(restorePoint);
    }
}