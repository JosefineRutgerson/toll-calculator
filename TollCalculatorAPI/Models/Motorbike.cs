using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculatorAPI.Models
{
    public class Motorbike : Vehicle
    {
        public Motorbike()
        {
            Type = "Motorbike";
        }
        public override string GetVehicleType()
        {
            return "Motorbike";
        }
    }
}
 

