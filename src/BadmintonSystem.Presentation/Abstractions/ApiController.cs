using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonSystem.Presentation.Abstractions;

[ApiController]
[Route("api/[controller]")]
//[Route("api/v{version:apiVersion}/[controller]")]

// Abstract is method generic,Member can't edit
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }
}
