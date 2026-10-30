using System.Reflection;

namespace AcademyManagementApi.DTO
{
    public class CourseDTO
    {
        public int CourseCode { get; set; }
        public string Title { get; set; } = string.Empty;
        public double TotalDuration { get; set; }
        public string Batch { get; set; } = String.Empty;
    }
}
