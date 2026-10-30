using System.Xml.Schema;

namespace AcademyManagementApi.DTO
{
    public class BatchDTO
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public decimal TuitionFee { get; set; }
        public string Status { get; set; } = string.Empty;
        public int CourseCode { get; set; }
    }
}
