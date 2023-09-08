using System.IO;
using System.IO.Compression;
using System.Xml;
using Backups.Services;
using Backups.Tools;

namespace Backups.Entities;

public class FileSystemRepository : IRepository
{
    private List<BackupObject> _backupObjects = new List<BackupObject>();
    private List<BackupTask> _backups = new List<BackupTask>();

    public FileSystemRepository(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupException("Invalid value of path");
        RepositoryPath = path;
        Directory.CreateDirectory(RepositoryPath);
    }

    public string RepositoryPath { get; }
    public void AddObject(BackupObject backupObject)
    {
        if (backupObject == null)
            throw new BackupException("No files");
        _backupObjects.Add(backupObject);
    }

    public void RemoveObject(BackupObject backupObject)
    {
        if (backupObject == null)
            throw new BackupException("No files");
        _backupObjects.Remove(backupObject);
    }

    public void AddBackup(BackupTask backup)
    {
        if (backup == null)
            throw new BackupException("Invalid data");
        _backups.Add(backup);
        Directory.CreateDirectory(backup.Path);
    }

    public void RemoveBackup(BackupTask backup)
    {
        if (backup == null)
            throw new BackupException("Invalid data");
        _backups.Remove(backup);
    }

    public IReadOnlyList<BackupTask> Backups() => _backups;

    public void Archive(RestorePoint newRestorePoint, int id, string path)
    {
        if (newRestorePoint == null)
            throw new BackupException("No files");
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupException("Invalid value of path");
        string restorePointPath = Path.Combine(path, newRestorePoint.Name);
        Directory.CreateDirectory(restorePointPath);
        foreach (var storage in newRestorePoint.Storages)
        {
            string storagePath = Path.Combine(restorePointPath, storage.StorageName);
            Directory.CreateDirectory(storagePath);
            foreach (var backupObject in storage.Objects)
            {
                if (Directory.Exists(backupObject.ObjectPath))
                {
                    string zipStoragePath = Path.Combine(storagePath, backupObject.ObjectName + ".zip");
                    ZipFile.CreateFromDirectory(backupObject.ObjectPath, zipStoragePath);
                    ZipFile.ExtractToDirectory(zipStoragePath, Path.Combine(storagePath, backupObject.ObjectName));
                    File.Delete(zipStoragePath);
                }
                else
                {
                    File.Copy(backupObject.ObjectPath, Path.Combine(storagePath, backupObject.ObjectName));
                }
            }
        }
    }

    public void Remove(RestorePoint restorePoint, string path)
    {
        if (restorePoint == null)
            throw new BackupException("Incorrect value of restore point!");
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupException("Incorrect value of backup task!");
        string pointPath = Path.Combine(path, restorePoint.Name);
        Directory.Delete(pointPath, true);
    }
}