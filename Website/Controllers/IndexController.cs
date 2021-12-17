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
public class IndexController : ControllerBase
{
    private readonly IJournalSearcher<JournalDocument> searcher;
    private readonly JournalsRecommenderData data;

    public IndexController(IJournalSearcher<JournalDocument> searcher, JournalsRecommenderData data)
    {
        this.searcher = searcher;
        this.data = data;
    }


    [HttpPost("fill")]
    [Authorize(Roles = ApplicationRoles.ADMIN)]
    public async Task<IActionResult> FillIndex([FromQuery] int version)
    {
        await searcher.CleanIndexAsync();

        var journals = data.GetAllJournals(version);

        await searcher.InsertManyAsync(journals);

        return Ok(new { Result = "Data successfully registered with Elasticsearch" });
    }

    [HttpPost("create")]
    [Authorize(Roles = ApplicationRoles.ADMIN)]
    public async Task<IActionResult> CreateIndex()
    {
        await searcher.CreateIndexAsync();

        return Ok(new { Result = "Index created sucessfully." });
    }
}