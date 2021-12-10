using Nest;

namespace SearchEngine.Indices;

public class JournalDocument : BaseIndex
{
    [Text(Name = "title", Analyzer = "journals", Boost = 2)]
    public string? Title { get; set; }
    [Text(Name = "about", Analyzer = "journals", Boost = 1.5)]
    public string? About { get; set; }
    [Text(Name = "aims_and_scope", Analyzer = "journals", Boost = 1.75)]
    public string? AimsAndScope { get; set; }
    public string? Url { get; set; }
    public string? ImgUrl { get; set; }
    public string? Editorial { get; set; }
    public double? ImpactFactor { get; set; }
}

public class JournalDocumentBM25 : JournalDocument
{
    [Text(Name = "title", Analyzer = "journals", Boost = 2, Similarity = "BM25")]
    public new string? Title { get; set; }

    [Text(Name = "about", Analyzer = "journals", Boost = 1.5, Similarity = "BM25")]
    public new string? About { get; set; }
    [Text(Name = "aims_and_scope", Analyzer = "journals", Boost = 1.75, Similarity = "BM25")]
    public new string? AimsAndScope { get; set; }
}


public class JournalDocumentClassic : JournalDocument
{
    [Text(Name = "title", Analyzer = "journals", Boost = 2, Similarity = "classic")]
    public new string? Title { get; set; }

    [Text(Name = "about", Analyzer = "journals", Boost = 1.5, Similarity = "classic")]
    public new string? About { get; set; }

    [Text(Name = "aims_and_scope", Analyzer = "journals", Boost = 1.75, Similarity = "classic")]
    public new string? AimsAndScope { get; set; }
}