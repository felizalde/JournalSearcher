
using SearchEngine.Indices;
using SearchEngine.Repositories;

namespace SearchEngine.Interfaces;

public interface IJournalsRepository : IElasticBaseRepository<JournalDocument> { }