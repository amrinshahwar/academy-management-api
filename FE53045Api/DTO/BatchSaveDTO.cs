using System.ComponentModel.DataAnnotations;

namespace AcademyManagementApi.DTO
{
    public class BatchSaveDTO
    {
        [Required]
        public DateTime? StartDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tuition fee must be zero or greater than zero.")]
        public decimal TuitionFee { get;  set; }

        [Required] 
        [StringLength(150)] 
        public string Status { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Course code is not valid")]
        public int CourseCode { get; set; }
    }
}
