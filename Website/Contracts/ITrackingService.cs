using Website.Models.Tracking;
namespace Website.Contracts;

public interface ITrackingService
{
    Task Save(Event data);
}