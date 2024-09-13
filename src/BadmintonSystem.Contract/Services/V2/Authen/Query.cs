﻿using BadmintonSystem.Contract.Abstractions.Messages;
using static BadmintonSystem.Contract.Services.V2.Authen.Response;

namespace BadmintonSystem.Contract.Services.V2.Authen;
public static class Query
{
    public record Token(string? AccessToken, string? RefreshToken) : IQuery<Authenticed>;
}