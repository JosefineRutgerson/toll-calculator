using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculatorAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<Vehicle> Vehicles { get; set; } = new();         

        public int CurrentMonthlyFee { get; set; } = 0;
    }
}