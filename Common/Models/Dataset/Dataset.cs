
using NpgsqlTypes;


namespace Common.Models.Dataset;

public record VenueInfo(
        [PgName("id")] string Id, 
        [PgName("displayname")] string DisplayName, 
        [PgName("normalizedname")] string NormalizedName);

public record PaperInfo(
        [PgName("id")] string Id,
        [PgName("title")] string Title,
        [PgName("venue_id")] string id,
        [PgName("year")] int Year,
        [PgName("keywords")] string Keywords,
        [PgName("abstract")] string Abstract,
        [PgName("urls")] string Urls,
        [PgName("lang")] string Lang,
        [PgName("source")] string Source
);

