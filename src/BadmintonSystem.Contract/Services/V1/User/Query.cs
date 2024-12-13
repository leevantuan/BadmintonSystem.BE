﻿using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.User;

public static class Query
{
    public record RegisterByCustomerQuery(Request.CreateUserAndAddress Data) : IQuery;

    public record GetAddressesByEmailWithFilterAndSortQuery(
        Guid UserId,
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.AddressByUserDetailResponse>>;
}
