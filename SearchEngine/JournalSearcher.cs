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

    private static FieldsDescriptor<JournalDocument> FillFieldsDescriptor(FieldsDescriptor<JournalDocument> descriptor, IEnumerable<RefineField> fields)
    {
        foreach (var field in fields)
        {
            switch (field.Name)
            {
                case "Title": descriptor.Field(j => j.Title, field.Boost); break;
                case "About": descriptor.Field(j => j.About, field.Boost); break;
                case "Aims and Scope": descriptor.Field(j => j.AimsAndScope, field.Boost); break;
                case "Keywords": descriptor.Field(j => j.Keywords, field.Boost); break;
            }
        }

        return descriptor;
    }

    private static MultiMatchQueryDescriptor<JournalDocument> FillMultiMatchQueryDescriptor(MultiMatchQueryDescriptor<JournalDocument> descriptor, 
                                                            string title, string _abstract, string keywords, 
                                                            IEnumerable<RefineItem> refines)
    {
        foreach (var refine in refines)
        {
            switch (refine.Title)
            {
                case "Paper Title": descriptor.Query(title); break;
                case "Paper Abstract": descriptor.Query(_abstract); break;
                case "Paper Keywords": descriptor.Query(keywords); break;
            }
            descriptor.Analyzer("journals").Fields(f => FillFieldsDescriptor(f, 
                                                                refine.Fields.Where(x => x.Active)
                                                                )).Type(TextQueryType.MostFields);
        }

        return descriptor;
    }

    public async Task<ICollection<JournalResult<JournalDocument>>> GetJournals(string title, string _abstract, string keywords, IEnumerable<RefineItem> refines) 
    {
        var query = new QueryContainerDescriptor<JournalDocument>()
                        .DisMax(d =>
                        {
                            //d.TieBreaker(0.7); Maybe is not fair, since not all journals contains the same fields. 
                            d.Queries(dq =>
                               dq.MultiMatch(m =>
                                FillMultiMatchQueryDescriptor(m, title, _abstract, keywords, refines)
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