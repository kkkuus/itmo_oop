using Backups.Entities;
using Backups.Extra.Tools;

namespace Backups.Extra;

public class GibridSelection : ISelection
{
    private const int MinimumCountOfConditions = 1;
    private const int MaximumCountOfConditions = 2;
    public GibridSelection(int countOfConditions, int limitNumber, DateTime limitData)
    {
        if (countOfConditions < MinimumCountOfConditions || countOfConditions > MaximumCountOfConditions)
            throw new BackupsExtraException("Incorrect value of conditions!");
        if (limitNumber < MaximumCountOfConditions)
            throw new BackupsExtraException("Incorrect value of limit for count!");
        Conditions = countOfConditions;
        LimitNumber = limitNumber;
        LimitData = limitData;
    }

    public int Conditions { get; }
    public int LimitNumber { get; }
    public DateTime LimitData { get; }

    public IReadOnlyList<RestorePoint> Selection(List<RestorePoint> restorePoints)
    {
        IReadOnlyList<RestorePoint> temp = new SelectionByNumber(LimitNumber).Selection(restorePoints);
        temp = new SelectionByDateOfCreating(LimitData).Selection(temp.ToList());
        return temp;
    }
}