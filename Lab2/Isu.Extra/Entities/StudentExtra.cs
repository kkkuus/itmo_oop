using Isu.Entities;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class StudentExtra : Student
{
    private const int _minimumCountOgnp = 2;
    private List<InfoCourseOgnp> _ognpLessons = new List<InfoCourseOgnp>();
    private List<UniversityClass> _schedule = new List<UniversityClass>();
    public StudentExtra(string name, int isuNumber, GroupExtra groupExtra)
        : base(name, groupExtra.Group, isuNumber)
    {
        if (groupExtra == null)
            throw new IsuExtraException("Invalid data");
        StudentGroupExtra = groupExtra;
        _schedule = (List<UniversityClass>)groupExtra.Schedule;
    }

    public GroupExtra StudentGroupExtra { get; }
    public IReadOnlyList<UniversityClass> Schedule => _schedule;
    public IReadOnlyList<InfoCourseOgnp> StudentOgnp => _ognpLessons;
    public OgnpGroup? CheckingForPlacesOnCourse(InfoCourseOgnp desiredOgnp)
    {
        if (!desiredOgnp.CheckingFacultyForPossibilityOfRecording(this.IsuGroup.GroupName.Faculty))
            throw new IsuExtraException("You can't sign up for this course");
        foreach (var stream in desiredOgnp.Streams)
        {
            if (!this.Schedule.Contains(stream.Lesson))
            {
                foreach (var group in stream.Groups)
                {
                    if (!this.Schedule.Contains(group.Lesson))
                        return group;
                }
            }
        }

        return null;
    }

    public void AddOgnpCourse(InfoCourseOgnp ognpCourse)
    {
        if (ognpCourse == null)
            throw new IsuExtraException("Ingalid data");
        _ognpLessons.Add(ognpCourse);
    }

    public void RemoveOgnpCourse(InfoCourseOgnp ognpCourse)
    {
        if (ognpCourse == null)
            throw new IsuExtraException("Invalid data");
        var selectedCourse = _ognpLessons.FirstOrDefault(course => course == ognpCourse);
        if (selectedCourse != null)
            _ognpLessons.Remove(selectedCourse);
    }

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
            throw new IsuExtraException("Invalid data");
        _schedule.Remove(oldClass);
    }
}