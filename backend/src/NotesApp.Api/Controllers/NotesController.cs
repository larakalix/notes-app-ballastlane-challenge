using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Application.DTOs.Notes;
using NotesApp.Application.Interfaces;

namespace NotesApp.Api.Controllers;

[ApiController]
[Route("api/notes")]
[Authorize]
public class NotesController(INoteService noteService) : ControllerBase
{
    private readonly INoteService _noteService = noteService;

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateNote request, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.GetCurrentUserIdOrThrow(User);
        var response = await _noteService.CreateAsync(userId, request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.GetCurrentUserIdOrThrow(User);
        var response = await _noteService.GetByUserAsync(userId, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.GetCurrentUserIdOrThrow(User);
        var response = await _noteService.GetByIdAsync(userId, id, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateNote request, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.GetCurrentUserIdOrThrow(User);
        var response = await _noteService.UpdateAsync(userId, id, request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.GetCurrentUserIdOrThrow(User);
        await _noteService.DeleteAsync(userId, id, cancellationToken);
        return NoContent();
    }
}
