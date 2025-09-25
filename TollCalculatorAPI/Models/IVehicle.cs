using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculatorAPI.Models 
{
    public interface IVehicle
    {
        string Type { get; set; }
        List<VehicleDate> SavedDates { get; set; }
        String GetVehicleType();
    }
}