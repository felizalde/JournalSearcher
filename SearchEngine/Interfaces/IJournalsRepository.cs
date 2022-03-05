
using SearchEngine.Indices;
using SearchEngine.Repositories;

namespace SearchEngine.Interfaces;

public interface IJournalsRepository<T> : IElasticBaseRepository<T> where T : BaseIndex {
    public string IndexName { get; set; }
 }