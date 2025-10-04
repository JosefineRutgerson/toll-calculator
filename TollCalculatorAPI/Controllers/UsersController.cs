using Microsoft.AspNetCore.Mvc;
using TollCalculatorAPI.Models;
using TollCalculatorAPI.Services;

namespace TollCalculatorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly ITollCalculatorService _tollCalculatorService;


        public UsersController(IUserRepository repo, ITollCalculatorService tollCalculatorService)
        {
            _repo = repo;
            _tollCalculatorService = tollCalculatorService;

        }

        //Hämtar mina användare och deras fordon från mockdatan
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            var users = _repo.GetUsers();
            var result = users.Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                Vehicles = u.Vehicles,

            }).ToList();

            return result;
        }

        [HttpGet("byname/{name}")]
        public ActionResult<User> GetUserByName(string name)
        {
            var user = _repo.GetUserByName(name);
            if (user == null)
                return NotFound();

            DateTime today = DateTime.Today;
            int currentYear = today.Year;
            int currentMonth = today.Month;

            foreach (var vehicle in user.Vehicles)
            {
                // Sätt alla avgifter för fordonet
                _tollCalculatorService.PopulateFeesForVehicle(vehicle);

                // Adderara alla avgifter för innevarande månad
                vehicle.CurrentMonthlyFee = vehicle.SavedDates
                .Where(d => d.Date.Year == currentYear && d.Date.Month == currentMonth)
                .Sum(d => d.Fee);

                // Hämta alla datum för innevarande månad
                vehicle.SavedDatesCurrentMonth = vehicle.SavedDates
                .Where(d => d.Date.Year == currentYear && d.Date.Month == currentMonth)
                .ToList();
            }

            // Addera alla avgifter för användaren
            user.CurrentMonthlyFee = user.Vehicles.Sum(v => v.CurrentMonthlyFee);

            return user;
        }
        
        [HttpGet("{name}/vehicles/{regNumber}")]
        public ActionResult<Vehicle> GetVehicleByUserAndRegNumber(string name, string regNumber)
        {
            var user = _repo.GetUserByName(name);
            
            if (user == null)
                return NotFound();

            DateTime today = DateTime.Today;
            int currentYear = today.Year;
            int currentMonth = today.Month;


            var vehicle = user.Vehicles
                .FirstOrDefault(v => v.RegistrationNumber.Equals(regNumber, StringComparison.OrdinalIgnoreCase));

            if (vehicle == null) 
                return NotFound($"Vehicle '{regNumber}' not found for user '{name}'");

            // Sätt alla avgifter för fordonet
            _tollCalculatorService.PopulateFeesForVehicle(vehicle);

            // Adderara alla avgifter för innevarande månad
            vehicle.CurrentMonthlyFee = vehicle.SavedDates
                .Where(d => d.Date.Year == currentYear && d.Date.Month == currentMonth)
                .Sum(d => d.Fee);

            // Hämta alla datum för innevarande månad
            vehicle.SavedDatesCurrentMonth = vehicle.SavedDates
                .Where(d => d.Date.Year == currentYear && d.Date.Month == currentMonth)
                .ToList();


            return vehicle;
        }

        

    }
}

