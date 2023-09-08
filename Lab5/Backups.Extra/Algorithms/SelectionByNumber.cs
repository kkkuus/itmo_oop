using Backups.Entities;
using Backups.Extra.Tools;
using Backups.Tools;

namespace Backups.Extra;

public class SelectionByNumber : ISelection
{
    private const int MinimumValueOfCount = 0;
    public SelectionByNumber(int limitNumber)
    {
        if (limitNumber < MinimumValueOfCount)
            throw new BackupsExtraException("Incorrect value of limit count!");
        LimitNumber = limitNumber;
    }

    public int LimitNumber { get; }

    public IReadOnlyList<RestorePoint> Selection(List<RestorePoint> restorePoints)
    {
        List<RestorePoint> restorePointsToDelete = new List<RestorePoint>();
        for (int i = 0; i < restorePoints.Count - LimitNumber; ++i)
            restorePointsToDelete.Add(restorePoints[i]);
        if (restorePointsToDelete.Count == restorePoints.Count)
            restorePointsToDelete.Remove(restorePointsToDelete.Last());
        return restorePointsToDelete;
    }
}