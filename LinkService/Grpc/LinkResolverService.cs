using Grpc.Core;
using LinkService.Infrastructure;
using Microsoft.EntityFrameworkCore;
//using TinyLinks;

namespace LinkService.Grpc;

public class LinkResolverService : LinkResolver.LinkResolverBase
{
    private readonly LinkDbContext dbContext;
    public LinkResolverService(LinkDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task<ResolveReplyDTO> Resolve(ResolveRequestDTO request, ServerCallContext callContext)
    {
        var l = await dbContext.Links.AsNoTracking().FirstOrDefaultAsync(x => x.Code == request.Code, callContext.CancellationToken);
        if (l is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "not found"));
        }
        return new ResolveReplyDTO { Url = l.TargetUrl, LinkId = l.Id.ToString() };
    }
}