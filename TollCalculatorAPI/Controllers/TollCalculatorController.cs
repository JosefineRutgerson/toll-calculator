using Microsoft.AspNetCore.Mvc;
using TollCalculatorAPI.Models;
using TollCalculatorAPI.Services;

namespace TollCalculatorAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TollCalculatorController : ControllerBase
    {
        private readonly ITollCalculatorService _tollCalculatorService;
        private readonly IUserRepository _repo;

        public TollCalculatorController(ITollCalculatorService tollCalculatorService, IUserRepository repo)
        {
            _tollCalculatorService = tollCalculatorService;
            _repo = repo;
        }

        [HttpGet("calculateWholeFee/{name}")]
        public ActionResult<int> CalculateWholeMonthlyTollFee(string name)
        {
            //Hämtar aktuell användare
            var user = _repo.GetUserByName(name);
            if (user == null)
            {
                return NotFound("User not found");
            }
            // Hämtar användarens fordon och dess data
            var vehicles = user?.Vehicles;
            if (vehicles == null || vehicles.Count == 0)
            {
                return NotFound("No vehicles found for the user");
            }

            // Fordonens registreringsnummer och dess sparade datum
            Dictionary<string, List<VehicleDate>> allDates = GetUserVehicleDatesByVehicle(user);
            // Fordonens vehicle typ.
            List<string> vehicleTypes = vehicles.Select(v => v.GetVehicleType()).ToList();

            // Beräknar den totala avgiften för alla fordon
            int totalFee = 0;
            foreach (var vehicle in vehicles)
            {
                if (allDates.TryGetValue(vehicle.RegistrationNumber, out var vehicleDates))
                {
                    DateTime[] dates = vehicleDates.Select(vd => vd.Date).ToArray();
                    totalFee += _tollCalculatorService.GetTollFee(vehicle, dates);
                }
            }


            return totalFee;

        }

        // Hämtar den totala avgiften för ett specifikt fordon baserat på registreringsnummer
        [HttpGet("calculateWholeFeePerCar/{name}/{regNumber}")]
        public ActionResult<int> CalculateWholeMonthlyTollFeePerCar(string name, string regNumber)
        {
            //Hämtar aktuell användare
            var user = _repo.GetUserByName(name);
            if (user == null)
            {
                return NotFound("User not found");
            }
            // Hämtar användarens fordon och dess data
            var vehicles = user?.Vehicles;
            if (vehicles == null || vehicles.Count == 0)
            {
                return NotFound("No vehicles found for the user");
            }

            // Fordonets sparade datum baserat på registreringsnummer
            List<VehicleDate> allDates = GetUserVehicleDatesPerVehicle(user, regNumber);
            if (allDates == null || allDates.Count == 0)
            {
                return NotFound("No dates found for the vehicle");
            }
            // Beräknar den totala avgiften för fordonet
            int totalFee = 0;            
            if (allDates != null && allDates.Count > 0)
            {
                DateTime[] dates = allDates.Select(vd => vd.Date).ToArray();
                var vehicle = vehicles.FirstOrDefault(v => v.RegistrationNumber == regNumber);
                if (vehicle != null)
                {
                    totalFee = _tollCalculatorService.GetTollFee(vehicle, dates);
                }
            }

            return totalFee;

        }

        public Dictionary<string, List<VehicleDate>> GetUserVehicleDatesByVehicle(User user)
        {
            return user.Vehicles
                       .ToDictionary(v => v.RegistrationNumber, v => v.SavedDates);
        }

        public List<VehicleDate> GetUserVehicleDatesPerVehicle(User user, string regNumber)
        {
            return user.Vehicles
                       .Where(v => v.RegistrationNumber == regNumber)
                       .SelectMany(v => v.SavedDates)
                       .ToList();
        }
    }
}