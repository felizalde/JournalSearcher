using Common.Models;

namespace Experiments.Model;


public record ExperimentSetting(
    string Query,
    List<RefineItem> Refines
    );
public record ExperimentInput(
    string Title,
    string Abstract,
    string Keywords,
    Guid JournalId,
    string JournalTitle
);


public record ExperimentResultDocument(string Title, string Id, double? Score);

public record ExperimentResult(
    Guid Id,
    ExperimentInput Input,
    List<ExperimentResultDocument> Result,
    MatchExperimentType MatchType
);

public enum MatchExperimentType
{
    First,
    Top5,
    Top10,
    Top50,
    NoMatch
}
