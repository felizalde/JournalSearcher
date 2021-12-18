using System.ComponentModel.DataAnnotations;
using Common.Models;

namespace Website.Models.Search;

public record SearchRequest([Required] string Title, 
    [Required] string Abstract, 
    List<string> Keywords, 
    List<RefineItem> Setting);
