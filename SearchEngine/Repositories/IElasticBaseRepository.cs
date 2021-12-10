using Nest;
using SearchEngine.Indices;
using SearchEngine.Interfaces;

namespace SearchEngine.Repositories;

public interface IElasticBaseRepository<T> where T : BaseIndex
{
    Task<T?> GetAsync(string id);
    Task<T?> GetAsync(IGetRequest request);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<JournalResult<T>>> SearchAsync(Func<SearchDescriptor<T>, ISearchRequest> selector);
    Task<IEnumerable<JournalResult<T>>> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> request);
    Task<ISearchResponse<T>> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> request, Func<AggregationContainerDescriptor<T>, IAggregationContainer> aggregationsSelector);
    Task<bool> CreateIndexAsync();
    Task<bool> InsertAsync(T t);
    Task<bool> InsertManyAsync(IList<T> tList);
    Task<bool> UpdateAsync(T t);
    Task<bool> UpdatePartAsync(T t, object partialEntity);
    Task<long> GetTotalCountAsync();
    Task<bool> DeleteByIdAsync(string id);
    Task<bool> DeleteByQueryAsync(Func<QueryContainerDescriptor<T>, QueryContainer> selector);
    Task<bool> DeleteAllAsync(IEnumerable<T> documents);
    Task<bool> ExistAsync(string id);
}