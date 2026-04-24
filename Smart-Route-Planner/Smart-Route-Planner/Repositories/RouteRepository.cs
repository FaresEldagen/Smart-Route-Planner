using Microsoft.EntityFrameworkCore;
using Smart_Route_Planner.Data;
using Smart_Route_Planner.Models;

namespace Smart_Route_Planner.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly AppDbContext _context;

        public RouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Node>> GetAllNodesAsync()
        {
            return await _context.Nodes.ToListAsync();
        }

        public async Task<List<Edge>> GetAllEdgesAsync()
        {
            return await _context.Edges.ToListAsync();
        }

        public async Task<List<Apartment>> GetAllApartmentsWithNodesAsync()
        {
            return await _context.Apartments.Include(a => a.Node).ToListAsync();
        }
    }
}
