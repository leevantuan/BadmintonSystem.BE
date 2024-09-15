using Asp.Versioning;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonSystem.Presentation.Controllers.V2;

[ApiVersion(2)]
public class TestController : ApiController
{
    public TestController(ISender sender) : base(sender)
    {
    }

    //[HttpGet(Name = "SayHi")]
    //[ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> SayHi()
    //{
    //    var hi = "Hi Customer!";

    //    return Ok(Result.Success(hi));
    //}
}
