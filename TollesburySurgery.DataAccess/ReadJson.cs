using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace TollesburySurgery.DataAccess
{
    public class ReadJson
    {
        public static List<T> ReadTheJson<T>(string url) where T : class, new()
        {
            List<T> result;

            var json = string.Empty;

            using (WebClient wc = new WebClient())
            {
                json = wc.DownloadString(url);
            }

            result = JsonConvert.DeserializeObject<List<T>>(json);

            return result;
        }
    }
}
