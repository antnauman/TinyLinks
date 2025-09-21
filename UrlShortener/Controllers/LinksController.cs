using LinkService.Application;
using LinkService.Contracts;
using Microsoft.AspNetCore.Mvc;
using LinkService.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LinksController : ControllerBase
{
    private readonly ISlugService slugService;

    public LinksController(ISlugService slugService)
    {
        this.slugService = slugService;
    }

    [HttpPost]
    public async Task<ActionResult<LinkResponseDTO>> Create(LinkRequestDTO createLinkRequest)
    {
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LinkResponseDTO>> ResolveById(Guid id)
    {
        return StatusCode(200);
    }

    [HttpGet("code/{code}")]
    public async Task<ActionResult<string>> ResolveByCode(string code)
    {
        return StatusCode(200);
    }
}