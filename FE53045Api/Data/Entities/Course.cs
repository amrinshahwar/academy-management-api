using System;
using System.Collections.Generic;

namespace AcademyManagementApi.Data.Entities;

public partial class Course
{
    public int CourseCode { get; set; }

    public string Title { get; set; } = null!;

    public double TotalDuration { get; set; }

    public string Batch { get; set; } = null!;

    public virtual ICollection<Batch> Batches { get; set; } = new List<Batch>();
}
