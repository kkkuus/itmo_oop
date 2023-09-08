using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Services;
using Isu.Extra.Tools;
using Isu.Models;
using Isu.Services;
using Xunit;
namespace Isu.Extra.Test;

public class IsuExtraServiceTest
{
    private IsuExtraService _isuExtraService = new IsuExtraService();

    [Fact]
    public void AddOgnpCourse()
    {
        UniversityClass classOgnpStream = new UniversityClass(1, "Ivanov", 331, "Monday", "even");
        UniversityClass classOgnpGroup = new UniversityClass(2, "Ivanova", 327, "Monday", "even");
        Megafaculty megafaculty = new Megafaculty("KTU", new List<char> { 'K', 'P' });
        InfoCourseOgnp courseOgnp = _isuExtraService.AddOgnpCourse(new InfoCourseOgnp("KIB", megafaculty));
        Assert.Contains(courseOgnp, _isuExtraService.OgnpCourses);
    }

    [Fact]
    public void AddStreamOgnpAndGroupOgnp()
    {
        UniversityClass classOgnpStream = new UniversityClass(1, "Ivanov", 331, "Monday", "even");
        UniversityClass classOgnpGroup = new UniversityClass(2, "Ivanova", 327, "Monday", "even");
        Megafaculty megafaculty = new Megafaculty("KTU", new List<char> { 'K', 'P' });
        InfoCourseOgnp courseOgnp = _isuExtraService.AddOgnpCourse(new InfoCourseOgnp("KIB", megafaculty));
        courseOgnp.AddStream(classOgnpStream);
        OgnpStream ognpStream = courseOgnp.GetStream(1);
        ognpStream.AddGroup(classOgnpGroup);
        OgnpGroup ognpGroup = ognpStream.GetGroup(1);
        Assert.Contains(ognpStream, courseOgnp.Streams);
        Assert.Contains(ognpGroup, ognpStream.Groups);
    }

    [Fact]
    public void AddStudentToOgnpCourse()
    {
        UniversityClass classOgnpStream = new UniversityClass(1, "Ivanov", 331, "Monday", "even");
        UniversityClass classOgnpGroup = new UniversityClass(2, "Ivanova", 327, "Monday", "even");
        Megafaculty megafaculty = new Megafaculty("KTU", new List<char> { 'K', 'P' });
        InfoCourseOgnp courseOgnp = _isuExtraService.AddOgnpCourse(new InfoCourseOgnp("KIB", megafaculty));
        courseOgnp.AddStream(classOgnpStream);
        OgnpStream ognpStream = courseOgnp.GetStream(1);
        ognpStream.AddGroup(classOgnpGroup);
        OgnpGroup ognpGroup = ognpStream.GetGroup(1);
        UniversityClass class1 = new UniversityClass(4, "Galkina", 123, "Wednesday", "odd");
        UniversityClass class2 = new UniversityClass(2, "Galkin", 333, "Friday", "even");
        GroupName groupName = new GroupName("M32041");
        Group group = new Group(groupName);
        GroupExtra newGroup = new GroupExtra(group, new List<UniversityClass> { class1, class2 });
        StudentExtra student = new StudentExtra("Kusaikina", 334792, newGroup);
        newGroup.AddStudent(student);
        _isuExtraService.AddStudentOnOgnpCourse(student, courseOgnp);
        Assert.Contains(courseOgnp, student.StudentOgnp);
        Assert.Contains(student, ognpGroup.Students);
        UniversityClass class3 = new UniversityClass(3, "Galkina", 333, "Friday", "odd");
        GroupName groupName1 = new GroupName("P32041");
        Group group1 = new Group(groupName1);
        GroupExtra newGroup1 = new GroupExtra(group1, new List<UniversityClass> { class3 });
        StudentExtra student1 = new StudentExtra("Kusaikina", 444444, newGroup1);
        newGroup1.AddStudent(student1);
        Assert.Throws<IsuExtraException>(() =>
        {
            _isuExtraService.AddStudentOnOgnpCourse(student1, courseOgnp);
        });
    }

    [Fact]
    public void RemovingStudentEntryForACourse()
    {
        UniversityClass classOgnpStream = new UniversityClass(1, "Ivanov", 331, "Monday", "even");
        UniversityClass classOgnpGroup = new UniversityClass(2, "Ivanova", 327, "Monday", "even");
        Megafaculty megafaculty = new Megafaculty("KTU", new List<char> { 'K', 'P' });
        InfoCourseOgnp courseOgnp = _isuExtraService.AddOgnpCourse(new InfoCourseOgnp("KIB", megafaculty));
        courseOgnp.AddStream(classOgnpStream);
        OgnpStream ognpStream = courseOgnp.GetStream(1);
        ognpStream.AddGroup(classOgnpGroup);
        OgnpGroup ognpGroup = ognpStream.GetGroup(1);
        UniversityClass class1 = new UniversityClass(4, "Galkina", 123, "Wednesday", "odd");
        UniversityClass class2 = new UniversityClass(2, "Galkin", 333, "Friday", "even");
        GroupName groupName = new GroupName("M32041");
        Group group = new Group(groupName);
        GroupExtra newGroup = new GroupExtra(group, new List<UniversityClass> { class1, class2 });
        StudentExtra student = new StudentExtra("Kusaikina", 334792, newGroup);
        newGroup.AddStudent(student);
        _isuExtraService.AddStudentOnOgnpCourse(student, courseOgnp);
        _isuExtraService.RemoveStudentFromOgnpGroup(student, ognpGroup);
        List<InfoCourseOgnp> courses = new List<InfoCourseOgnp>();
        List<StudentExtra> students = new List<StudentExtra>();
        Assert.Equal(students, ognpGroup.Students);
    }

    [Fact]
    public void ListOfStudentsWhoDidNotEnroll()
    {
        UniversityClass classOgnpStream = new UniversityClass(1, "Ivanov", 331, "Monday", "even");
        UniversityClass classOgnpGroup = new UniversityClass(2, "Ivanova", 327, "Monday", "even");
        Megafaculty megafaculty = new Megafaculty("KTU", new List<char> { 'K', 'P' });
        InfoCourseOgnp courseOgnp = _isuExtraService.AddOgnpCourse(new InfoCourseOgnp("KIB", megafaculty));
        courseOgnp.AddStream(classOgnpStream);
        OgnpStream ognpStream = courseOgnp.GetStream(1);
        ognpStream.AddGroup(classOgnpGroup);
        OgnpGroup ognpGroup = ognpStream.GetGroup(1);
        UniversityClass class1 = new UniversityClass(4, "Galkina", 123, "Wednesday", "odd");
        UniversityClass class2 = new UniversityClass(2, "Galkin", 333, "Friday", "even");
        GroupName groupName = new GroupName("M32041");
        Group group = new Group(groupName);
        GroupExtra newGroup = new GroupExtra(group, new List<UniversityClass> { class1, class2 });
        StudentExtra student = new StudentExtra("Kusaikina", 334792, newGroup);
        newGroup.AddStudent(student);
        _isuExtraService.AddStudentOnOgnpCourse(student, courseOgnp);
        List<StudentExtra> students1 = new List<StudentExtra> { student };
        List<StudentExtra> students2 = (List<StudentExtra>)_isuExtraService.GetStudentsWhoHaveNotSignedUpForOgnp(newGroup);
        Assert.Equal(students1, students2);
    }
}