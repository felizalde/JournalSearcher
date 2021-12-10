using Common;
using Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scraper;
using System.Linq;
using System.Security.Claims;
using Website.Utils.Account;

namespace Website.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ScrappersController : ControllerBase
{
    private readonly ILogger<ScrappersController> logger;
    private readonly JournalsRecommenderData data;

    public ScrappersController(ILogger<ScrappersController> logger, JournalsRecommenderData data)
    {
        this.data = data;
        this.logger = logger;
    }

    private void InsertJournals(IEnumerable<Journal> journals, int version)
    {
        var journals_version = journals.ToList();
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        journals_version.ForEach( j =>  {
            j.Version = version;
            j.CreatedBy = Guid.Parse(userId);
            j.CreatedAt = DateTime.Now;
        });

        data.InsertBulkJournals(journals_version);
    }

    
    // GET: api/<WileyRunnerController>
    [HttpGet, Route("wiley")]
    [Authorize(Roles = ApplicationRoles.ADMIN)]
    public async Task<IEnumerable<Journal>> RunWileyScrapper([FromServices] WileyScraper scraper, int version)
    {
        logger.LogInformation("Start running Wiley Scrapper");
        var startTime = DateTime.Now;

        var journals = await scraper.Run();
        InsertJournals(journals, version);

        var totalTime = startTime - DateTime.Now;
        logger.LogInformation("End execution of Wiley Scrapper. Total time:{totalTime}", totalTime);

        return journals;
    }

    [HttpGet, Route("elsevier")]
    [Authorize(Roles = ApplicationRoles.ADMIN)]
    public async Task<IEnumerable<Journal>> RunElsevierScrapper([FromServices] ElsevierScraper scraper, int version)
    {
        logger.LogInformation("Start running Elsevier Scrapper");
        var startTime = DateTime.Now;

        var journals = await scraper.Run();
        InsertJournals(journals, version);

        var totalTime = startTime - DateTime.Now;
        logger.LogInformation("End execution of Elsevier Scrapper. Total time:{totalTime}", totalTime);

        return journals;
    }

    [HttpGet, Route("springer")]
    [Authorize(Roles = ApplicationRoles.ADMIN)]
    public async Task<IEnumerable<Journal>> RunSpringerScrapper([FromServices] SpringerScraper scraper, int version)
    {
        logger.LogInformation("Start running Springer Scrapper");
        var startTime = DateTime.Now;

        var journals = await scraper.Run();
        InsertJournals(journals, version);

        var totalTime = startTime - DateTime.Now;
        logger.LogInformation("End execution of Springer Scrapper. Total time:{totalTime}", totalTime);

        return journals;
    }



}
