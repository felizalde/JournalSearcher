using AngleSharp;
using AngleSharp.Dom;
using Common;
using Common.Models;
using Scraper.Utils;

namespace Scraper;
public class WileyScraper : Scraper
{

    private readonly List<int> Subjects = new();

    public WileyScraper(IBrowsingContext browsingContext) : base(browsingContext)
    {
        //55: Statistics
        //45: Mathematics
        //68: Computer Science
        //90: Electrical & Electronics Engineering
        Subjects.AddRange(new[] { 45 , 55, 68, 90 });
        editorial = new WileyEditorial();
    }


    private ISet<Journal> ParseListItems(IEnumerable<IElement> elems) =>
        elems.Aggregate(new HashSet<Journal>(), (journals, li) =>
        {
            var imgUrl = li.QuerySelectorAll("img").FirstOrDefault()?.GetAttribute("data-src");
            var Url = li.QuerySelectorAll("a").FirstOrDefault()?.GetAttribute("href"); 
            var Title = li.QuerySelector("a.visitable")?.TextContent;
            var journal = new Journal
            {
                ImgUrl = ScraperUtils.GetUrl(imgUrl, editorial.BaseUrl),
                Url = ScraperUtils.GetUrl(Url, editorial.BaseUrl),
                Title = Title,
                OriginalID = Url[(Url.LastIndexOf('/') + 1)..],
                Editorial = editorial.Key
            };

            journals.Add(journal);
            return journals;
        });

    public static void ParseDetailPage(Journal journal, IDocument doc)
    {
        var nodes = doc.QuerySelector(".main-content")?.QuerySelector(".pb-rich-text")?.ChildNodes;

        if (nodes != null)
        {
            int index = 0;
            foreach (var node in nodes)
            {
                if (node.NodeName.ToLower() == "h4")
                {
                    var h4 = node.TextContent;
                    var p = node.NextSibling?.NextSibling?.TextContent;
                    if (h4.Contains("Overview"))
                    {
                        journal.About = ScraperUtils.ReadNodesUntilNextSection(nodes, index + 1);
                    }
                    else if (h4.Contains("Keywords"))
                    {
                        journal.Keywords = ScraperUtils.ReadNodesUntilNextSection(nodes, index + 1);
                    }
                    else if (h4.Contains("Aims and Scope"))
                    {
                        journal.AimsAndScope = ScraperUtils.ReadNodesUntilNextSection(nodes, index + 1);
                    }
                }
                index = index + 1;
            }
        }
    }


    private async Task<ISet<Journal>> GetListBelowASubject(int subject)
    {
        var list_page = await browser.OpenAsync(editorial.GetListUrl(subject));
        var journals = ParseListItems(list_page.QuerySelectorAll("li.search__item"));

        return journals;
    }

    public override async Task<ISet<Journal>> GetListOfJouranls()
    {
        List<Task<ISet<Journal>>> tasks = new();
        foreach (var subject in Subjects)
        {
            tasks.Add(GetListBelowASubject(subject));
        }

        var res = await Task.WhenAll(tasks);

        var elements = res.SelectMany(x => x).ToList();

        return new HashSet<Journal>(elements);
    }

    public override async Task<Journal> FillJournalDetails(Journal journal, IDocument page)
    {
        if (page.Url.Contains("ietresearch"))
        {
            ParseImpactFactorForITEPage(journal, page);
        } 
        else
        {
            ParseImpactFactorForBasicPage(journal, page);
        }

        var overviewPageUrl = page.QuerySelector("#About-1-dropdown > li:nth-child(1) > a")?.GetAttribute("href");

        if (overviewPageUrl != null && overviewPageUrl.Contains("productinformation.html"))
        {
            var detailDoc = await browser.OpenAsync(ScraperUtils.GetUrl(overviewPageUrl, editorial.BaseUrl));
            ParseDetailPage(journal, detailDoc);

        }
        else
        {
            journal.Errors = "Not contain productinformation page";
        }

        return journal;
    }

    private void ParseImpactFactorForBasicPage(Journal journal, IDocument page)
    {
        var impactFactor = page.QuerySelector("#main-content > div > div > div:nth-child(1) > section > div > div > div.journal-info-container.col-md-8 > div:nth-child(3) > div:nth-child(1) > span.info_value")?.TextContent.Trim();
        journal.ImpactFactor = 0;
        if (impactFactor != null)
        {
            journal.ImpactFactor = ScraperUtils.ParseDouble(impactFactor, 0);
            journal.Metrics.Add(new() { Name = "Impact Factor", Value = journal.ImpactFactor.Value });
        }
    }

    private void ParseImpactFactorForITEPage(Journal journal, IDocument page)
    {
        var impactFactor = page.QuerySelector("#main-content > div.page-body.pagefulltext > div > section.hub-journal-bar > section > div > div > div.journal-info-container.col-md-8 > div > div:nth-child(2) > div:nth-child(1) > span.info_value")?.TextContent.Trim();
        journal.ImpactFactor = 0;
        if (impactFactor != null)
        {
            journal.ImpactFactor = ScraperUtils.ParseDouble(impactFactor, 0);
            journal.Metrics.Add(new() { Name = "Impact Factor", Value = journal.ImpactFactor.Value });
        }
    }
}
