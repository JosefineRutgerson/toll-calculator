using System;
using System.Globalization;
using TollCalculatorAPI.Models;

namespace TollCalculatorAPI.Services
{
    public class TollCalculatorService : ITollCalculatorService
    {
      

        public void PopulateFeesForVehicle(Vehicle vehicle)
        {
            if (vehicle.SavedDates == null || vehicle.SavedDates.Count == 0)
                return;

            // Gruppera datum per dag
            var groupedByDay = vehicle.SavedDates
                .OrderBy(d => d.Date)
                .GroupBy(d => d.Date.Date);

            foreach (var dayGroup in groupedByDay)
            {
                var dates = dayGroup.OrderBy(d => d.Date).ToList();
                DateTime? windowStart = null;
                int windowMaxFee = 0;
                int dailyFee = 0;

                foreach (var vd in dates)
                {
                    var currentFee = GetTollFee(vd.Date, vehicle);

                    if (windowStart == null)
                    {
                        // Första tidpunkten i dagen
                        windowStart = vd.Date;
                        windowMaxFee = currentFee;
                        vd.Fee = 0; // We'll assign later
                        continue;
                    }

                    // kolla tidsskillnaden från windowStart
                    var minutes = (vd.Date - windowStart.Value).TotalMinutes;

                    if (minutes <= 60)
                    {
                        // Inom samma 60-minuters fönster
                        if (currentFee > windowMaxFee)
                            windowMaxFee = currentFee;

                        vd.Fee = 0; // så att vi bara debiterar en gång per fönster
                    }
                    else
                    {
                        // Slut på föregående fönster så tilldela dess avgift till det första datumet
                        var firstInWindow = dates.First(d => d.Date == windowStart.Value);
                        firstInWindow.Fee = windowMaxFee;

                        dailyFee += windowMaxFee;

                        // respektera daglig maxkostnad
                        if (dailyFee >= 60)
                        {
                            firstInWindow.Fee = Math.Max(0, 60 - (dailyFee - windowMaxFee));
                            // resterande datum blir noll
                            foreach (var rest in dates.Where(d => d.Date >= vd.Date))
                            rest.Fee = 0;
                            break;
                        }

                        // börja nytt fönster
                        windowStart = vd.Date;
                        windowMaxFee = currentFee;
                        vd.Fee = 0;
                    }
                }

                // stäng sista fönstret för dagen
                if (windowStart != null && dailyFee < 60)
                {
                    var firstInWindow = dates.First(d => d.Date == windowStart.Value);
                    firstInWindow.Fee = windowMaxFee;

                    dailyFee += windowMaxFee;

                    // Respektera daglig maxkostnad
                    if (dailyFee > 60)
                        firstInWindow.Fee -= (dailyFee - 60);
                }
            }
        }

        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle == null) return false;
            String vehicleType = vehicle.Type; // Ändrade så att det använder Type property
            if (string.IsNullOrEmpty(vehicleType)) return false;
            return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Military.ToString());
        }

        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            // Kollar om det är en avgiftsfri dag eller fordon
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) 
                return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            // Morgon
            if (hour == 6 && minute <= 29) return 8;
            if (hour == 6 && minute >= 30) return 13;
            if (hour == 7) return 18;
            if (hour == 8 && minute <= 29) return 13;

            // Lunch
            if ((hour == 8 && minute >= 30) || (hour >= 9 && hour <= 14)) return 8;

            // Eftermiddag
            if (hour == 15 && minute <= 29) return 13;
            if ((hour == 15 && minute >= 30) || hour == 16) return 18;
            if (hour == 17) return 13;
            if (hour == 18 && minute <= 29) return 8;

            // Kväll och natt
            return 0;
        }

        private bool IsTollFreeDate(DateTime date)
        {
            // Helger är alltid avgiftsfria
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
              return true;
              
            // Juli = avgiftsfri månad
            if (date.Month == 7)
            return true;

            // Endast hårdkodat för 2025
            if (date.Year != 2025)
                return false;

            // Röda dagar och aftnar i Sverige 2025
            var holidays2025 = new HashSet<(int Month, int Day)>
            {
                (1, 1),    // Nyårsdagen
                (1, 6),    // Trettondedag jul
                (4, 18),   // Långfredagen
                (4, 20),   // Påskdagen
                (4, 21),   // Annandag påsk
                (5, 1),    // Första maj
                (5, 29),   // Kristi himmelsfärdsdag
                (6, 6),    // Sveriges nationaldag
                (6, 20),   // Midsommarafton
                (6, 21),   // Midsommardagen
                (11, 1),   // Alla helgons dag
                (12, 24),  // Julafton
                (12, 25),  // Juldagen
                (12, 26),  // Annandag jul
                (12, 31)   // Nyårsafton

            };

            return holidays2025.Contains((date.Month, date.Day));
        }

        private enum TollFreeVehicles
        {
            Motorbike = 0,
            Tractor = 1,
            Emergency = 2,
            Diplomat = 3,
            Foreign = 4,
            Military = 5
        }
    }
}