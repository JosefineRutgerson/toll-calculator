using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculatorAPI.Models 
{
    public class Diplomat : Vehicle
    {
        public Diplomat()
        {
            Type = "Diplomat";
        }
        public override string GetVehicleType()
        {
            return "Diplomat";
        }
    }
}
