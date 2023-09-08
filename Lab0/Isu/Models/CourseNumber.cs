using Isu.Entities;
using Isu.Tools;

namespace Isu.Models;

public class CourseNumber
{
    private int _minimumCourseNumber = 1;
    private int _maximumCourseNumber = 6;
    public CourseNumber(int number)
    {
        if (number < _minimumCourseNumber || number > _maximumCourseNumber)
        {
            throw new IsuException("Incorrect course number entered");
        }

        Number = number;
    }

    public int Number { get; }
}