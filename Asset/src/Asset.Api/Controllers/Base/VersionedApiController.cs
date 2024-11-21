using Microsoft.AspNetCore.Mvc;

namespace Asset.Api.Controllers.Base
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VersionedApiController : BaseApiController
    {
    }
}
