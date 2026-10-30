using System.ComponentModel.DataAnnotations;

namespace AcademyManagementApi.DTO
{
    public class CourseSaveDTO
    {
        [Required] 
        [StringLength(150)] 
        public string Title { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage="Total duration must be greater than zero")]
        public double TotalDuration  { get; set; }

        [Required] 
        [StringLength(150)]
        public string Batch { get; set; } = string.Empty;
    }
}
