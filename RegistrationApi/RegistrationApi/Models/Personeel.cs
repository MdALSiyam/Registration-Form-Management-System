using System;
using System.Collections.Generic;

namespace RegistrationApi.Models;

public partial class Personeel
{
    public int Sl { get; set; }

    public string? PName { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? EMail { get; set; }

    public string? PType { get; set; }

    public string? Description { get; set; }

    public string? Qualification { get; set; }

    public string? PicPath { get; set; }

    public string? Password { get; set; }

    public DateTime? LastAccessDate { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public string? UpdateBy { get; set; }

    public string? Organization { get; set; }

    public string? Designation { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }
}
