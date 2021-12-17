using Nest;

namespace SearchEngine.Indices;


public class JournalDocument : BaseIndex 
{
    [Text(Name = "title", Analyzer = "journals", Boost = 3, Similarity = "BM25")]
    public string? Title { get; set; }

    [Text(Name = "about", Analyzer = "journals", Boost = 1, Similarity = "BM25")]
    public string? About { get; set; }

    [Text(Name = "aims_and_scope", Analyzer = "journals", Boost = 2, Similarity = "BM25")]
    public string? AimsAndScope { get; set; }

    [Text(Name = "keywords", Analyzer = "journals", Boost = 1.5, Similarity = "BM25")]
    public string Keywords { get; set; }
    public string? Url { get; set; }
    public string? ImgUrl { get; set; }
    public string? Editorial { get; set; }
    public double? ImpactFactor { get; set; }
    public List<JournalMetricDocument> Metrics { get; set; }

}
public class JournalMetricDocument : BaseIndex
{
    [Text(Name = "journalid", Index = false)]
    public string JournalId { get; set; }
    
    [Text(Name = "metric_name", Index = false)]
    public string Name { get; set; }

    [Text(Name = "metric_value", Index = false)]
    public double Value { get; set; }
}