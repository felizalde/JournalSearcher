using System.ComponentModel.DataAnnotations;

namespace Website.Models.Tracking;
public record Event ([Required] EventType type, [Required] string Data, [Required] DateTime CreatedAt, [Required] Guid CreatedBy);
public enum EventType
{
    NewQuery,
    OpenResult,
    NextPage
}