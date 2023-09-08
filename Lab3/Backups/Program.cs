using Backups.Algorithms;
using Backups.Entities;
using Backups.Services;
using Backups.Tools;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IAlgorithm algo = new SingleStorage();
            IRepository rep = new FileSystemRepository(@"C:\lab3\Repository");
            BackupObject file1 = new BackupObject("file1", @"C:\lab3");
            BackupObject file2 = new BackupObject("file2.txt", @"C:\lab3");
            List<BackupObject> objects = new List<BackupObject>() { file1, file2 };
            BackupTask backupTask = new BackupTask("Task1", rep, algo, @"C:\lab3\Repository");
            rep.AddBackup(backupTask);
            backupTask.AddBackupObject(file1);
            backupTask.AddBackupObject(file2);
            backupTask.CreateRestorePoint(backupTask.BackupObjects.ToList());
        }
    }
}