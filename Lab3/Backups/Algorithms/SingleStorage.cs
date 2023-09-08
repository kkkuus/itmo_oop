using System.Collections.Generic;
using System.IO.Compression;
using Backups.Entities;
using Backups.Services;
using Backups.Tools;

namespace Backups.Algorithms;

public class SingleStorage : IAlgorithm
{
    public object CreateStorage(List<BackupObject> objects, int number, string path)
    {
        if (objects == null)
            throw new BackupException("No files for backup");
        Storage storage = new Storage("Storage", number);
        foreach (var obj in objects)
            storage.AddObject(obj);
        List<Storage> storages = new List<Storage>() { storage };
        return storages;
    }
}