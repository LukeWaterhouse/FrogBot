using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogBot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController() : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> TestAsync(CancellationToken ct)
    {
        return Ok("Hello World");
    }
}