using Asp.Versioning;
using BadmintonSystem.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonSystem.Presentation.Controllers.V1;

[ApiVersion(1)]
public class AuthenControllers : ApiController
{
    public AuthenControllers(ISender sender)
        : base(sender)
    {
    }

    [HttpGet(Name = "unauthorized")]
    public IActionResult Unauthorized()
    {
        return Unauthorized();
    }
}
