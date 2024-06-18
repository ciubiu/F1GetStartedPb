using F1LapsGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

namespace F1LapsGame.Controllers;

public class LapGameController : Controller
{
    private readonly LaptimeContext _context;

    public LapGameController(LaptimeContext context)
    {
        _context = context;
    }

    public IActionResult ChooseYear()
    {
        var years = _context.Races
            .Select(r => r.Year)
            .Distinct()
            .OrderByDescending(r => r)
            .ToList();

        return View(years);
    }

    // year
    public IActionResult ChooseRace(int id)
    {
        var races = _context.Races
            .Where(r => r.Year == id)
            .OrderBy(r => r.RaceId)
            .ToList();

        HttpContext.Session.SetInt32("raceYear", id);

        return View(races);
    }

    // raceid
    [HttpGet]
    public IActionResult StartingPosition(int id)
    {
        Guid newGuid = Guid.NewGuid();
        HttpContext.Session.SetString("0", newGuid.ToString());
        TempData["NextLapGuid"] = newGuid;

        var race = _context.Races
            .FirstOrDefault(r => r.RaceId == id);

        HttpContext.Session.SetString("raceLocation", race.Name);
        HttpContext.Session.SetInt32("raceId", race.RaceId);
        HttpContext.Session.SetInt32("raceLap", 0);
        TempData["IsStartingRace"] = true;


        // resultId,raceId,driverId,constructorId,number,grid
        var gridResults = _context.Results
            .Where(r => r.RaceId == id)
            .Include(r => r.Driver)
            .Include(r => r.Constructor)
            .OrderBy(r => r.Grid == 0 ? int.MaxValue : r.Grid)
            .ToList();

        return View(gridResults);
    }


    // raceid?lap_guid=xx
    public IActionResult NextLap(int id)
    {
        int currentDisplayedLap = 6;  // we can go back and forth
        string? urlGuid = Request.Query["guid"];

        // Check if the guid is valid and stored in the session state
        if (string.IsNullOrEmpty(urlGuid))
        {
            TempData["Message"] = "No completed race lap was selected";
            return RedirectToAction("Index", "Home");
        }

        foreach (var key in HttpContext.Session.Keys)
        {
            var storedGuid = HttpContext.Session.GetString(key);
            if (storedGuid == urlGuid)
            {
                currentDisplayedLap = int.Parse(key.ToString());
                break;
            }
        }

        // generate lap nr and guid for next lap
        Guid nextLapGuid = Guid.NewGuid();
        int nextDisplayedLap = currentDisplayedLap + 1;
        HttpContext.Session.SetString(nextDisplayedLap.ToString(), nextLapGuid.ToString());
        TempData["NextLapGuid"] = nextLapGuid;

        HttpContext.Session.SetInt32("raceLap", currentDisplayedLap);


        List<Laptime>? laptimesForView = new();

        var lapsToShow = 10;
        var fromLap = (currentDisplayedLap > lapsToShow) ? (currentDisplayedLap - lapsToShow) : 1;
        var toLap = currentDisplayedLap;
        var showingLaps = (currentDisplayedLap <= lapsToShow) ? currentDisplayedLap : lapsToShow;

        // we need to display starting position too as lap 0

        List<Laptime>? gridLap = new();
        // we have exact same query in StartingPosition
        List<Result>? startingGrid = _context.Results
        .Where(r => r.RaceId == id)
        .Include(r => r.Driver)
        .Include(r => r.Constructor)
        .OrderBy(r => r.Grid == 0 ? int.MaxValue : r.Grid)
        .ToList();

        // convert to type List<Laptime>
        // raceId,driverId,lap,position,time,milliseconds
        foreach (var result in startingGrid)
        {
            var position = new Laptime
            {
                RaceId = result.RaceId,
                DriverId = result.DriverId,
                Lap = 0,
                Position = result.Grid,
                Time = string.Empty,
                Milliseconds = 0
            };
            gridLap.Add(position);
        }


        var lapsRaced = _context.Laptimes
        .Where(l => (l.RaceId == id) && (l.Lap <= currentDisplayedLap))
        .Include(l => l.Driver)
        .OrderBy(l => l.Lap).ThenBy(l => l.Position)
        .ToList();

        laptimesForView = gridLap.Concat(lapsRaced).ToList();

        // 1st lap has most drivers
        //int driversMax = laptimesForView
        //    .Where(l => l.Lap == 1)
        //    .Count();



        // check, if we getting any elements exception from Linq query here
        var latestLap = laptimesForView.Max(l => l.Lap);
        var latestLapResults = laptimesForView
            .Where(l => l.Lap == latestLap)
            .ToList();

        // previousLapOrder<driverId, position>
        var previousLapOrder = new Dictionary<int, int>();

        // save current driver positions with driver id to directory
        if (currentDisplayedLap > 0)
        {
            // get driver positions from previous lap
            var previousLapResults = laptimesForView
                .Where(l => l.Lap == currentDisplayedLap - 1)
                .ToList();

            foreach (var driver in previousLapResults)
            {
                previousLapOrder.Add(driver.DriverId ?? 0, driver.Position);
            }
        }
        else
        {
            // get driver positions from starting grid
            foreach (var driver in gridLap)
            {
                previousLapOrder.Add(driver.DriverId ?? 0, driver.Position);
            }
        }


        List<LapPosition> currentDriversPositions = new List<LapPosition>();

        // save current driver positions in order of lastet lap
        foreach (var driver in latestLapResults)
        {
            // raced laps + starting grid column
            LapPosition driverPosition = new LapPosition(latestLap + 1);
            driverPosition.CurrentPosition = driver.Position;
            driverPosition.DriverId = driver.DriverId ?? 0;
            driverPosition.DriverLaptimesInMs[latestLap] = driver.Milliseconds;
            currentDriversPositions.Add(driverPosition);
        }

        // populate now with all previous lap times
        // we have already latest lap and lap 0 comes from starting grid
        // kui on 1. ring ajad ainult, siis seda for lauset ei käivitata !!
        for (int lap = 1; lap < latestLap; lap++)
        {
            // filter only for X lap
            var oneLapLaptimes = laptimesForView
                .Where(ol => ol.Lap == lap)
                .ToList();

            // lets get next laptime, search for driver position and enter the time into matrix
            // got all laptimes from one lap
            // we have driverID, save the driver ms to correct matrix in list of lap position
            foreach (var lt in oneLapLaptimes)
            {
                // get driver position from currentLapOrder and save to List<LapPosition>
                // search for driver's position and enter laptime to laptime matrix
                var driver = lt.DriverId;
                var driverTime = lt.Milliseconds;

                // if driver id found, save milliseconds to time matrix
                foreach (var entry in currentDriversPositions)
                {
                    if (entry.DriverId == driver)
                    {
                        entry.DriverLaptimesInMs[lap] = driverTime;
                    }
                    if (entry.DriverId == driver && lap == latestLap - 1)
                    {
                        entry.PreviousLapPosition = previousLapOrder.FirstOrDefault(d => d.Key == driver).Value;
                    }
                }
            }
        }

        foreach (var entry in currentDriversPositions)
        {
            
                entry.PreviousLapPosition = previousLapOrder.FirstOrDefault(d => d.Key == entry.DriverId).Value;
            
        }



        return View(currentDriversPositions);
    }

}
