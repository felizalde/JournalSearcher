using FluentValidation;

namespace DatasetCleaner;


public record Paper(string id, string title, Venue venue,
    int year, string[] keywords, string[] fos, int n_citation,
    string[] references, string doc_type, string lang, string publisher,
    string[] url, string Abstract);

public record Venue(string raw, string id);
