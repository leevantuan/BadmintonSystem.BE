﻿using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities;

public class Bill : EntityAuditBase<Guid>
{
    public decimal TotalPrice { get; set; }

    public decimal? TotalPayment { get; set; }

    public string? Content { get; set; }

    public string? Name { get; set; }

    public Guid? UserId { get; set; }

    public Guid? BookingId { get; set; }

    public BillStatusEnum Status { get; set; }

    public virtual ICollection<ServiceLine>? ServiceLines { get; set; }

    public virtual ICollection<BillLine>? BillLines { get; set; }

    public virtual Booking? Booking { get; set; }
}
