using Microsoft.AspNetCore.Mvc;

namespace webapi;

[ApiController]
[Route("/api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
}