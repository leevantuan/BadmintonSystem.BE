﻿namespace BadmintonSystem.Contract.Abstractions.Entities;

public interface IEntityAuditBase<TKey> : IEntityBase<TKey>, IAuditable
{ }
