using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculatorAPI.Models 
{
    public class Vehicle : IVehicle
    {
        public int Id { get; set; }
        public string Type { get; set; } = "";
        public List<VehicleDate> SavedDates { get; set; } = new();
        public virtual String GetVehicleType()
        {
            return "Vehicle";
        }
    }
}