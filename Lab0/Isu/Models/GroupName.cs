using Isu.Tools;

namespace Isu.Models;

public class GroupName
{
    private const int _minimumNameLength = 5;
    private const int _maximumNameLength = 6;
    public GroupName(string name)
    {
        if (name.Length < _minimumNameLength || name.Length > _maximumNameLength)
        {
            throw new IsuException("Invalid name of group");
        }

        Name = name;

        Faculty = name[0];

        switch (name[1])
        {
            case '3':
                StageOfEducation = "bachelor";
                break;
            case '4':
                StageOfEducation = "magistracy";
                break;
            case '5':
                StageOfEducation = "specialty";
                break;
            default:
                throw new IsuException("Name of group is set incorrectly");
        }

        // char charCourse = name[2];
        Course = name[2] - '0';

        // Course = Convert.ToInt32(name[2]);
        Number = Convert.ToInt32(name[3] + name[4]);
        if (name.Length == _maximumNameLength)
        {
            Specialization = Convert.ToInt32(name[5]);
        }
    }

    public string Name { get; }
    public char Faculty { get; }
    public string? StageOfEducation { get; }
    public int Course { get; }
    public int Number { get; }
    public int Specialization { get; }
}