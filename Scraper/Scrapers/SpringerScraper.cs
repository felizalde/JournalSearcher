using AngleSharp;
using AngleSharp.Dom;
using Common;
using Common.Models;
using Scraper.Utils;

namespace Scraper;
public class SpringerScraper : Scraper
{
    public SpringerScraper(IBrowsingContext browsingContext) : base(browsingContext)
    {
        editorial = new SpringerEditorial();
    }
    private int GetTotalPages(IDocument doc)
    {
        var pages = doc.QuerySelector(".number-of-pages")?.TextContent.Trim();
        var total_pages = int.Parse(pages);

        return total_pages;
    }

    public override async Task<ISet<Journal>> GetListOfJouranls()
    {
        var list_page = await browser.OpenAsync(editorial.GetListUrl(0, 1));

        var total_pages = GetTotalPages(list_page);

        var journals = new HashSet<Journal>();
        foreach (var page in Enumerable.Range(1, total_pages))
        {
            var page_url = editorial.GetListUrl(0, page);
            var doc = await browser.OpenAsync(page_url);
            journals.UnionWith(ParseListItems(doc.QuerySelectorAll(".content-item-list > li")));
        }

        return journals;
    }

    public override async Task<Journal> FillJournalDetails(Journal journal, IDocument page)
    {
        if (!ScraperUtils.IsRedirectPage(journal, page) ||
                    page.Url.EndsWith($"journal/{journal.OriginalID}"))
        {
            await ParseJournalPageAndFillJournal(page, journal);
        }
        else
        {
            if (page.Url.EndsWith("springeropen.com/"))
            {
                ParseJournalPageAndFillJournalSpringerOpen(page, journal);
            }
        }

        return journal;
    }

    private ISet<Journal> ParseListItems(IEnumerable<IElement> elems) =>
        elems.Aggregate(new HashSet<Journal>(), (journals, li) =>
        {
            var titleElement = li.QuerySelector(".title");
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
            //ReadMetrics(li, journal);
            journals.Add(journal);
            return journals;
        });
    private void ParseJournalPageAndFillJournalSpringerOpen(IDocument doc, Journal journal)
    {
        var content = doc.QuerySelector(".placeholder-aimsAndScope_content")?.TextContent.Trim();

        journal.AimsAndScope = content;
        ParseMetricSpringerOpenPage(journal, doc);
    }
    private async Task ParseJournalPageAndFillJournal(IDocument doc, Journal journal)
    {
        ParseAboutDetailPage(journal, doc);
        ParseMetricDetailPage(journal, doc);
        await ParseAimsAndScope(journal, doc.Url + "/aims-and-scope");
    }
    public async Task ParseAimsAndScope(Journal journal, string url)
    {
        var doc = await browser.OpenAsync(url);
        var content = doc.QuerySelector(".placeholder-aimsAndScope_content")?.TextContent.Trim();


        journal.AimsAndScope = content;
    }
    private static void ParseAboutDetailPage(Journal journal, IDocument doc)
    {
        var content = doc.QuerySelector(".app-promo-text.app-promo-text--keyline")?.TextContent.Trim();

        journal.About = content;
    }
    private static void ParseMetricDetailPage(Journal journal, IDocument doc)
    {
        var metrics = doc.QuerySelector(".app-journal-metrics");
        var metrics_list = metrics?.QuerySelectorAll(".app-journal-metrics__details");
        if (metrics_list == null) return;
        foreach (var metric in metrics_list)
        {
            var metric_value = metric.TextContent.Trim();
            var metric_name = metric.NextElementSibling?.TextContent.Trim();
            metric_value = metric_value.Replace("(2020)", "").Replace("days", "").Trim();
            journal.Metrics.Add(new() { Name = metric_name, Value = ScraperUtils.ParseDouble(metric_value, 0) });

            if (metric_name.ToLower() == "impact factor")
            {
                journal.ImpactFactor = ScraperUtils.ParseDouble(metric_value, 0);
            }
        }
    }
    private static void ParseMetricSpringerOpenPage(Journal journal, IDocument doc)
    {
        var sections = doc.QuerySelectorAll(".c-separator > h3");
        var metrics = sections?.Where(x => x.TextContent.Trim() == "Annual Journal Metrics")?.FirstOrDefault();
        if (metrics == null) return;

        var metrics_list = metrics.NextElementSibling.QuerySelectorAll("p");

        //TODO: reduce complexity of code. very hard to read
        foreach (var metric in metrics_list)
        {
            var title = metric.QuerySelector("strong")?.TextContent.Trim();
            if (title == null) continue;
            metric.QuerySelector("strong").Remove();
            foreach (var m in metric.QuerySelectorAll("br"))
            {
                if (title.Equals("Speed", StringComparison.OrdinalIgnoreCase))
                {
                    var data = m.NextSibling?.TextContent.Trim();
                    if (data == null) continue;
                    var values = data.Split(" ");
                    journal.Metrics.Add(new() { Name = string.Join(" ", values.Skip(1)), Value = ScraperUtils.ParseDouble(values[0], 0) });
                }
                else
                {
                    if (title.Equals("Citation Impact", StringComparison.OrdinalIgnoreCase))
                    {
                        var value = m.NextSibling?.TextContent.Trim();
                        var data = m.NextElementSibling?.TextContent.Trim();
                        if (data == null || value == null) continue;
                        value = value.Replace(" ", "").Replace("-", "");
                        journal.Metrics.Add(new() { Name = data, Value = ScraperUtils.ParseDouble(value, 0) });
                    }
                }
            }
        }
    }

}