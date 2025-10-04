using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculatorAPI.Models;

namespace TollCalculatorAPI.Services
{
    public interface ITollCalculatorService
    {
        void PopulateFeesForVehicle(Vehicle vehicle);
        int GetTollFee(DateTime date, Vehicle vehicle);
    }
}