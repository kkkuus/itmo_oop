using Isu.Entities;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class GroupExtra
{
    private const int MaximumCountOfStudents = 25;
    private List<UniversityClass> _schedule = new List<UniversityClass>();
    private List<StudentExtra> _students = new List<StudentExtra>();
    public GroupExtra(Group group, List<UniversityClass> schedule)
    {
        if (group == null)
            throw new IsuExtraException("Invalid data");
        Group = group;
        _schedule = schedule;
    }

    public Group Group { get; }
    public IReadOnlyList<StudentExtra> Students => _students;
    public IReadOnlyList<UniversityClass> Schedule => _schedule;

    public void AddClass(UniversityClass newClass)
    {
        if (newClass == null)
            throw new IsuExtraException("Invalid data");
        if (_schedule.Any(curClass => curClass.NumberOfClass == newClass.NumberOfClass && curClass.DayOfWeek == newClass.DayOfWeek
                && curClass.ParityOfWeek == newClass.ParityOfWeek))
        {
            throw new IsuExtraException("There are already lessons at this time");
        }

        _schedule.Add(newClass);
    }

    public void RemoveClass(UniversityClass oldClass)
    {
        if (oldClass == null)
            throw new IsuExtraException("Inalid data");
        _schedule.Remove(oldClass);
    }

    public UniversityClass? FindLesson(int numberClass, string dayOfWeek, string parityOfWeek)
    {
        return _schedule.FirstOrDefault(lesson => lesson.NumberOfClass == numberClass
                                                  && lesson.DayOfWeek == dayOfWeek && lesson.ParityOfWeek == parityOfWeek);
    }

    public void AddStudent(StudentExtra newStudent)
    {
        if (_students.Count >= MaximumCountOfStudents)
            throw new IsuExtraException("Maximum number of people in a group");
        _students.Add(newStudent);
    }
}