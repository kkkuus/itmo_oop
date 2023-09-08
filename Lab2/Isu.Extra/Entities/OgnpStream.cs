using Isu.Entities;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class OgnpStream
{
    private const int MinimumGroupNumber = 1;
    private const int MaximumGroupCount = 5;
    private int _groupId = 1;
    private List<OgnpGroup> _groups = new List<OgnpGroup>();
    public OgnpStream(int streamNumber, UniversityClass lessonTime, InfoCourseOgnp courseOgnp)
    {
        if (lessonTime == null)
            throw new IsuExtraException("Invalid data");
        StreamNumber = streamNumber;
        Lesson = lessonTime;
        CourseOgnp = courseOgnp;
    }

    public UniversityClass Lesson { get; }
    public int StreamNumber { get; }
    public InfoCourseOgnp CourseOgnp { get; }
    public List<OgnpGroup> Groups => _groups;

    public void AddGroup(UniversityClass lessonTime)
    {
        if (_groups.Count > MaximumGroupCount)
            throw new IsuExtraException("You can't add groups anymore");
        _groups.Add(new OgnpGroup(_groupId, lessonTime, CourseOgnp));
        _groupId++;
    }

    public OgnpGroup GetGroup(int groupNumber)
    {
        if (groupNumber < MinimumGroupNumber || groupNumber > MaximumGroupCount)
            throw new IsuExtraException("Invalid value of stream number");
        foreach (var group in _groups)
        {
            if (group.GroupNumber == groupNumber)
                return group;
        }

        throw new IsuExtraException("There aren't this stream in this ognp");
    }
}