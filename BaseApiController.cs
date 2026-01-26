using Microsoft.AspNetCore.Mvc;

namespace webapi;

[ApiController]
[Route("/api")]
public abstract class BaseApiController : ControllerBase
{
}