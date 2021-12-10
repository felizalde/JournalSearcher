using SearchEngine.Abstractions;
using SearchEngine.Indices;
using SearchEngine.Interfaces;

namespace Website.Models.Search;

public class SearchResult
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public List<JournalResult<JournalDocument>> Items { get; set; }

}