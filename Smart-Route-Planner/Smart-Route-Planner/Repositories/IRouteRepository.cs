using Smart_Route_Planner.Models;

namespace Smart_Route_Planner.Repositories
{
    public interface IRouteRepository
    {
        Task<List<Node>> GetAllNodesAsync();
        Task<List<Edge>> GetAllEdgesAsync();
        Task<List<Apartment>> GetAllApartmentsWithNodesAsync();
    }
}
