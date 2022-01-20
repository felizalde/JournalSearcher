
using Nest;
using SearchEngine.Indices;
using SearchEngine.Interfaces;

namespace SearchEngine.Repositories;


public class JournalsRepository: ElasticBaseRepository<JournalDocument>, IJournalsRepository<JournalDocument>
{
    public JournalsRepository(IElasticClient elasticClient): base(elasticClient)
    {
    }

    public override string IndexName => "journals-index_springer";
}


