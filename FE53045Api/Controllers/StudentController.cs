using AutoMapper;
using AcademyManagementApi.Data;
using AcademyManagementApi.Data.Entities;
using AcademyManagementApi.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademyManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Manager")]
    public class StudentController(AcademyDbContext _context, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _context.Students.ToList();
            var studentDTOs = _mapper.Map<List<StudentDTO>>(students);
            return Ok(studentDTOs);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _context.Students.Find(id);

            if (student == null)
                return NotFound(new { message = "Student not found." });

            return Ok(_mapper.Map<StudentDTO>(student));
        }

        [HttpPost]
        public IActionResult Save(StudentSaveDTO model)
        {
            var email = model.Email.Trim();

            if (_context.Students.Any(s => s.Email == email))
            {
                return Conflict(new { message = "This email is already registered." });
            }

            var student = _mapper.Map<Student>(model);
            student.Email = email;

            _context.Students.Add(student);
            _context.SaveChanges();

            var studentDTO = _mapper.Map<StudentDTO>(student);

            return CreatedAtAction(nameof(GetStudentById), 
                new { id = student.Id }, studentDTO);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateStudent(int id, StudentSaveDTO model)
        {
            var student = _context.Students.Find(id);

            if (student == null)
                return NotFound(new { message = "Student not found." });

            var email = model.Email.Trim();

            if (_context.Students.Any(
                s => s.Email == email && s.Id != id))
            {
                return Conflict(new { message = "This email is already registered." });
            }

            _mapper.Map(model, student);
            student.Email = email;

            _context.SaveChanges();

            return Ok(_mapper.Map<StudentDTO>(student));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);

            if (student == null)
                return NotFound(new { message = "Student not found." });

            if (_context.Enrollments.Any(e => e.StudentId == id))
            {
                return Conflict(new { message = "Student with enrollment history cannot be deleted." });
            }

            _context.Students.Remove(student);
            _context.SaveChanges();

            return Ok(new { message = "Student Deleted Successfully." });
        }
    }
}