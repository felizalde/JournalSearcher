using System.Diagnostics;
using DatasetCleaner;
using Common.Models.Dataset;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Common;


//Set up local data
int MAX_ENTRIES = 1_000_000;
var validator = new PaperValidator();

// dois prefix: https://gist.github.com/TomDemeranville/8699224 
var dois = "10.1251,10.1186,10.4076,10.1114,10.1023,10.5819,10.1361,10.1379,10.1065,10.1381,10.7603,10.1385,10.4098,10.3758,10.1617,10.5052,10.1245,10.4333,10.1365,10.1891,10.1140,10.1007,10.3322,10.2966,10.1892,10.1359,10.2755,10.1034,10.1113,10.1111,10.1046,10.1348,10.1506,10.3162,10.2746,10.1516,10.1301,10.1002,10.3405,10.1196,10.3170,10.3401,10.1581,10.1576,10.1256,10.1526,10.1897,10.5054,10.4004";
validator._dois.AddRange(dois.Split(','));
var validPapers = new List<Paper>();
var venuesIds = new List<string>();

void LoadVenuesIds()
{
    var json = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "venues-ids.json"));
    venuesIds = JsonConvert.DeserializeObject<List<string>>(json);

    validator._venues = venuesIds;
}


IConfiguration configuration =
    new ConfigurationBuilder()
        .AddJsonFile(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\Website\appsettings.Development.json"), optional: true, reloadOnChange: true)
        .AddCommandLine(Environment.GetCommandLineArgs())
        .Build();


// Read all files below a folder
IEnumerable<string> ReadFilesInDirectory(string folder)
{
    var files = Directory.GetFiles(folder);
    foreach (var file in files)
    {
        yield return file;
    }
}

// Read all Papers in a file
void CalculateValidPaperInFile(string fileName, int numberOfThreads = 8)
{

    foreach (var line in File.ReadLines(fileName).AsParallel().WithDegreeOfParallelism(numberOfThreads))
    {
        if (validPapers?.Count > MAX_ENTRIES) break;

        var paper = JsonConvert.DeserializeObject<Paper>(line);

        if (paper != null)
        {
            var result = validator?.Validate(paper);
            if (result.IsValid)
            {
                validPapers.Add(paper);

            }
        }
    }
}

// Read all venus in a file.
IEnumerable<VenueInfo> ReadVenuesInFile(string fileName)
{
    foreach (var line in File.ReadLines(fileName))
    {
        var venue = JsonConvert.DeserializeObject<VenueInfo>(line);
        if (venue != null)
        {
            yield return venue;
        }
    }
}

void WriteResultsInFile<T>(string fileName, List<T> results)
{
    var json = JsonConvert.SerializeObject(results);
    File.WriteAllText(fileName, json);
}


void CalculateValidPapersInFolder(string folder)
{
    var files = ReadFilesInDirectory(folder);

    foreach (var file in files)
    {
        if (file.EndsWith(".json")) continue;

        CalculateValidPaperInFile(file);
    }

    WriteResultsInFile(Path.Combine(folder, "papers-results.json"), validPapers);
}


void LoadVenueList(string fileName)
{

    var venues = ReadVenuesInFile(fileName);

    var db = new JournalsRecommenderData(configuration);

    db.InsertBulkVenues(venues);
}


void GenerateValidVenues(string path)
{
    var db = new JournalsRecommenderData(configuration);
    var venues = db.GetAllVenueInfoExistingInJournal().ToList();

    WriteResultsInFile(Path.Combine(path, "venues-ids.json"), 
      venues.Select( v => v.Id).ToList());
} 


void ImportPaperFile(string json)
{

    var content = File.ReadAllText(json);
    var papers = JsonConvert.DeserializeObject<Paper[]>(content);

    var db = new JournalsRecommenderData(configuration);
    var papersInfo = papers.Select(p => 
                        new PaperInfo(
                            p.id,
                            p.title, 
                            p.venue.id, 
                            p.year, 
                            string.Join(",", p.keywords),
                            p.Abstract,
                            string.Join(",", p.url),
                            p.lang,
                            "aminer")).ToList();
    db.InsertBulkPaper(papersInfo);
}


void Main()
{
    // Start time span
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    try
    {
        switch (configuration["mode"])
        {
            case "import-venues":
                LoadVenueList(configuration["path"]);
                break;
            case "generate-valid-venues":
                GenerateValidVenues(configuration["path"]);
                break;
            case "validate-papers":
                LoadVenuesIds();
                CalculateValidPapersInFolder(configuration["path"]);
                Console.WriteLine($"{validPapers.Count} papers are valid");
                break;
            case "import-paper":
                ImportPaperFile(configuration["path"]);
                break;
            default:
                Console.Write("Please, indicate a valid execution mode. The available modes are: \n --mode=\"import-venue\" \n --mode=\"validate-venues\" \n --mode=\"validate-papers\" \n --mode=\"import-paper\"");
                break;
            
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
        Console.WriteLine($"Time elapsed: {elapsedTime}");
    }
}


Main();
