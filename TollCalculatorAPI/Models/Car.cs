using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculatorAPI.Models 
{
    public class Car : Vehicle
    {
        public Car()
        {
            Type = "Car";
        }
        public override String GetVehicleType()
        {
            return "Car";
        }
    }
}