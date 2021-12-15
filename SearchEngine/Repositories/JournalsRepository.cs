
using Nest;
using SearchEngine.Indices;
using SearchEngine.Interfaces;

namespace SearchEngine.Repositories;


public class ClassicJournalsRepository: ElasticBaseRepository<JournalDocument>, IJournalsRepository
{
    public ClassicJournalsRepository(IElasticClient elasticClient): base(elasticClient)
    {
    }

    public override string IndexName => IndexMapping.GetIndexName(typeof(JournalDocumentClassic));
}


public class BM25JournalsRepository : ElasticBaseRepository<JournalDocument>, IJournalsRepository
{
    public BM25JournalsRepository(IElasticClient elasticClient) : base(elasticClient)
    {
    }

    public override string IndexName => IndexMapping.GetIndexName(typeof(JournalDocumentBM25));
}

