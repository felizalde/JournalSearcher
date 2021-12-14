using System.ComponentModel.DataAnnotations;

namespace Website.Models.Search;

public class SearchRequest
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Abstract { get; set; }
    public List<string> Keywords { get; set; }
    public MinMaxValue ImpactFactor { get; set; }

}

public class MinMaxValue
{
    public double Min { get; set; }

    public double Max { get; set; }

}