using Common.Models;
using Nest;
using SearchEngine.Abstractions;
using SearchEngine.Indices;
using SearchEngine.Interfaces;

namespace SearchEngine;

public class JournalSearcher : IJournalSearcher<JournalDocument>
{
    private readonly IJournalsRepository<JournalDocument> repository;

    public JournalSearcher(IJournalsRepository<JournalDocument> repository)
    {
        this.repository = repository;
    }
    public async Task InsertManyAsync(IEnumerable<Journal> journals)
    {

        var documents = journals.Select(x => new JournalDocument
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
        }).ToList();

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

    public async Task<ICollection<JournalResult<JournalDocument>>> GetJournalsAllCondition(string title, string _abstract, string keywords)
    {

        var query = new QueryContainerDescriptor<JournalDocument>()
                        .DisMax(d => {
                            d.TieBreaker(0.7);
                            d.Queries(dq => 
                                dq.MultiMatch(m => 
                                    m.Query(title)
                                        .Fields(f => 
                                            f.Field(p => p.Title, 2)
                                            .Field(p => p.AimsAndScope)
                                            .Field(p => p.About))
                                        .Analyzer("journals")
                                        //.Fuzziness(Fuzziness.Auto)
                                    //    .ZeroTermsQuery(ZeroTermsQuery.All)
                                    .Query(_abstract)
                                        .Fields(f => 
                                            f.Field(p => p.Title)
                                            .Field(p => p.AimsAndScope, 1.5)
                                            .Field(p => p.About))
                                        .Analyzer("journals")
                                        //.Fuzziness(Fuzziness.Auto)
                                    //    .ZeroTermsQuery(ZeroTermsQuery.All)
                                    // .Query(keywords)
                                    //     .Fields(f => 
                                    //         f.Field(p => p.Title, 1.5)
                                    //         .Field(p => p.AimsAndScope)
                                    //         .Field(p => p.About))
                                    //     //.Analyzer("journals")
                                    //     //.Fuzziness(Fuzziness.Auto)
                                    //     .ZeroTermsQuery(ZeroTermsQuery.All)
                                )
                                
                            );
                            return d;
                        });
                        

        var result = await repository.SearchAsync(_ => query);

        return result.ToList();
    }

    public async Task CleanIndexAsync()
    {
        var documents = await repository.GetAllAsync();
        await repository.DeleteAllAsync(documents);
    }

    public async Task CreateIndexAsync()
    {
        await repository.CreateIndexAsync();
    }
}