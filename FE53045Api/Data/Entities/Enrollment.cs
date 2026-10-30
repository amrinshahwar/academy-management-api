using System;
using System.Collections.Generic;

namespace AcademyManagementApi.Data.Entities;

public partial class Enrollment
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int BatchId { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Batch Batch { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
