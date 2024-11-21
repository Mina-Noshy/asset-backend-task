using Microsoft.AspNetCore.Mvc;

namespace Asset.Api.Controllers.Base;

[Route("api/v{version:apiVersion}/auth/[controller]")]
public class BaseV1AuthController : BaseApiController
{
}
