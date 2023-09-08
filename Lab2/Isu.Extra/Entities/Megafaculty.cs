using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class Megafaculty
{
    private List<char> _faculties = new List<char>();
    public Megafaculty(string name, List<char> faculties)
    {
        if (string.IsNullOrWhiteSpace(name) || faculties == null)
            throw new IsuExtraException("Invalid data");
        Name = name;
        _faculties = faculties;
    }

    public string Name { get; }
    public IReadOnlyList<char> Faculties => _faculties;

    public void AddFaculty(char newFaculty)
    {
        if (char.IsWhiteSpace(newFaculty))
            throw new IsuExtraException("Invalid data");
        if (!_faculties.Contains(newFaculty))
            _faculties.Add(newFaculty);
    }

    public void RemoveFaculty(char oldFaculty)
    {
        if (char.IsWhiteSpace(oldFaculty))
            throw new IsuExtraException("Invalid data");
        if (_faculties.Contains(oldFaculty))
            _faculties.Remove(oldFaculty);
    }
}