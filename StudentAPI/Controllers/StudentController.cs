using Microsoft.AspNetCore.Mvc;
using StudentAPI.Interface;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController(IStudentRepository studentRepository): Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Student>))]
        public IActionResult GetStudents()
        {
            var students = studentRepository.GetStudents();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(students);
        }

        [HttpGet("students/{rollno}")]
        [ProducesResponseType(200, Type = typeof(Student))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetStudent(int rollno)
        {
            var student = studentRepository.GetStudent(rollno);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(student);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(Student))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetStudentByName(string name)
        {
            var student = studentRepository.GetStudent(name);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateStudent([FromBody] Student student)
        {
            if (student == null)
                return BadRequest(ModelState);

            var studentDetail = studentRepository
                .GetStudents()
                .FirstOrDefault(c => c.Name.Trim().ToUpper() == student.Name.Trim().ToUpper());

            if (studentDetail != null)
            {
                ModelState.AddModelError("", $"Category {student.Name} already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!studentRepository.CreateStudent(student))
            {
                ModelState.AddModelError("", $"Something went wrong saving the student " + $"{student.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpPut("{rollNo}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStudent(int rollNo, [FromBody] Student studentUpdate)
        {
            if (studentUpdate == null) return BadRequest(ModelState);

            if (rollNo != studentUpdate.RollNo) return BadRequest(ModelState);

            if (!studentRepository.StudentExists(rollNo)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!studentRepository.UpdateStudent(studentUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating {studentUpdate.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{rollNo}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult DeleteStudent(int rollNo)
        {
            if (!studentRepository.StudentExists(rollNo)) return NotFound();

            var studentDelete = studentRepository.GetStudent(rollNo);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!studentRepository.DeleteStudent(studentDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {studentDelete.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
