
using Nest;
using SearchEngine.Indices;
using SearchEngine.Interfaces;

namespace SearchEngine.Repositories;

public interface IJournalsRepository : IElasticBaseRepository<JournalDocument> { }

public class JournalsRepository: ElasticBaseRepository<JournalDocument>, IJournalsRepository
{
    public JournalsRepository(IElasticClient elasticClient): base(elasticClient)
    {
    }

    public override string IndexName => IndexMapping.GetIndexName(typeof(JournalDocumentClassic));
}


public class JournalsBM25Repository : ElasticBaseRepository<JournalDocument>, IJournalsRepository
{
    public JournalsBM25Repository(IElasticClient elasticClient) : base(elasticClient)
    {
    }

    public override string IndexName => IndexMapping.GetIndexName(typeof(JournalDocumentBM25));
}

