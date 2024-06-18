using F1LapsGame.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace F1LapsGame.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LaptimeContext _context;

        public HomeController(ILogger<HomeController> logger, LaptimeContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("ChooseYear", "LapGame");
        }

        [HttpGet]
        public async Task<IActionResult> Drivers(int? pageNumber)
        {
            var drivers = from r in _context.Drivers select r;
            drivers = drivers.OrderBy(d => d.DriverId);

            var pageSize = 200;

            return View(await DataViewPage<Driver>.CreateAsync(drivers.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Races(int? pageNumber)
        {
            var races = from r in _context.Races select r;
            races = races.OrderByDescending(r => r.Year).ThenBy(r => r.Round);

            var pageSize = 200;

            return View(await DataViewPage<Race>.CreateAsync(races.AsNoTracking(), pageNumber ?? 1, pageSize)); 
        }

        [HttpGet]
        public async Task<IActionResult> Laptimes(int? pageNumber)
        {
            var laptimes = from lt in _context.Laptimes select lt;
            laptimes = laptimes.OrderByDescending(r => r.RaceId);

            var pageSize = 500;

            return View(await DataViewPage<Laptime>.CreateAsync(laptimes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Constructors(int? pageNumber)
        {
            var constructors = from lt in _context.Constructors select lt;
            constructors = constructors.OrderBy(c => c.ConstructorId);

            var pageSize = 100;

            return View(await DataViewPage<Constructor>.CreateAsync(constructors.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Results(int? pageNumber)
        {
            var results = from lt in _context.Results select lt;
            results = results.OrderByDescending(r => r.RaceId);

            var pageSize = 200;

            return View(await DataViewPage<Result>.CreateAsync(results.AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
