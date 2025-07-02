using System;
using System.Collections.Generic;

namespace RegistrationApi.Models;

public partial class Announcement
{
    public int Sl { get; set; }

    public int? CourseSl { get; set; }

    public int? Rpsl { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateEnd { get; set; }

    public DateTime? TimeFrom { get; set; }

    public DateTime? TimeEnd { get; set; }

    public int? NoOfClass { get; set; }

    public string? Days { get; set; }

    public DateTime? LastDateOfReg { get; set; }

    public string? Venue { get; set; }

    public decimal? Fees { get; set; }

    public string? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? TotalHrs { get; set; }

    public byte[]? Banner { get; set; }

    public DateTime? EarlyBird { get; set; }
}
