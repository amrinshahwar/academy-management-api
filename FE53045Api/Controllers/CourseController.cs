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
    [Authorize(Roles = "Admin, Manager")]
    public class CourseController(AcademyDbContext _context, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCourses()
        {
            var courses = _context.Courses.ToList();
            var courseDTOs = _mapper.Map<List<CourseDTO>>(courses);
            return Ok(courseDTOs);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCourseById(int id)
        {
            var course = _context.Courses.Find(id);

            if(course == null)
                return NotFound(new{message="Course not found."});
            return Ok(_mapper.Map<CourseDTO>(course));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Save(CourseSaveDTO model)
        {
            var course = _mapper.Map<Course>(model);

            _context.Courses.Add(course);
            _context.SaveChanges();

            var courseDTO = _mapper.Map<CourseDTO>(course);

            return CreatedAtAction(
                nameof(GetCourseById),
                new { id = course.CourseCode },
                courseDTO);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCourse(int id, CourseSaveDTO model)
        {
            var course = _context.Courses.Find(id);

            if (course == null)
                return NotFound(new { message = "Course not found." });

            _mapper.Map(model, course);
            _context.SaveChanges();
            return Ok(_mapper.Map<CourseDTO>(course));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
                return NotFound(new { message = "Course not found." });

            if (_context.Batches.Any(b => b.CourseCode == id))
            {
                return Conflict(new { message = "Course with existing batches cannot be deleted." });
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();
            return Ok(new { message = "Course Deleted Successfully." });
        }
    }
}


