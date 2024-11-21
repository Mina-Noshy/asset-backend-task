using Microsoft.AspNetCore.Mvc;

namespace Asset.Api.Controllers.Base;

[Route("api/v{version:apiVersion}/bookinventory/[controller]")]
public class BaseV2BookInventoryController : BaseApiController
{
}
