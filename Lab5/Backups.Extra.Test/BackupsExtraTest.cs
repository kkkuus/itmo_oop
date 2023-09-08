using Backups.Algorithms;
using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.Logging;
using Backups.Services;
using Xunit;
namespace Backups.Extra.Test;

public class BackupsExtraTest
{
    [Fact]
    public void MergeOnInMemoryRepository()
    {
        IAlgorithm algo = new SplitStorage();
        IRepository rep = new AbstractRepository();
        BackupObject file1 = new BackupObject("1.txt", "C:\\Test");
        BackupObject file2 = new BackupObject("2.txt", "C:\\Test");
        List<BackupObject> objects = new List<BackupObject>() { file1, file2 };
        BackupTask backupTask = new BackupTask("Task1", rep, algo, "C:\\Backup");
        rep.AddBackup(backupTask);
        ILogger logger = new ConsoleLogger();
        BackupTaskExtra backupTaskExtra = new BackupTaskExtra(backupTask, logger);
        backupTaskExtra.AddBackupObject(file1);
        backupTaskExtra.AddBackupObject(file2);
        backupTaskExtra.CreateRestorePoint(backupTask.BackupObjects.ToList());
        Assert.Contains(file1, backupTaskExtra.BackupTask.BackupObjects);
        backupTaskExtra.RemoveBackupObject(file2);
        backupTaskExtra.CreateRestorePoint(backupTask.BackupObjects.ToList());
        Assert.Equal(2, rep.Backups()[0].Backup.RestorePoints.Count);
        Assert.Equal(2, rep.Backups()[0].Backup.RestorePoints[0].Storages.Count);
        Assert.Single(rep.Backups()[0].Backup.RestorePoints[1].Storages);
        RestorePoint newRestorePoint = backupTaskExtra.BackupTask.Backup.RestorePoints[1];
        RestorePoint oldRestorePoint = backupTaskExtra.BackupTask.Backup.RestorePoints[0];
        new Merge().MergePoint(backupTaskExtra, newRestorePoint, oldRestorePoint);
        Assert.Single(backupTaskExtra.BackupTask.Backup.RestorePoints);
        Assert.Equal(2, backupTaskExtra.BackupTask.Backup.RestorePoints[0].Storages.Count);
    }

    [Fact]
    public void RemoveWithSelectionByNumber()
    {
        IAlgorithm algo = new SplitStorage();
        IRepository rep = new AbstractRepository();
        BackupObject file1 = new BackupObject("1.txt", "C:\\Test");
        BackupObject file2 = new BackupObject("2.txt", "C:\\Test");
        List<BackupObject> objects = new List<BackupObject>() { file1, file2 };
        BackupTask backupTask = new BackupTask("Task1", rep, algo, "C:\\Backup");
        rep.AddBackup(backupTask);
        ILogger logger = new ConsoleLogger();
        BackupTaskExtra backupTaskExtra = new BackupTaskExtra(backupTask, logger);
        backupTaskExtra.AddBackupObject(file1);
        backupTaskExtra.AddBackupObject(file2);
        backupTaskExtra.CreateRestorePoint(backupTask.BackupObjects.ToList());
        backupTaskExtra.RemoveBackupObject(file2);
        backupTaskExtra.CreateRestorePoint(backupTask.BackupObjects.ToList());
        ISelection selection = new SelectionByNumber(1);
        List<RestorePoint> selectionsPoints = selection.Selection(backupTaskExtra.BackupTask.Backup.RestorePoints.ToList()).ToList();
        Assert.Single(selectionsPoints);
        Assert.Contains(backupTaskExtra.BackupTask.Backup.RestorePoints[0], selectionsPoints);
        Assert.Equal(2, backupTaskExtra.BackupTask.Backup.RestorePoints.Count);
        RemoveProcess removeProcess = new RemoveProcess(backupTaskExtra, selectionsPoints);
        Assert.Single(backupTaskExtra.BackupTask.Backup.RestorePoints);
    }

    [Fact(Skip = "Because it's use file system")]
    public void CheckConfiguration()
    {
        IAlgorithm algo = new SplitStorage();
        IRepository rep = new AbstractRepository();
        BackupObject file1 = new BackupObject("1.txt", "C:\\Test");
        BackupObject file2 = new BackupObject("2.txt", "C:\\Test");
        List<BackupObject> objects = new List<BackupObject>() { file1, file2 };
        BackupTask backupTask = new BackupTask("Task1", rep, algo, "C:\\Backup");
        rep.AddBackup(backupTask);
        ILogger logger = new ConsoleLogger();
        BackupTaskExtra backupTaskExtra = new BackupTaskExtra(backupTask, logger);
        backupTaskExtra.AddBackupObject(file1);
        backupTaskExtra.AddBackupObject(file2);
        backupTaskExtra.CreateRestorePoint(backupTask.BackupObjects.ToList());
        Serializer conf = new Serializer();
        conf.SaveBackupTask(backupTaskExtra, @"C:\lab3\conf.txt");
        BackupTaskExtra newBackupTaskExtra = conf.DownloadBackupTask(@"C:\lab3\conf.txt");
        conf.SaveBackupTask(newBackupTaskExtra, @"C:\lab3\conf2.txt");
        Assert.Equal(backupTaskExtra.BackupTask.Name, newBackupTaskExtra.BackupTask.Name);
    }

    [Fact(Skip = "Because it's FileSystem Repository")]
    public void MergeOnFileSystemRepository()
    {
        IAlgorithm algo = new SplitStorage();
        IRepository rep = new FileSystemRepository(@"C:\lab3\NewRepository");
        BackupObject file1 = new BackupObject("file1", @"C:\lab3\file1");
        BackupObject file2 = new BackupObject("file2.txt", @"C:\lab3\file2.txt");
        List<BackupObject> objects = new List<BackupObject>() { file1, file2 };
        BackupTask backupTask = new BackupTask("Task1", rep, algo, @"C:\lab3\NewRepository\Task1");
        rep.AddBackup(backupTask);
        ILogger logger = new FileLogger(@"C:\lab3\logger.txt", false);
        BackupTaskExtra backupTaskExtra = new BackupTaskExtra(backupTask, logger);
        backupTaskExtra.AddBackupObject(file1);
        backupTaskExtra.AddBackupObject(file2);
        backupTaskExtra.CreateRestorePoint(backupTaskExtra.BackupTask.BackupObjects.ToList());
        Assert.Contains(file1, backupTaskExtra.BackupTask.Backup.RestorePoints[0].Storages[0].Objects);
        Assert.Contains(file1, rep.Backups()[0].Backup.RestorePoints[0].Storages[0].Objects);
        Thread.Sleep(5000);
        backupTaskExtra.RemoveBackupObject(file1);
        backupTaskExtra.CreateRestorePoint(backupTask.BackupObjects.ToList());
        RestorePoint newRestorePoint = backupTaskExtra.BackupTask.Backup.RestorePoints[1];
        RestorePoint oldRestorePoint = backupTaskExtra.BackupTask.Backup.RestorePoints[0];
        Thread.Sleep(5000);
        new Merge().MergePoint(backupTaskExtra, newRestorePoint, oldRestorePoint);
    }

    [Fact(Skip = "Because it's FileSystem Repository")]
    public void RemoveProcessInFileSystemRepository()
    {
        IAlgorithm algo = new SplitStorage();
        IRepository rep = new FileSystemRepository(@"C:\lab3\NewRepository2");
        BackupObject file1 = new BackupObject("file1", @"C:\lab3\file1");
        BackupObject file2 = new BackupObject("file2.txt", @"C:\lab3\file2.txt");
        List<BackupObject> objects = new List<BackupObject>() { file1, file2 };
        BackupTask backupTask = new BackupTask("Task1", rep, algo, @"C:\lab3\NewRepository2\Task1");
        rep.AddBackup(backupTask);
        ILogger logger = new FileLogger(@"C:\lab3\logger2.txt", false);
        BackupTaskExtra backupTaskExtra = new BackupTaskExtra(backupTask, logger);
        backupTaskExtra.AddBackupObject(file1);
        backupTaskExtra.CreateRestorePoint(backupTaskExtra.BackupTask.BackupObjects.ToList());
        Thread.Sleep(5000);
        backupTaskExtra.RemoveBackupObject(file1);
        backupTaskExtra.AddBackupObject(file2);
        backupTaskExtra.CreateRestorePoint(backupTaskExtra.BackupTask.BackupObjects.ToList());
        Thread.Sleep(5000);
        backupTaskExtra.CreateRestorePoint(backupTaskExtra.BackupTask.BackupObjects.ToList());
        DateTime limitTime = new DateTime(2022, 12, 9, 4, 32, 30);
        ISelection selection = new SelectionByDateOfCreating(limitTime);
        RemoveProcess removeProcess = new RemoveProcess(backupTaskExtra, selection.Selection(backupTaskExtra.BackupTask.Backup.RestorePoints.ToList()));
    }

    [Fact(Skip = "Because it's FileSystem Repository")]
    public void MergeInsteadOfRemoveInFileSystemRepository()
    {
        IAlgorithm algo = new SplitStorage();
        IRepository rep = new FileSystemRepository(@"C:\lab3\NewRepository2");
        BackupObject file1 = new BackupObject("file1", @"C:\lab3\file1");
        BackupObject file2 = new BackupObject("file2.txt", @"C:\lab3\file2.txt");
        List<BackupObject> objects = new List<BackupObject>() { file1, file2 };
        BackupTask backupTask = new BackupTask("Task1", rep, algo, @"C:\lab3\NewRepository2\Task1");
        rep.AddBackup(backupTask);
        ILogger logger = new FileLogger(@"C:\lab3\logger2.txt", false);
        BackupTaskExtra backupTaskExtra = new BackupTaskExtra(backupTask, logger);
        backupTaskExtra.AddBackupObject(file1);
        backupTaskExtra.CreateRestorePoint(backupTaskExtra.BackupTask.BackupObjects.ToList());
        Thread.Sleep(5000);
        backupTaskExtra.RemoveBackupObject(file1);
        backupTaskExtra.AddBackupObject(file2);
        backupTaskExtra.CreateRestorePoint(backupTaskExtra.BackupTask.BackupObjects.ToList());
        Thread.Sleep(5000);
        backupTaskExtra.CreateRestorePoint(backupTaskExtra.BackupTask.BackupObjects.ToList());
        Thread.Sleep(5000);
        Assert.Equal(3, backupTaskExtra.BackupTask.Backup.RestorePoints.Count);
        ISelection selection = new SelectionByNumber(1);
        IReadOnlyList<RestorePoint> restorePointsToDelete =
            selection.Selection(backupTaskExtra.BackupTask.Backup.RestorePoints.ToList());
        Assert.Equal(2, restorePointsToDelete.Count);
        MergeProcess mergeProcess = new MergeProcess(backupTaskExtra, selection.Selection(backupTaskExtra.BackupTask.Backup.RestorePoints.ToList()));
    }

    [Fact(Skip = "Because it's FileSystem Repository")]
    public void RecoveryRestorePointInOriginalAndDifferentLocation()
    {
        IAlgorithm algo = new SplitStorage();
        IRepository rep = new FileSystemRepository(@"C:\lab3\NewRepository3");

        BackupObject file1 = new BackupObject("file1", @"C:\lab3\file1");
        BackupObject file2 = new BackupObject("file2.txt", @"C:\lab3\file2.txt");
        List<BackupObject> objects = new List<BackupObject>() { file1, file2 };
        BackupTask backupTask = new BackupTask("Task1", rep, algo, @"C:\lab3\NewRepository3\Task1");
        rep.AddBackup(backupTask);
        ILogger logger = new FileLogger(@"C:\lab3\logger2.txt", false);
        BackupTaskExtra backupTaskExtra = new BackupTaskExtra(backupTask, logger);
        backupTaskExtra.AddBackupObject(file1);
        backupTaskExtra.AddBackupObject(file2);
        backupTaskExtra.CreateRestorePoint(backupTaskExtra.BackupTask.BackupObjects.ToList());
        RestorePoint restorePoint = backupTaskExtra.BackupTask.Backup.RestorePoints[0];
        Thread.Sleep(5000);
        backupTaskExtra.RecoveryRestorePoint(restorePoint, @"C:\lab3\DifferentLocation");
        backupTaskExtra.RecoveryRestorePoint(restorePoint);
    }
}