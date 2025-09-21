using LinkService.Application;
using FluentAssertions;

namespace LinkService.Tests;

public class SlugServiceTests
{
    [Fact]
    public void Generate_Fixed_Length_And_UrlSafe()
    {
        var slugService = new SlugService();
        var slug = slugService.NewSlug(7);
        Assert.Equal(7, slug.Length);
        slug.Should().MatchRegex("^[a-zA-Z0-9]+$");
    }
}
