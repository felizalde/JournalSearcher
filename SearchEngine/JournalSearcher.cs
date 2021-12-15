using Common.Models;
using Nest;
using SearchEngine.Abstractions;
using SearchEngine.Indices;
using SearchEngine.Interfaces;
using SearchEngine.Repositories;

namespace SearchEngine;

public class JournalSearcher : IJournalSearcher
{
    private readonly IJournalsRepository repository;

    public JournalSearcher(IJournalsRepository repository)
    {
        this.repository = repository;
    }
    public async Task InsertManyAsync(IEnumerable<Journal> journals)
    {
        var documents = (journals.Select(x => new JournalDocumentBM25
        {
            Id = x.Id.ToString(),
            Title = x.Title,
            About = x.About,
            AimsAndScope = x.AimsAndScope,
            Editorial = x.Editorial,
            Url = x.Url,
            ImgUrl = x.ImgUrl,
            ImpactFactor = x.ImpactFactor,
            Metrics =  x.Metrics?.Select(y => 
                                    new JournalMetricDocument() { 
                                        Id = y.Id.ToString(), 
                                        JournalId = x.Id.ToString(), 
                                        Name = y.Name, 
                                        Value = y.Value
                                    }).ToList() ?? new()
        }) as IEnumerable<JournalDocument>).ToList();

        await repository.InsertManyAsync(documents);
    }

    public async Task<ICollection<JournalDocument>> GetAllAsync()
    {
        var result = await repository.GetAllAsync();

        return result.ToList();
    }

    public async Task<ICollection<JournalResult<JournalDocument>>> SearchInAllFiels(string term)
    {
        var query = NestExtensions.BuildMultiMatchQuery<JournalDocument>(term);
        var result = await repository.SearchAsync(_ => query);

        return result.ToList();
    }

    public async Task<ICollection<JournalResult<JournalDocument>>> GetJournalsAllCondition(string term, double impactFactorMax, double impactFactorMin)
    {
        var query = new QueryContainerDescriptor<JournalDocument>()
                        .CombinedFields(c =>
                           c.Fields(f => f.Field(p => p.Title, 3)
                                                .Field(p => p.AimsAndScope, 2)
                                                .Field(p => p.About, 1)
                                                )
                           .Query(term)
                           .Operator(Operator.Or)
                           .ZeroTermsQuery(ZeroTermsQuery.All)
                           .Name("combined_fields")
                           .AutoGenerateSynonymsPhraseQuery(true)
                            );
                        

        var result = await repository.SearchAsync(_ => query);

        return result.ToList();
    }

    public async Task CleanIndexAsync()
    {
        var documents = await repository.GetAllAsync();
        await repository.DeleteAllAsync(documents);
    }
}