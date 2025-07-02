using System;
using System.Collections.Generic;

namespace RegistrationApi.Models;

public partial class ViwRegistration
{
    public int? IPersoneelSl { get; set; }

    public int? IAnnouncementSl { get; set; }

    public string? PName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? EMail { get; set; }

    public string? PType { get; set; }

    public string? Description { get; set; }

    public string? Qualification { get; set; }

    public string? Organization { get; set; }

    public string? Designation { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }

    public int? CourseSl { get; set; }

    public string? CourseName { get; set; }

    public string? CourseType { get; set; }

    public DateTime? DRegistrationDate { get; set; }

    public int? IFees { get; set; }

    public string? SPaymentMethod { get; set; }

    public string? VCategory { get; set; }

    public string? SStatus { get; set; }

    public string? VPaymentType { get; set; }

    public string? VTrxId { get; set; }

    public string? VEntryBy { get; set; }

    public int Sl { get; set; }
}
