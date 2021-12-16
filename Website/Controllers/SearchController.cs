using Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchEngine.Interfaces;
using Website.Models.Search;
using Website.Utils.Account;

namespace Website.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SearchController : ControllerBase
{
    private readonly IJournalSearcher searcher;
    private readonly JournalsRecommenderData data;

    public SearchController(IJournalSearcher searcher, JournalsRecommenderData data)
    {
        this.searcher = searcher;
        this.data = data;
    }

    [HttpPost]
    [Authorize(Roles = ApplicationRoles.ALL)]
    public async Task<SearchResult> Search([FromBody] SearchRequest search)
    {
        //TODO: Improve search
        var term = $"{search.Title} {search.Abstract} {string.Join(" ", search.Keywords)}";
        var result = await searcher.GetJournalsAllCondition(term,search.ImpactFactor.Max, search.ImpactFactor.Min);
        //TODO: paginate need to be handle in frontend, here it will return all journals
        return new SearchResult()
        {
            Total = result.Count(),
            Page = 1,
            Size = 20,
            Items = result.ToList()
        };
    }

    [HttpPost("generate-index")]
    [Authorize(Roles = ApplicationRoles.ADMIN)]
    public async Task<IActionResult> PostSampleData([FromQuery] int version)
    {
        //await searcher.CleanIndexAsync();

        var journals = data.GetAllJournals(version);

        await searcher.InsertManyAsync(journals);

        return Ok(new { Result = "Data successfully registered with Elasticsearch" });
    }
}