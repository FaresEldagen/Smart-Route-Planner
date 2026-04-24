using Smart_Route_Planner.Repositories;
using Smart_Route_Planner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Route_Planner.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;

        public RouteService(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<List<ResultVM>> GetNearestApartmentsAsync(double lat, double lng, int limit = 5)
        {
            var nodes = await _routeRepository.GetAllNodesAsync();
            var edges = await _routeRepository.GetAllEdgesAsync();
            var apartments = await _routeRepository.GetAllApartmentsWithNodesAsync();

            var sourceNode = nodes.OrderBy(n => Math.Pow(n.Lat - lat, 2) + Math.Pow(n.Lng - lng, 2)).FirstOrDefault();

            if (sourceNode == null)
            {
                return new List<ResultVM>();
            }

            var adjacencyList = new Dictionary<long, List<(long target, double weight)>>();
            foreach (var edge in edges)
            {
                if (!adjacencyList.ContainsKey(edge.FromNodeId))
                    adjacencyList[edge.FromNodeId] = new List<(long target, double weight)>();
                adjacencyList[edge.FromNodeId].Add((edge.ToNodeId, edge.Distance));
                
                if (!adjacencyList.ContainsKey(edge.ToNodeId))
                    adjacencyList[edge.ToNodeId] = new List<(long target, double weight)>();
                adjacencyList[edge.ToNodeId].Add((edge.FromNodeId, edge.Distance));
            }

            var distances = new Dictionary<long, double>();
            foreach (var node in nodes)
            {
                distances[node.Id] = double.MaxValue;
            }
            distances[sourceNode.Id] = 0;

            var priorityQueue = new PriorityQueue<long, double>();
            priorityQueue.Enqueue(sourceNode.Id, 0);

            while (priorityQueue.Count > 0)
            {
                var currentId = priorityQueue.Dequeue();
                var currentDist = distances[currentId];

                if (currentDist > distances[currentId]) continue;

                if (adjacencyList.TryGetValue(currentId, out var neighbors))
                {
                    foreach (var neighbor in neighbors)
                    {
                        var newDist = currentDist + neighbor.weight;
                        if (newDist < distances[neighbor.target])
                        {
                            distances[neighbor.target] = newDist;
                            priorityQueue.Enqueue(neighbor.target, newDist);
                        }
                    }
                }
            }

            return apartments
                .Where(a => distances.ContainsKey(a.NodeId) && distances[a.NodeId] != double.MaxValue)
                .Select(a => new ResultVM
                {
                    Id = a.Id,
                    Name = a.Name,
                    Type = a.Type,
                    Latitude = a.Lat,
                    Longitude = a.Lng,
                    Distance = Math.Round(distances[a.NodeId], 2),
                    Price = a.Price
                })
                .OrderBy(a => a.Distance)
                .Take(limit)
                .ToList();
        }
    }
}
