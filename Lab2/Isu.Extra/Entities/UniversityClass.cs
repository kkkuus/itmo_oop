using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class UniversityClass
{
    private const int MinimumNumberOfClass = 1;
    private const int MaximumNumberOfClass = 8;

    private Dictionary<int, string> _stringTime = new Dictionary<int, string>()
    {
        { 1, "8:20-9:50" },
        { 2, "10:00-11:30" },
        { 3, "11:40-13:10" },
        { 4, "13:30-15:00" },
        { 5, "15:20-16:50" },
        { 6, "17:00-18:30" },
        { 7, "18:40-20:10" },
        { 8, "20:20-21:50" },
    };
    public UniversityClass(int numberOfClass)
    {
        if (numberOfClass < MinimumNumberOfClass || numberOfClass > MaximumNumberOfClass)
            throw new IsuExtraException("Invalid number of class");
        NumberOfClass = numberOfClass;
    }

    public UniversityClass(int numberOfClass, string teacher, int auditorium, string dayOfWeek, string parityOfWeek)
    {
        if (numberOfClass < MinimumNumberOfClass || numberOfClass > MaximumNumberOfClass
           || string.IsNullOrWhiteSpace(teacher) || auditorium < MinimumNumberOfClass
           || string.IsNullOrWhiteSpace(dayOfWeek) || string.IsNullOrWhiteSpace(parityOfWeek))
            throw new IsuExtraException("Invalid data");
        NumberOfClass = numberOfClass;
        Teacher = teacher;
        Auditorium = auditorium;
        DayOfWeek = dayOfWeek;
        ParityOfWeek = parityOfWeek;
        foreach (var stringClass in _stringTime)
        {
            if (stringClass.Key == numberOfClass)
                Time = stringClass.Value;
        }
    }

    public int NumberOfClass { get; }
    public string Teacher { get; } = null!;
    public int Auditorium { get; }
    public string Time { get; } = null!;
    public string DayOfWeek { get; } = null!;
    public string ParityOfWeek { get; } = null!;
}