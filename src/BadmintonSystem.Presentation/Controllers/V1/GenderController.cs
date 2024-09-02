using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.Gender;
using BadmintonSystem.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonSystem.Presentation.Controllers.V1;
public class GenderController : ApiController
{
    public GenderController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet(Name = "GetGenders")]
    [ProducesResponseType(typeof(Result<IEnumerable<Response.GenderResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllGender()
    {
        return Ok(await Sender.Send(new Query.GetAllGender()));
    }
}
