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


public record ExperimentResult (
    Guid Id,
    ExperimentInput Input,
    object Result,
    MatchExperimentType MatchType
);

public enum MatchExperimentType 
{
    First,
    Top5,
    Top10,
    Top20,
    NoMatch
}
