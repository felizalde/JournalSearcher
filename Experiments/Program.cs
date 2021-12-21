using Common;
using Common.Models;
using Experiments.Model;
using Microsoft.Extensions.Configuration;
using Nest;
using SearchEngine;
using SearchEngine.Indices;
using SearchEngine.Interfaces;
using SearchEngine.Repositories;
using System.Text.Json;

IConfiguration Configuration =
    new ConfigurationBuilder()
        .AddJsonFile(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\Website\appsettings.Development.json"), optional: true, reloadOnChange: true)
        .AddCommandLine(Environment.GetCommandLineArgs())
        .Build();

JournalsRecommenderData Data = new(Configuration);


ElasticClient CreateElasticClient(IConfiguration configuration) 
{
    var settings = new ConnectionSettings(new Uri(configuration["ElasticsearchSettings:uri"]));
    var basicAuthUser = configuration["ElasticsearchSettings:username"];
    var basicAuthPassword = configuration["ElasticsearchSettings:password"];

    if (!string.IsNullOrEmpty(basicAuthUser) && !string.IsNullOrEmpty(basicAuthPassword))
        settings = settings.BasicAuthentication(basicAuthUser, basicAuthPassword);

    return new ElasticClient(settings);
}


List<ExperimentInput> LoadInitialInputs(string query)
{
    return Data.ExecQuery<ExperimentInput>(query).ToList();
}


async Task<List<ExperimentResult>> RunExperiments(List<ExperimentInput> inputs, List<RefineItem> refines)
{
    var output = new List<ExperimentResult>();
    IElasticClient elasticClient = CreateElasticClient(Configuration);
    var repository = new JournalsRepository(elasticClient);
    var searcher = new JournalSearcher(repository);

    foreach(var input in inputs)
    {
        var results = await searcher.GetJournals(input.Title, input.Abstract, input.Keywords, refines);
        output.Add(CreateExperimentResult(input, results.ToList()));
    }

    return output;

}

ExperimentResult CreateExperimentResult(ExperimentInput input, List<JournalResult<JournalDocument>> results)
{
    var indexOf = results.FindIndex(item => item.Document.Id == input.JournalId.ToString());

    MatchExperimentType type = MatchExperimentType.NoMatch;
    switch (indexOf)
    {
        case < 0: type = MatchExperimentType.NoMatch; break;
        case 0 : type = MatchExperimentType.First; break;
        case < 5: type = MatchExperimentType.Top5; break;
        case < 10: type = MatchExperimentType.Top10; break;
        case < 20: type = MatchExperimentType.Top20; break;
    }

    return new ExperimentResult(Guid.NewGuid(), input, results, type);
}

async Task Main() {
    var settingFile = Configuration["path"];
    var fileContent = File.ReadAllText(settingFile);
    var setting =  JsonSerializer.Deserialize<ExperimentSetting>(fileContent, new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    });
    var inputs = LoadInitialInputs(setting.Query);
    var results = await RunExperiments(inputs, setting.Refines);

    var result = JsonSerializer.Serialize(results);

    Console.WriteLine("Found in top 1: {0}", results.Where(x => x.MatchType == MatchExperimentType.First).Count());
    Console.WriteLine("Found in top 5: {0}", results.Where(x => x.MatchType == MatchExperimentType.Top5).Count());
    Console.WriteLine("Found in top 10: {0}", results.Where(x => x.MatchType == MatchExperimentType.Top10).Count());
    Console.WriteLine("Found in top 20: {0}", results.Where(x => x.MatchType == MatchExperimentType.Top20).Count());
    Console.WriteLine("Not Found: {0}", results.Where(x => x.MatchType == MatchExperimentType.NoMatch).Count());
}

await Main();