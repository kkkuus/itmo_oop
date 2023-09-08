using Isu.Entities;
using Isu.Models;
using Isu.Services;
using Isu.Tools;
using Xunit;

namespace Isu.Test;

public class IsuServiceTest
{
    private IsuService _isuService = new IsuService();
    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        GroupName groupName = new GroupName("M32041");
        Group group = _isuService.AddGroup(groupName);
        Student student = _isuService.AddStudent(group, "Kusaikina Elizaveta");
        Assert.Contains(student, group.Students);
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        GroupName groupName = new GroupName("M32041");
        Group group = _isuService.AddGroup(groupName);
        for (int i = 0; i < group.MaxCountOfStudents; ++i)
        {
            Student student = _isuService.AddStudent(group, "Ivanov Ivan");
        }

        Assert.Throws<IsuException>(() =>
        {
            Student newStudent = _isuService.AddStudent(group, "Ivanov Ivan");
        });
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        Assert.Throws<IsuException>(() =>
        {
            GroupName groupName = new GroupName("M320416");
            Group group = _isuService.AddGroup(groupName);
        });
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        GroupName oldName = new GroupName("M32041");
        GroupName newName = new GroupName("P32011");
        Group oldGroup = _isuService.AddGroup(oldName);
        Group newGroup = _isuService.AddGroup(newName);
        Student student = _isuService.AddStudent(oldGroup, "Kusaikina Elizaveta");
        _isuService.ChangeStudentGroup(student, newGroup);
        Assert.Equal(student.IsuGroup, newGroup);
    }
}