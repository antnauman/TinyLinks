using LinkService.Application;
using LinkService.Infrastructure;
using LinkService.Contracts;
using Microsoft.AspNetCore.Mvc;
using LinkService.Domain;
using Microsoft.EntityFrameworkCore;

namespace LinkService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LinksController : ControllerBase
{
    private readonly LinkDbContext dbContext;
    private readonly ISlugService slugService;

    public LinksController(LinkDbContext dbContext, ISlugService slugService)
    {
        this.dbContext = dbContext;
        this.slugService = slugService;
    }

    [HttpPost]
    public async Task<ActionResult<LinkResponseDTO>> Create(CreateLinkRequestDTO createLinkRequest, CancellationToken ct)
    {
        var ownerSub = User?.FindFirst("sub")?.Value ?? "dev-user";
        var code = slugService.NewSlug();
        var link = new LinkEntity { OwnerSub = ownerSub, Code = code, TargetUrl = createLinkRequest.TargetUrl, Threshold = createLinkRequest.Threshold };
        dbContext.Add(link);
        await dbContext.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetById), new { id = link.Id }, new LinkResponseDTO(link.Id, link.Code, link.TargetUrl, link.Threshold, link.OwnerSub, link.CreatedUtc));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LinkResponseDTO>> GetById(Guid id, CancellationToken ct)
    {
        var l = await dbContext.Links.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        return l is null ? NotFound() : new LinkResponseDTO(l.Id, l.Code, l.TargetUrl, l.Threshold, l.OwnerSub, l.CreatedUtc);
    }

    [HttpGet("code/{code}")]
    public async Task<ActionResult<string>> GetByCode(string code, CancellationToken ct)
    {
        var l = await dbContext.Links.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code, ct);
        return l is null ? NotFound() : l.TargetUrl;
    }
}