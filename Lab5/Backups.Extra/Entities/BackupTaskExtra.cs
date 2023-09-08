using System.Runtime.CompilerServices;
using System.Text.Json;
using Backups.Entities;
using Backups.Extra.Logging;
using Backups.Extra.Tools;

namespace Backups.Extra.Entities;

public class BackupTaskExtra
{
    private List<RestorePoint> _pointsForDelete = new List<RestorePoint>();
    private ISelection? _selection;
    private Merge _merge = new Merge();
    private IRecovery? _recovery;
    public BackupTaskExtra(BackupTask backupTask, ILogger logger)
    {
        if (backupTask == null)
            throw new BackupsExtraException("Incorrect value of backup task!");
        if (logger == null)
            throw new BackupsExtraException("Incorrect value of logger!");
        BackupTask = backupTask;
        Logger = logger;
    }

    public BackupTask BackupTask { get; }
    public ILogger Logger { get; }

    public IReadOnlyList<RestorePoint> PointsForDelete => _pointsForDelete;
    public void AddBackupObject(BackupObject backupObject)
    {
        Logger.PrintLog("Try to add backup object");
        BackupTask.AddBackupObject(backupObject);
        Logger.PrintLog("Backup object successfully added");
    }

    public void RemoveBackupObject(BackupObject backupObject)
    {
        Logger.PrintLog("Try to remove backup object");
        BackupTask.RemoveBackupObject(backupObject);
        Logger.PrintLog("Backup object successfully removed");
    }

    public void CreateRestorePoint(List<BackupObject> backupObjects)
    {
        Logger.PrintLog("Try to create restore point");
        BackupTask.CreateRestorePoint(backupObjects.ToList());
        Logger.PrintLog("Restore point successfully created");
    }

    public void RemoveRestorePoint(RestorePoint restorePoint)
    {
        Logger.PrintLog("Try to remove restore point");
        BackupTask.RemoveRestorePoint(restorePoint);
        BackupTask.Repository.Remove(restorePoint, BackupTask.Path);
        Logger.PrintLog("Restore point successfully removed");
    }

    public void SetSelection(ISelection selection)
    {
        if (selection == null)
            throw new BackupsExtraException("Incorrect value of selection!");
        _selection = selection;
        _pointsForDelete = selection.Selection(BackupTask.Backup.RestorePoints.ToList()).ToList();
    }

    public void Merge()
    {
        MergeProcess mergeProcess = new MergeProcess(this, _pointsForDelete);
    }

    public void MergeTwoPoints(RestorePoint newPoint, RestorePoint oldPoint)
    {
        if (newPoint == null || oldPoint == null)
            throw new BackupsExtraException("Incorrect value of restore points!");
        _merge.MergePoint(this, newPoint, oldPoint);
    }

    public void RecoveryRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint == null)
            throw new BackupsExtraException("Incorrect value of restore point!");
        _recovery = new RecoveryToOriginalLocation(this, restorePoint);
    }

    public void RecoveryRestorePoint(RestorePoint restorePoint, string path)
    {
        if (restorePoint == null)
            throw new BackupsExtraException("Incorrect value of restore point!");
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupsExtraException("Incorrect value of path!");
        _recovery = new RecoveryToDifferentLocation(this, restorePoint, path);
    }
}