using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Asset.Api.Controllers.Base;

[ApiVersionNeutral]
[Route("api/[controller]")]
public class VersionNeutralApiController : BaseApiController
{
}
