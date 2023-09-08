using Isu.Entities;
using Isu.Models;
using Isu.Tools;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private static int _id = 100000;
    private List<Group> _groups = new List<Group>();
    public Group AddGroup(GroupName name)
    {
        var newGroup = new Group(name);
        if (_groups.Contains(newGroup))
        {
            throw new IsuException("This group has already been created");
        }

        _groups.Add(newGroup);
        return newGroup;
    }

    public Student AddStudent(Group group, string name)
    {
        if (group == null || name == null)
        {
            throw new IsuException("The name of the group or student was entered incorrectly");
        }

        var newStudent = new Student(name, group, _id);
        ++_id;

        group.AddStudent(newStudent);
        newStudent.ChangeGroup(group);
        return newStudent;
    }

    public Student GetStudent(int id)
    {
        var selectedStudent = _groups
            .SelectMany(students => students.Students)
            .SingleOrDefault(@student => @student.IsuNumber == id);
        if (selectedStudent == null)
        {
            throw new IsuException("Student not found");
        }

        return selectedStudent;
    }

    public Student? FindStudent(int id)
    {
         var selectedStudent = _groups
            .SelectMany(students => students.Students)
            .SingleOrDefault(@student => @student.IsuNumber == id);
         return selectedStudent;
    }

    public IReadOnlyList<Student>? FindStudents(GroupName groupName)
    {
        foreach (Group group in _groups)
        {
            if (group.GroupName == groupName)
                return (List<Student>)group.Students;
        }

        return null;
    }

    public IReadOnlyList<Student> FindStudents(CourseNumber courseNumber)
    {
        var studentsInCourse = new List<Student>();
        foreach (Group group in _groups)
        {
            if (group.GroupCourse == courseNumber)
            {
                foreach (Student student in group.Students)
                    studentsInCourse.Add(student);
            }
        }

        return studentsInCourse;
    }

    public Group? FindGroup(GroupName groupName)
    {
        foreach (Group group in _groups)
        {
            if (group.GroupName == groupName)
                return group;
        }

        return null;
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        return _groups.Where(group => group.GroupCourse == courseNumber).ToList();
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        if (newGroup.CountOfStudents >= newGroup.MaxCountOfStudents)
        {
            throw new IsuException("Maximum number of people in a group");
        }

        if (student.IsuGroup == newGroup)
        {
            throw new IsuException("The student is already in this group");
        }

        student.IsuGroup.RemoveStudent(student);
        newGroup.AddStudent(student);
        student.ChangeGroup(newGroup);
    }
}