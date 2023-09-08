using Backups.Entities;
namespace Backups.Extra;

public class SelectionByDateOfCreating : ISelection
{
    private const int MinimumValueOfCount = 0;

    public SelectionByDateOfCreating(DateTime limitData)
    {
        LimitDate = limitData;
    }

    public DateTime LimitDate { get; }
    public IReadOnlyList<RestorePoint> Selection(List<RestorePoint> restorePoints)
    {
        var selectedRestorePoints =
            restorePoints.Where(restorePoint => DateTime.ParseExact(restorePoint.CreationDate, "hh-mm-ss-dd-MM-yyyy", null) < LimitDate);
        List<RestorePoint> restorePointsToDelete = selectedRestorePoints.ToList();
        if (restorePointsToDelete.Count() == restorePoints.Count)
            restorePointsToDelete.Remove(restorePointsToDelete.Last());
        return restorePointsToDelete.ToList();
    }
}