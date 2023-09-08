using Backups.Entities;
using Backups.Services;
using Backups.Tools;
namespace Backups.Algorithms;

public class SplitStorage : IAlgorithm
{
    public object CreateStorage(List<BackupObject> objects, int number, string path)
    {
        if (objects == null)
            throw new BackupException("No files for backup");
        List<Storage> storages = new List<Storage>();
        foreach (var obj in objects)
        {
            Storage storage = new Storage(obj.ObjectName, number);
            storage.AddObject(obj);
            storages.Add(storage);
            ++number;
        }

        return storages;
    }
}