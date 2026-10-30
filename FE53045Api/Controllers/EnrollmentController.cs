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
    public class EnrollmentController(AcademyDbContext _context, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetEnrollments()
        {
            var enrollments = _context.Enrollments.ToList();
            var enrollmentDTOs = _mapper.Map<List<EnrollmentDTO>>(enrollments);

            return Ok(enrollmentDTOs);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetEnrollmentById(int id)
        {
            var enrollment = _context.Enrollments.Find(id);

            if (enrollment == null)
            {
                return NotFound(new { message = "Enrollment details not found." });
            }

            return Ok(_mapper.Map<EnrollmentDTO>(enrollment));
        }

        [HttpPost]
        public IActionResult Save(EnrollmentSaveDTO model)
        {
            var studentExists = _context.Students.Any(s => s.Id == model.StudentId);

            if (!studentExists)
                return BadRequest(new { message = "Student not found." });

            var batch = _context.Batches.Find(model.BatchId);

            if (batch == null)
                return BadRequest(new { message = "Batch not found." });

            if (!string.Equals(batch.Status, "Enrolling", StringComparison.OrdinalIgnoreCase))
                return Conflict(new { message = "This batch is not accepting enrollment." });

            var alreadyEnrolled = _context.Enrollments.Any(e => e.StudentId == model.StudentId
                                                                && e.BatchId == model.BatchId && e.Status == "Active");
            if (alreadyEnrolled)
            {
                return Conflict(new { message = "Student is already enrolled in this batch." });
            }

            var enrollment = new Enrollment
            {
                StudentId = model.StudentId,
                BatchId = model.BatchId,
                EnrollmentDate = DateTime.UtcNow,
                Status = "Active"
            };

            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();

            var enrollmentDTO = _mapper.Map<EnrollmentDTO>(enrollment);

            return CreatedAtAction(nameof(GetEnrollmentById),
                new { id = enrollment.Id }, enrollmentDTO);

        }

        [HttpPatch("{id:int}/cancel")]
        public IActionResult CancelEnrollment(int id)
        {
            var enrollment = _context.Enrollments.Find(id);

            if (enrollment == null)
            {
                return NotFound(new { message = "Enrollment detail not found." });
            }

            if (enrollment.Status == "Cancelled")
            {
                return Conflict(new { message = "Enrollment is already cancelled." });
            }

            enrollment.Status = "Cancelled";
            _context.SaveChanges();

            return Ok(_mapper.Map<EnrollmentDTO>(enrollment));
        }
    }
}
