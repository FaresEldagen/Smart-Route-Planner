using Smart_Route_Planner.ViewModels;

namespace Smart_Route_Planner.Services
{
    public interface IRouteService
    {
        Task<List<ResultVM>> GetNearestApartmentsAsync(double lat, double lng, int limit = 5);
    }
}
