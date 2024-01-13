using StudentAPI.Models;

namespace StudentAPI.Interface
{
    public interface IStudentRepository
    {
        ICollection<Student> GetStudents();
        Student? GetStudent(int rollno);
        Student? GetStudent(string name);
        bool StudentExists(int rollno);

        bool CreateStudent(Student student);
        bool UpdateStudent(Student student);
        bool DeleteStudent(Student student);
        bool Save();
    }
}
