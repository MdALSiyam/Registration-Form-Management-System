using System;
using System.Collections.Generic;

namespace RegistrationApi.Models;

public partial class TblRegistration
{
    public int Sl { get; set; }

    public int? IPersoneelSl { get; set; }

    public int? IAnnouncementSl { get; set; }

    public DateTime? DRegistrationDate { get; set; }

    public string? VCategory { get; set; }

    public int? IFees { get; set; }

    public string? SPaymentMethod { get; set; }

    public string? SStatus { get; set; }

    public string? VPaymentType { get; set; }

    public string? VTrxId { get; set; }

    public string? VEntryBy { get; set; }
}
