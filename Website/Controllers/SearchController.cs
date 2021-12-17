using Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchEngine.Indices;
using SearchEngine.Interfaces;
using Website.Models.Search;
using Website.Utils.Account;

namespace Website.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SearchController : ControllerBase
{
    private readonly IJournalSearcher<JournalDocument> searcher;
    private readonly JournalsRecommenderData data;

    public SearchController(IJournalSearcher<JournalDocument> searcher, JournalsRecommenderData data)
    {
        this.searcher = searcher;
        this.data = data;
    }

    [HttpPost]
    [Authorize(Roles = ApplicationRoles.ALL)]
    public async Task<SearchResult<JournalDocument>> Search([FromBody] SearchRequest search)
    {
        var result = await searcher.GetJournalsAllCondition(search.Title, search.Abstract, string.Join(",", search.Keywords));
        //TODO: paginate need to be handle in frontend, here it will return all journals
        return new SearchResult<JournalDocument>()
        {
            Total = result.Count(),
            Page = 1,
            Size = 20,
            Items = result.ToList()
        };
    }

    [HttpPost("get-all")]
    [Authorize(Roles = ApplicationRoles.ALL)]
    public async Task<SearchResult<JournalDocument>> SearchAll()
    {
        var result = await searcher.GetAllAsync();
        //TODO: paginate need to be handle in frontend, here it will return all journals
        return new SearchResult<JournalDocument>()
        {
            Total = result.Count(),
            Page = 1,
            Size = 20,
            Items = result.Select(x => new JournalResult<JournalDocument>(x)).ToList()
        };
    }

    [HttpPost("fill-index")]
    [Authorize(Roles = ApplicationRoles.ADMIN)]
    public async Task<IActionResult> FillIndex([FromQuery] int version)
    {
        //await searcher.CleanIndexAsync();

        var journals = data.GetAllJournals(version);

        await searcher.InsertManyAsync(journals);

        return Ok(new { Result = "Data successfully registered with Elasticsearch" });
    }

    [HttpPost("create-index")]
    [Authorize(Roles = ApplicationRoles.ADMIN)]
    public async Task<IActionResult> CreateIndex()
    {
        await searcher.CreateIndexAsync();

        return Ok(new { Result = "Index created sucessfully." });
    }
}