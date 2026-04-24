using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Smart_Route_Planner.Models;
using Smart_Route_Planner.Services;
using Smart_Route_Planner.ViewModels;

namespace Smart_Route_Planner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRouteService _routeService;

        public HomeController(ILogger<HomeController> logger, IRouteService routeService)
        {
            _logger = logger;
            _routeService = routeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Result(double lat, double lng)
        {
            var apt_list = await _routeService.GetNearestApartmentsAsync(lat, lng);
            return View(apt_list);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
