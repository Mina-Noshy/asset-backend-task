using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Asset.Api.Controllers.Base;

[ApiController]
[Produces("application/json")]
public class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
