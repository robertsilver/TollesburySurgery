using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TollesburySurgery.DataAccess;

namespace TollesburySurgery.Business
{
    public class SurgeryClosed
    {
        public static string GetAll(string url)
        {
            var results = SurgeryClosedData.GetAll(url);

            if (!results.Any())
                return null;

            var resultData = new StringBuilder();

            foreach (var r in results)
            {
                var totalDays = (r.DateClosed.Date - DateTime.Now.Date).TotalDays;
                if (totalDays >=0 && totalDays <= r.ShowDaysBefore)
                {
                    var suffix = GetSuffix(r.DateClosed.Day.ToString());
                    resultData.AppendLine(r.Text.Replace("[date]", $"{r.DateClosed.ToString("dddd dd")}{suffix}{r.DateClosed.ToString(" MMMM yyyy")}"));
                }
            }

            return resultData.ToString();
        }

        private static string GetSuffix(string day)
        {
            string suffix = "th";

            if (int.Parse(day) < 11 || int.Parse(day) > 20)
            {
                day = day.ToCharArray()[day.ToCharArray().Length - 1].ToString();
                switch (day)
                {
                    case "1":
                        suffix = "st";
                        break;
                    case "2":
                        suffix = "nd";
                        break;
                    case "3":
                        suffix = "rd";
                        break;
                }
            }

            return suffix;
        }
    }
}
