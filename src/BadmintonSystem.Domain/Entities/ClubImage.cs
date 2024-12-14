﻿using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities.Identity;

public class ClubImage : EntityAuditBase<Guid>
{
    public string ImageLink { get; set; }

    public Guid ClubId { get; set; }
}