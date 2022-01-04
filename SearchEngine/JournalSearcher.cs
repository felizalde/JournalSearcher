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
            Metrics = x.Metrics?.Select(y =>
                                   new JournalMetricDocument()
                                   {
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

    private static IEnumerable<Field> CreateFields(IEnumerable<RefineField> fields)
    {
        var output = new List<Field>();
        foreach (var field in fields)
        {
            switch (field.Name)
            {
                case "Title": output.Add(new Field("title", field.Boost)); break;
                case "About": output.Add(new Field("about", field.Boost)); break;
                case "Aims and Scope": output.Add(new Field("aims_and_scope", field.Boost)); break;
                case "Keywords": output.Add(new Field("keywords", field.Boost)); break;
            }
        }

        return output;
    }

    private static IEnumerable<QueryContainer> CreateQueries(string title, string _abstract, string keywords, IEnumerable<RefineItem> refines)
    {
        var queries = new List<QueryContainer>();
        foreach (var refine in refines)
        {
            switch (refine.Title)
            {
                case "Paper Title":
                    queries.Add(new MultiMatchQuery
                    {
                        Name = "title_query",
                        Query = title,
                        Fields = CreateFields(refine.Fields.Where(f => f.Active)).ToArray(),
                        Analyzer = "journals",
                        Type = TextQueryType.MostFields

                    });
                    break;
                case "Paper Abstract":
                    queries.Add(new MultiMatchQuery
                    {
                        Name = "abstract_query",
                        Query = _abstract,
                        Fields = CreateFields(refine.Fields.Where(f => f.Active)).ToArray(),
                        Analyzer = "journals",
                        Type = TextQueryType.MostFields

                    });
                    break;
                case "Paper Keywords":
                    queries.Add(new MultiMatchQuery
                    {
                        Name = "keywords_query",
                        Query = keywords,
                        Fields = CreateFields(refine.Fields.Where(f => f.Active)).ToArray(),
                        Analyzer = "journals",
                        Type = TextQueryType.MostFields

                    });
                    break;
            }
        }

        return queries;
    }

    public async Task<ICollection<JournalResult<JournalDocument>>> GetJournals(string title, string _abstract, string keywords, IEnumerable<RefineItem> refines)
    {

        var query = new DisMaxQuery()
        {
            Name = "dynamic_query",
            //TieBreaker = 0.11, //->TODO:Review! Maybe is not fair, since not all journals contains the same fields. 
            Queries = CreateQueries(title, _abstract, keywords, refines)
        };
        
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