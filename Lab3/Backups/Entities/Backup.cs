using Backups.Tools;

namespace Backups.Entities;

public class Backup
{
    private List<RestorePoint> _restorePoints = new List<RestorePoint>();

    public Backup(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupException("Invalid value of path");
        Path = path;
    }

    public string Path { get; }
    public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

    public void AddRestorePoint(RestorePoint newRestorePoint)
    {
        if (newRestorePoint == null)
            throw new BackupException("Invalid value of RestorePoint");
        _restorePoints.Add(newRestorePoint);
    }

    public void RemoveRestorePoint(RestorePoint oldRestorePoint)
    {
        if (oldRestorePoint == null)
            throw new BackupException("Invalid value of RestorePoint");
        _restorePoints.Remove(oldRestorePoint);
    }
}