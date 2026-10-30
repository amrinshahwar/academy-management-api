using System.ComponentModel.DataAnnotations;

namespace AcademyManagementApi.DTO
{
    public class StudentSaveDTO
    {
        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(250)]
        public string Email { get; set; } = string.Empty;

        [StringLength(11)]
        public string? Phone { get; set; }
    }
}
