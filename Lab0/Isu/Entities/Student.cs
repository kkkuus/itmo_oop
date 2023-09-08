using Isu.Tools;

namespace Isu.Entities;

public class Student
{
    // private static int isuNumber = 100000;
    public Student(string name, Group group, int isuNumber)
    {
        if (string.IsNullOrEmpty(name) || group == null)
        {
            throw new IsuException("The name is set incorrectly");
        }

        Name = name;
        IsuNumber = isuNumber;

        // ++isuNumber;
        IsuGroup = group;
    }

    public string Name { get; }
    public int IsuNumber { get; }
    public Group IsuGroup { get; private set; }

    public void ChangeGroup(Group newGroup)
    {
        this.IsuGroup = newGroup;
    }
}