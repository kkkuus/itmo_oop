using Backups.Entities;

namespace Backups.Extra;

public interface ISelection
{
    IReadOnlyList<RestorePoint> Selection(List<RestorePoint> restorePoints);
}