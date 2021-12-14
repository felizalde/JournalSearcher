using System.ComponentModel.DataAnnotations;

namespace Website.Models.Tracking;

public record EventRequest ([Required] EventType Type, [Required] string Data);