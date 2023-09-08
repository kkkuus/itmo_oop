using Isu.Models;
using Isu.Tools;

namespace Isu.Entities;

public class Group
{
    private const int _maxStudents = 25;
    private List<Student> _students = new List<Student>();

    public Group(GroupName groupName)
    {
        if (groupName == null)
        {
            throw new IsuException("The group is set incorrectly");
        }

        GroupName = groupName;
        CourseNumber course = new CourseNumber(GroupName.Course);
        GroupCourse = course;
    }

    public GroupName GroupName { get; }
    public string Name => GroupName.Name;

    public CourseNumber GroupCourse { get; }

    public IReadOnlyList<Student> Students => _students;

    public int CountOfStudents => _students.Count;

    public int MaxCountOfStudents => _maxStudents;

    public void AddStudent(Student newStudent)
    {
        if (_students.Count >= _maxStudents)
        {
            throw new IsuException("Group is full");
        }

        _students.Add(newStudent);
    }

    public void RemoveStudent(Student oldStudent)
    {
        if (oldStudent == null)
        {
            throw new IsuException("The student is entered incorrectly");
        }

        _students.Remove(oldStudent);
    }
}