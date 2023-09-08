using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class OgnpGroup
{
    private const int MaximumCountStudentInGroup = 25;
    private List<StudentExtra> _students = new List<StudentExtra>();

    public OgnpGroup(int groupNumber, UniversityClass lessonTime, InfoCourseOgnp courseOgnp)
    {
        if (lessonTime == null)
            throw new IsuExtraException("Invalid data");
        GroupNumber = groupNumber;
        Lesson = lessonTime;
        CourseOgnp = courseOgnp;
    }

    public UniversityClass Lesson { get; }
    public int GroupNumber { get; }
    public InfoCourseOgnp CourseOgnp { get; }
    public List<StudentExtra> Students => _students;

    public void AddStudent(StudentExtra newStudent)
    {
        if (_students.Count > MaximumCountStudentInGroup)
            throw new IsuExtraException("You can't add students in this group");
        _students.Add(newStudent);
    }

    public void RemoveStudent(StudentExtra oldStudent)
    {
        if (oldStudent == null)
            throw new IsuExtraException("Invalid data");
        _students.Remove(oldStudent);
    }
}