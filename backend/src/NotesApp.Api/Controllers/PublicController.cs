using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NotesApp.Api.Controllers;

[ApiController]
[Route("api/public")]
public class PublicController : ControllerBase
{
    [HttpGet("status")]
    [AllowAnonymous]
    public ActionResult Status()
        => Ok(new { status = "ok" });
}
