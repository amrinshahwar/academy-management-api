using System;
using System.Collections.Generic;

namespace AcademyManagementApi.Data.Entities;

public partial class Batch
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }

    public decimal TuitionFee { get; set; }

    public string Status { get; set; } = null!;

    public int CourseCode { get; set; }

    public virtual Course CourseCodeNavigation { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
