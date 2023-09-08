using Backups.Entities;

namespace Backups.Services;

public interface IAlgorithm
{
    object CreateStorage(List<BackupObject> objects, int number, string path);
}