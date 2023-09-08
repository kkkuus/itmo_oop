using Backups.Extra.Entities;
using Backups.Extra.Tools;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Backups.Extra;

public class Serializer
{
    private JsonSerializerSettings? _jsonSerializerSettings;

    public Serializer()
    {
        _jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
        };
    }

    public void SaveBackupTask(BackupTaskExtra backupTask, string configurationFile)
    {
        if (backupTask == null)
            throw new BackupsExtraException("Incorrect value of backup task!");
        string json = JsonConvert.SerializeObject(backupTask, _jsonSerializerSettings);
        File.WriteAllText(configurationFile, json);
    }

    public BackupTaskExtra DownloadBackupTask(string configurationFile)
    {
        if (string.IsNullOrWhiteSpace(configurationFile))
            throw new BackupsExtraException("Incorrect value of configuration file path!");
        return JsonConvert.DeserializeObject<BackupTaskExtra>(File.ReadAllText(configurationFile), _jsonSerializerSettings) !;
    }
}