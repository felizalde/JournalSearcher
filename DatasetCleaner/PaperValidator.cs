using FluentValidation;

namespace DatasetCleaner;

public class PaperValidator : AbstractValidator<Paper>
{
    public List<string> _venues = new();
    public List<string> _dois = new();

    public PaperValidator()
    {
        RuleFor(p => p.title).NotEmpty();
        RuleFor(p => p.keywords).NotNull();
        RuleFor(p => p.Abstract).NotEmpty();
        RuleFor(p => p.lang).Equal("en");
        RuleFor(p => p.venue).NotNull().Must(v => v is not null && v.id is not null && IsValidVenue(v.id));
        RuleFor(p => p.url).NotEmpty().Must(u => u?.Any(x => IsValidURL(x)) ?? false);
    }

    private bool IsValidVenue(string id)
    {
        if (!_venues.Any()) return true;
        return _venues.Contains(id.ToLower());
    }

    // Is valid URL, and it contains the DOI prefix
    private bool IsValidURL(string url)
    {
        return _dois.Exists(d => url.Contains($"dx.doi.org/{d}/"));
    }

}