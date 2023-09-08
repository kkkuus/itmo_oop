using Isu.Extra.Entities;

namespace Isu.Extra.Services;

public interface IIsuExtraService
{
    InfoCourseOgnp AddOgnpCourse(InfoCourseOgnp newOgnpCourse);
    OgnpGroup AddStudentOnOgnpCourse(StudentExtra student, InfoCourseOgnp ognpCourse);
    OgnpGroup RemoveStudentFromOgnpGroup(StudentExtra student, OgnpGroup group);
    IReadOnlyList<OgnpStream> GetStreamsOnCourse(InfoCourseOgnp ognpCourse);
    IReadOnlyList<StudentExtra> GetStudentsOnOgnpGroup(OgnpGroup group);
    IReadOnlyList<StudentExtra> GetStudentsWhoHaveNotSignedUpForOgnp(GroupExtra group);
}