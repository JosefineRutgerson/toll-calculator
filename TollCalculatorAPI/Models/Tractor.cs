using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculatorAPI.Models 
{
    public class Tractor : Vehicle
    {
        public Tractor()
        {
            Type = "Tractor";
        }
        public override String GetVehicleType()
        {
            return "Tractor";
        }
    }
}
