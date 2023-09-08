using System.Runtime.CompilerServices;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class InfoCourseOgnp
{
    private const int MaximumCountOfStreams = 3;
    private const int MinimumNumberOfStream = 1;
    private List<char> _faculties = new List<char>();
    private List<OgnpStream> _streams = new List<OgnpStream>();
    private int _streamId = 1;
    public InfoCourseOgnp(string name, Megafaculty megafaculty)
    {
        if (string.IsNullOrWhiteSpace(name) || megafaculty == null)
            throw new IsuExtraException("Invalid data");
        Name = name;
        _faculties = (List<char>)megafaculty.Faculties;
        CourseMegafaculty = megafaculty;
    }

    public string Name { get; }
    public IReadOnlyList<char> FacultiesOfCourse => _faculties;
    public IReadOnlyList<OgnpStream> Streams => _streams;
    public Megafaculty CourseMegafaculty { get; }
    public bool CheckingFacultyForPossibilityOfRecording(char studentFaculty)
    {
        if (_faculties.Contains(studentFaculty))
            return false;
        else
            return true;
    }

    public void AddStream(UniversityClass newStreamTime)
    {
        if (_streamId > MaximumCountOfStreams)
            throw new IsuExtraException("You can't add streams anymore");
        OgnpStream newStream = new OgnpStream(_streamId, newStreamTime, new InfoCourseOgnp(Name, CourseMegafaculty));
        _streams.Add(newStream);
        _streamId++;
    }

    public OgnpStream GetStream(int streamNumber)
    {
        if (streamNumber < MinimumNumberOfStream || streamNumber > MaximumCountOfStreams)
            throw new IsuExtraException("Invalid value of stream number");
        var selectedStream = _streams.FirstOrDefault(stream => stream.StreamNumber == streamNumber);
        if (selectedStream == null)
            throw new IsuExtraException("There aren't this stream in this ognp");
        return selectedStream;
    }
}