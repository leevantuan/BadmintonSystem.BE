﻿using BadmintonSystem.Application.Attributes;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonSystem.Presentation.Extensions;

public static class JwtAuthorizationExtensions
{
    public static RouteHandlerBuilder RequireJwtAuthorize
    (
        this RouteHandlerBuilder builder,
        string functionKey,
        int actionValue)
    {
        builder.AddEndpointFilter(async (context, next) =>
        {
            HttpContext httpContext = context.HttpContext;
            UserManager<AppUser> userManager = httpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
            RoleManager<AppRole> roleManager = httpContext.RequestServices.GetRequiredService<RoleManager<AppRole>>();

            var filter = new JwtAuthorizeFilter(functionKey, actionValue, userManager, roleManager);

            bool isAccess = await filter.CanAccessToAction(httpContext);

            if (!isAccess)
            {
                throw new AuthenticationException.AuthorizedForbiddenException($"You not have access {functionKey}");
            }

            return await next(context);
        });

        return builder;
    }
}
