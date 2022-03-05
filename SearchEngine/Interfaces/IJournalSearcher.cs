using Common.Models;
using SearchEngine.Indices;

namespace SearchEngine.Interfaces;


public interface IJournalSearcher<T> where T : BaseIndex 
{
    Task InsertManyAsync(IEnumerable<Journal> journals);    
    Task CreateIndexAsync();
    Task CleanIndexAsync();
    void SetIndex(string index);
    Task<ICollection<T>> GetAllAsync();
    Task<ICollection<JournalResult<T>>> SearchInAllFiels(string term);
    Task<ICollection<JournalResult<T>>> GetJournals(string title, string _abstract, string keywords, IEnumerable<RefineItem> refines);
}