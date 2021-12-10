using Common.Models;

namespace Common;

public interface IEditorial
{
    string Key { get; init;}
    string BaseUrl { get; init; }
    string GetListUrl(int subject, int page = 1);
    bool IsJournalPage(Journal journal, string url);
}

public class WileyEditorial : IEditorial
{
    public string Key { get; init; }  = "Wiley";
    public string BaseUrl { get; init; } = "https://onlinelibrary.wiley.com/";
    public string GetListUrl(int subject, int page = 1)
    {
        return $"{BaseUrl}action/showPublications?ConceptID={subject}&PubType=journal&pageSize=200&startPage=0";
    }
    public bool IsJournalPage(Journal journal, string url)
    {
        return journal.Url == url;
    }
}

public class ElsevierEditorial : IEditorial
{
    public string Key { get; init; } = "Elsevier";
    public string BaseUrl { get; init; } = "https://www.elsevier.com/";
    public string GetListUrl(int subject, int page = 1)
    {
        return $"{BaseUrl}search-results?labels=journals&subject-0={subject}&page={page}";
    }

    public bool IsJournalPage(Journal journal, string url)
    {
        if (journal.Url == url) return true;
        if (url.EndsWith("/" + journal.OriginalID)) return true;
        if (url.EndsWith($"{journal.OriginalID}/home")) return true;

        return false;
    }
}

public class SpringerEditorial : IEditorial
{
    public string Key { get; init; } = "Springer";
    public string BaseUrl { get; init; } = "https://link.springer.com/";
    public string GetListUrl(int subject, int page = 1)
    {
        return $"{BaseUrl}search/page/{page}?facet-discipline=%22Computer+Science%22&facet-content-type=%22Journal%22";
    }

    public bool IsJournalPage(Journal journal, string url)
    {
        if (journal.Url == url) return true;
        if (url.EndsWith("/" + journal.OriginalID)) return true;
        var subdomain = journal.Title.Replace(" ", string.Empty).ToLower();
        if (url.Contains(subdomain)) return true;

        return false;
    }
}

public class IEEEEditorial : IEditorial
{
    public string Key { get; init; } = "IEEE";
    public string BaseUrl { get; init; } = "https://ieeexplore.ieee.org/";
    public string GetListUrl(int subject, int page = 1)
    {
        return $"{BaseUrl}browse/periodicals/title?refinements=ContentType:Journals&refinements=Publisher:IEEE&refinements=Topic:Computing%20and%20Processing&rowsPerPage=100&pageNumber={page}";
    }

    public bool IsJournalPage(Journal journal, string url)
    {
        return journal.Url == url;
    }
}
