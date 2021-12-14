using AngleSharp;
using AngleSharp.Dom;
using Common;
using Common.Models;
using Scraper.Utils;

namespace Scraper;
public class ElsevierScraper : Scraper
{
    private readonly List<int> Subjects = new();
    public ElsevierScraper(IBrowsingContext browsingContext) : base(browsingContext)
    {
        //27364 : "Computer Science"
        Subjects.Add(27364);
        editorial = new ElsevierEditorial();
    }    

    private ISet<Journal> ParseListItems(IEnumerable<IElement> elems) =>
        elems.Aggregate(new HashSet<Journal>(), (journals, li) =>
        {
            var titleElement = li.QuerySelector(".search-result-journal-title").QuerySelector("a");
            var imgUrl = li.QuerySelectorAll("img").FirstOrDefault()?.GetAttribute("src");
            var Url = titleElement?.GetAttribute("href");
            var Title = titleElement?.TextContent;
            var journal = new Journal
            {
                ImgUrl = imgUrl,
                Url = ScraperUtils.GetUrl(Url, editorial.BaseUrl),
                Title = Title,
                OriginalID = Url[(Url.LastIndexOf('/') + 1)..],
                Editorial = editorial.Key
            };
            ReadMetrics(li, journal);
            journals.Add(journal);
            return journals;
        });

    private static void ReadMetrics(IElement article, Journal journal)
    {
        var metrics = article.QuerySelector(".search-result-journal-metrics");
        // read each resul-detail-list-item and get the value
        var metrics_list = metrics.QuerySelectorAll(".result-detail-list-item");
        foreach (var metric in metrics_list)
        {
            var metric_name = metric.QuerySelector("dt")?.TextContent.Trim();
            var metric_value = metric.QuerySelector("dd").TextContent.Trim();
            journal.Metrics.Add(new() { Name = metric_name, Value = ScraperUtils.ParseDouble(metric_value, 0) });
        }
    }

    private async Task ParseJournalPageScienceDirect(IDocument doc, Journal journal)
    {
        var detail = await browser.OpenAsync(doc.Url + "/about/aims-and-scope");
        var content = detail.QuerySelector(".js-aims-and-scope")?.TextContent.Trim();


        journal.AimsAndScope = content;
        ParseMetricScienceDirectPage(journal, detail);
    }

    private static void ParseJournalPageAndFillJournal(IDocument doc, Journal journal)
    {
        ParseDetailPage(journal, doc);
        ParseMetricDetailPage(journal, doc);
    }

    public static void ParseDetailPage(Journal journal, IDocument doc)
    {
        var content = doc.QuerySelector(".sc-1w35udv-1.iMeAYe")?.TextContent.Trim();

        journal.AimsAndScope = content;
    }

    public static void ParseMetricDetailPage(Journal journal, IDocument doc)
    {
        var metrics = doc.QuerySelector(".sc-1dxr8k6-0.sc-ewcnzs-0");
        var metrics_list = metrics?.QuerySelectorAll(".sc-1q1u28e-0.hRitPy");
        if (metrics_list == null) return;
        foreach (var metric in metrics_list)
        {
            var metric_name = metric.QuerySelector("span.sc-11qqina-0.fwMVMZ")?.TextContent.Trim() 
                    ?? metric.QuerySelector("a > span")?.TextContent.Trim();
            var metric_value = metric.QuerySelector("p > span")?.TextContent.Trim() ?? "";
            metric_value = metric_value.Replace("weeks", "");
            journal.Metrics.Add(new() { Name = metric_name, Value = ScraperUtils.ParseDouble(metric_value, 0) });
            if (metric_name == "Impact Factor")
            {
                journal.ImpactFactor = ScraperUtils.ParseDouble(metric_value, 0);
            }
        }
    }
    public static void ParseMetricScienceDirectPage(Journal journal, IDocument doc)
    {
        var metrics = doc.QuerySelector(".thb58v-7.cZmsEm");
        var metrics_list = metrics?.QuerySelectorAll(".metric");
        if (metrics_list == null) return;
        foreach (var metric in metrics_list)
        {
            var metric_value = metric.QuerySelector("span.text-l.u-display-block")?.TextContent.Trim();
            var metric_name = metric.QuerySelector("span.text-xs")?.TextContent.Trim() ?? "";
            journal.Metrics.Add(new() { Name = metric_name, Value = ScraperUtils.ParseDouble(metric_value, 0) });
            if (metric_name == "Impact Factor")
            {
                journal.ImpactFactor = ScraperUtils.ParseDouble(metric_value, 0);
            }
        }
    }

    private int GetTotalPages(IDocument doc)
    {
        var pages = doc.QuerySelector(".pagination-status")?.TextContent.Trim();
        var total_pages = int.Parse(pages?.Split(" ")[pages.Split(" ").Length - 1]);

        return total_pages;
    }

    private async Task<ISet<Journal>> GetListBelowASubject(int subject)
    {
        var doc = await browser.OpenAsync(editorial.GetListUrl(subject, 1));
        var total_pages = GetTotalPages(doc);

        var journals = new HashSet<Journal>();
        foreach (var page in Enumerable.Range(1, total_pages))
        {
            var page_url = editorial.GetListUrl(subject, page);
            var list_page = await browser.OpenAsync(page_url);
            journals.UnionWith(ParseListItems(list_page.QuerySelectorAll("article.search-result-journal")));
        }

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
        if (journal != null)
        {
            if (!ScraperUtils.IsRedirectPage(journal, page))
            {
                ParseJournalPageAndFillJournal(page, journal);
            }
            else
            {
                await ParseJournalPageScienceDirect(page, journal);
            }
        }
        else
        {
            Console.WriteLine(page.Url);
        }

        return journal;
    }
}