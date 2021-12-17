using SearchEngine.Indices;
using SearchEngine.Interfaces;

namespace Website.Models.Search;

public class SearchResult<T> where T : BaseIndex
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public List<JournalResult<T>> Items { get; set; }

}