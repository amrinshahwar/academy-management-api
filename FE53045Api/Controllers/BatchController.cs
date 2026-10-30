using AutoMapper;
using AcademyManagementApi.Data;
using AcademyManagementApi.Data.Entities;
using AcademyManagementApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AcademyManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Manager")]
    public class BatchController(
        AcademyDbContext _context,
        IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBatches()
        {
            var batches = _context.Batches
                .AsNoTracking()
                .ToList();

            var batchDTOs = _mapper.Map<List<BatchDTO>>(batches);

            return Ok(batchDTOs);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBatchById(int id)
        {
            var batch = _context.Batches.Find(id);

            if (batch == null)
                return NotFound(new { message = "Batch not found." });

            return Ok(_mapper.Map<BatchDTO>(batch));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Save(BatchSaveDTO model)
        {
            var courseExists = _context.Courses
                .Any(c => c.CourseCode == model.CourseCode);

            if (!courseExists)
            {
                return BadRequest(new
                {
                    message = "The selected course does not exist."
                });
            }

            var batch = _mapper.Map<Batch>(model);

            _context.Batches.Add(batch);
            _context.SaveChanges();

            var batchDTO = _mapper.Map<BatchDTO>(batch);

            return CreatedAtAction(
                nameof(GetBatchById),
                new { id = batch.Id },
                batchDTO);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateBatch(int id, BatchSaveDTO model)
        {
            var batch = _context.Batches.Find(id);

            if (batch == null)
                return NotFound(new { message = "Batch not found." });

            var courseExists = _context.Courses
                .Any(c => c.CourseCode == model.CourseCode);

            if (!courseExists)
            {
                return BadRequest(new
                {
                    message = "The selected course does not exist."
                });
            }

            _mapper.Map(model, batch);

            _context.SaveChanges();

            return Ok(_mapper.Map<BatchDTO>(batch));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteBatch(int id)
        {
            var batch = _context.Batches.Find(id);

            if (batch == null)
                return NotFound(new { message = "Batch not found." });

            if (_context.Enrollments.Any(e => e.BatchId == id))
            {
                return Conflict(new { message = "Cannot delete a batch with enrollment history." });
            }
            _context.Batches.Remove(batch);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Batch deleted successfully."
            });
        }
    }
}