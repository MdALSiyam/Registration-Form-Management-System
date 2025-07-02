using System;
using System.Collections.Generic;

namespace RegistrationApi.Models;

public partial class TableCourseDetail
{
    public int Sl { get; set; }

    public string? CourseName { get; set; }

    public string? CourseType { get; set; }

    public string? Description { get; set; }

    public string? OutComes { get; set; }

    public string? Methology { get; set; }

    public string? CourseContent { get; set; }

    public string? WhoCanAttend { get; set; }

    public string? Boarding { get; set; }

    public string? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? CategorySl { get; set; }
}
