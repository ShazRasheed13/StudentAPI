using StudentAPI.Data;
using StudentAPI.Interface;
using StudentAPI.Models;

namespace StudentAPI.Repository
{
    public class StudentRepository(DataContext context):IStudentRepository
    {
        public ICollection<Student> GetStudents() => 
            context.Students.OrderBy(c => c.RollNo).ToList();

        public Student? GetStudent(int rollno) => 
            context.Students.FirstOrDefault(s => s.RollNo == rollno);

        public Student? GetStudent(string name) =>
            context.Students.FirstOrDefault(s => s.Name == name);

        public bool StudentExists(int rollno) => 
            context.Students.Any(s=>s.RollNo == rollno);

        public bool CreateStudent(Student student)
        {
            context.Add(student);
            return Save();
        }

        public bool UpdateStudent(Student student)
        {
            context.Update(student);
            return Save();
        }

        public bool DeleteStudent(Student student)
        {
            context.Remove(student);
            return Save();
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
