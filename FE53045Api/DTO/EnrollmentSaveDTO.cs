using System.ComponentModel.DataAnnotations;

namespace AcademyManagementApi.DTO
{
    public class EnrollmentSaveDTO
    {
        [Range(1, int.MaxValue)]
        public int StudentId { get; set; }

        [Range(1, int.MaxValue)]
        public int BatchId { get; set; }
    }
}
