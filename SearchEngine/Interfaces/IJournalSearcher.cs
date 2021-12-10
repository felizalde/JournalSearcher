using Common.Models;
using SearchEngine.Indices;

namespace SearchEngine.Interfaces;


public interface IJournalSearcher
{
    Task InsertManyAsync(IEnumerable<Journal> journals);
    Task CleanIndexAsync();
    Task<ICollection<JournalDocument>> GetAllAsync();
    Task<ICollection<JournalResult<JournalDocument>>> SearchInAllFiels(string term);
    Task<ICollection<JournalResult<JournalDocument>>> GetJournalsAllCondition(string term, double impactFactorMax, double impactFactorMin);
}