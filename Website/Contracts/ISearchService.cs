using Website.Models.Search;

namespace Website.Contracts;

public interface ISearchService
{
    Task<SearchResult> SearchAsync(string title, string _abstract, List<string> keywords, double impactFactorMin, double impactFactorMax);
}