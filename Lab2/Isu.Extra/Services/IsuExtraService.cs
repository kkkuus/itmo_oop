using Isu.Extra.Entities;
using Isu.Extra.Tools;

namespace Isu.Extra.Services;

public class IsuExtraService : IIsuExtraService
{
    private List<InfoCourseOgnp> _ognpCourses = new List<InfoCourseOgnp>();
    public IReadOnlyList<InfoCourseOgnp> OgnpCourses => _ognpCourses;
    public InfoCourseOgnp AddOgnpCourse(InfoCourseOgnp newOgnpCourse)
    {
        if (newOgnpCourse == null)
            throw new IsuExtraException("Invalid data");
        if (_ognpCourses.Contains(newOgnpCourse))
            throw new IsuExtraException("This course has already been created");
        _ognpCourses.Add(newOgnpCourse);
        return newOgnpCourse;
    }

    public OgnpGroup AddStudentOnOgnpCourse(StudentExtra student, InfoCourseOgnp ognpCourse)
    {
        if (student == null || ognpCourse == null)
            throw new IsuExtraException("Invalid data");
        OgnpGroup? findedGroup;
        findedGroup = student.CheckingForPlacesOnCourse(ognpCourse);
        if (findedGroup == null)
        {
            throw new IsuExtraException("There are no places on this course");
        }
        else
        {
            findedGroup.AddStudent(student);
            student.AddOgnpCourse(ognpCourse);
            student.AddClass(findedGroup.Lesson);
        }

        return findedGroup;
    }

    public OgnpGroup RemoveStudentFromOgnpGroup(StudentExtra student, OgnpGroup group)
    {
        if (student == null || group == null)
            throw new IsuExtraException("Invalid data");
        if (!group.Students.Contains(student))
            throw new IsuExtraException("There is no such student in this course");
        group.RemoveStudent(student);
        student.RemoveOgnpCourse(group.CourseOgnp);
        student.RemoveClass(group.Lesson);
        return group;
    }

    public IReadOnlyList<OgnpStream> GetStreamsOnCourse(InfoCourseOgnp ognpCourse)
    {
        if (ognpCourse == null)
            throw new IsuExtraException("Invalid data");
        return ognpCourse.Streams;
    }

    public IReadOnlyList<StudentExtra> GetStudentsOnOgnpGroup(OgnpGroup group)
    {
        if (group == null)
            throw new IsuExtraException("Invalid data");
        return group.Students;
    }

    public IReadOnlyList<StudentExtra> GetStudentsWhoHaveNotSignedUpForOgnp(GroupExtra group)
    {
        List<StudentExtra> unsignedStudents = new List<StudentExtra>();
        foreach (var student in group.Students)
        {
            if (student.StudentOgnp.Count < 2)
                unsignedStudents.Add(student);
        }

        return unsignedStudents;
    }
}