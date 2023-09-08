using Backups.Algorithms;
using Backups.Entities;
using Backups.Services;
using Backups.Tools;
using Xunit;
namespace Backups.Test;

public class BackupsTest
{
    [Fact]
    public void InMemoryRepository()
    {
        IAlgorithm algo = new SplitStorage();
        IRepository rep = new AbstractRepository();
        BackupObject file1 = new BackupObject("1.txt", "C:\\Test");
        BackupObject file2 = new BackupObject("2.txt", "C:\\Test");
        List<BackupObject> objects = new List<BackupObject>() { file1, file2 };
        BackupTask backupTask = new BackupTask("Task1", rep, algo, "C:\\Backup");
        rep.AddBackup(backupTask);
        backupTask.AddBackupObject(file1);
        backupTask.AddBackupObject(file2);
        backupTask.CreateRestorePoint(backupTask.BackupObjects.ToList());
        backupTask.RemoveBackupObject(file2);
        backupTask.CreateRestorePoint(backupTask.BackupObjects.ToList());
        Assert.Equal(2, rep.Backups()[0].Backup.RestorePoints.Count);
        Assert.Equal(2, rep.Backups()[0].Backup.RestorePoints[0].Storages.Count);
        Assert.Equal(1, rep.Backups()[0].Backup.RestorePoints[1].Storages.Count);
    }

    [Fact(Skip = "Because it's FileSystem Repository")]
    public void FileSystemRepository()
    {
        IAlgorithm algo = new SingleStorage();
        IRepository rep = new FileSystemRepository(@"C:\lab3\Repository");
        BackupObject file1 = new BackupObject("file1", @"C:\lab3\file1");
        BackupObject file2 = new BackupObject("file2.txt", @"C:\lab3\file2.txt");
        List<BackupObject> objects = new List<BackupObject>() { file1, file2 };
        BackupTask backupTask = new BackupTask("Task1", rep, algo, @"C:\lab3\Repository\Task1");
        rep.AddBackup(backupTask);
        backupTask.AddBackupObject(file1);
        backupTask.AddBackupObject(file2);
        backupTask.CreateRestorePoint(backupTask.BackupObjects.ToList());
        Assert.Contains(file1, backupTask.Backup.RestorePoints[0].Storages[0].Objects);
        Assert.Contains(file1, rep.Backups()[0].Backup.RestorePoints[0].Storages[0].Objects);
        backupTask.CreateRestorePoint(backupTask.BackupObjects.ToList());
    }
}