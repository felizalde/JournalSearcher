using AngleSharp;
using AngleSharp.Dom;
using Common;
using Common.Models;

namespace Scraper;

public abstract class Scraper
{
    protected readonly IBrowsingContext browser;
    protected IEditorial editorial;

    public Scraper(IBrowsingContext browsingContext)
    {
        this.browser = browsingContext;
    }

    /// <summary>
    /// Open the journal url and return the document
    /// </summary>
    public async Task<IDocument> GetDetailPage(Journal journal)
    {
        return await browser.OpenAsync(journal.Url);
    }

    /// <summary>
    /// Parse the list of journals and return them as a list of Journal objects with only title, url and imgUrl
    /// </summary>
    public abstract Task<ISet<Journal>> GetListOfJouranls();

    /// <summary>
    /// Parse the detail page and return the journal object with all the information
    /// </summary>
    public abstract Task<Journal> FillJournalDetails(Journal journal, IDocument page);

    /// <summary>
    /// Download all the detail pages in parallel.
    /// </summary>
    protected Task<IDocument[]> DownloadDetailPages(ISet<Journal> journals) =>
        Task.WhenAll(journals.Aggregate(new List<Task<IDocument>>(), (tasks, journal) =>
        {
            //Open every page async, and add the task to the list of tasks to be waited.
            tasks.Add(GetDetailPage(journal));
            return tasks;
        }));

    /// <summary>
    /// Read all journals from the editorial website and return them as a list of Journal objects with full information
    /// </summary>
    public async Task<ISet<Journal>> Run()
    {
        var journals = await GetListOfJouranls();
        var pages = await DownloadDetailPages(journals);

        foreach(var page in pages)
        {
            var journal = journals.FirstOrDefault(j => editorial.IsJournalPage(j, page.Url));
            if (journal != null)
            {
                await FillJournalDetails(journal, page);
            }
        }

        return journals;
    }


}