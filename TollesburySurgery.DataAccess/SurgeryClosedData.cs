using System;
using System.Collections.Generic;

namespace TollesburySurgery.DataAccess
{
    public class SurgeryClosedData
    {
        public DateTime DateClosed { get; set; }
        public int ShowDaysBefore { get; set; }
        public string Text { get; set; }

        public static List<SurgeryClosedData> GetAll(string url)
        {
            return ReadJson.ReadTheJson<SurgeryClosedData>(url);
        }
    }
}
