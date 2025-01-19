﻿using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.TimeSlot;

public static class Query
{
    public record GetTimeSlotByIdQuery(Guid Id)
        : IQuery<Response.TimeSlotResponse>;

    public record GetTimeSlotsWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.TimeSlotDetailResponse>>;
}
