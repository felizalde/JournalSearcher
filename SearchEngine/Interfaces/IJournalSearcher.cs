using Common.Models;
using SearchEngine.Indices;

namespace SearchEngine.Interfaces;


public interface IJournalSearcher<T> where T : BaseIndex 
{
    Task InsertManyAsync(IEnumerable<Journal> journals);    
    Task CreateIndexAsync();
    Task CleanIndexAsync();
    Task<ICollection<T>> GetAllAsync();
    Task<ICollection<JournalResult<T>>> SearchInAllFiels(string term);
    Task<ICollection<JournalResult<T>>> GetJournalsAllCondition(string title, string _abstract, string keywords);
}