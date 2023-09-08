using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.Tools;

namespace Backups.Extra;

public class MergeProcess
{
    private const int MinimumCountOfPointsToMerge = 2;
    public MergeProcess(BackupTaskExtra backupTask, IReadOnlyList<RestorePoint> restorePoints)
    {
        if (backupTask == null)
            throw new BackupsExtraException("Incorrect value of backup task!");
        if (restorePoints.Count < MinimumCountOfPointsToMerge)
            throw new BackupsExtraException("Incorrect value of count of restore points!");
        Merge merge = new Merge();
        RestorePoint mergeRestorePoint = merge.MergePoint(backupTask, restorePoints[1], restorePoints[0]);
        for (int i = MinimumCountOfPointsToMerge; i < restorePoints.Count; ++i)
            mergeRestorePoint = merge.MergePoint(backupTask, restorePoints[i], mergeRestorePoint);

        merge.MergePoint(backupTask, backupTask.BackupTask.Backup.RestorePoints[0], mergeRestorePoint);
    }
}