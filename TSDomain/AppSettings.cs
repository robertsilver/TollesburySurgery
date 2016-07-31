using System.Configuration;

namespace TSDomain
{
    public class AppSettings
    {
        public static string AppSetting(string key)
        {
            string keyToGet = string.Empty;

            string computerName = string.Empty;

            try
            {
                computerName = ConfigurationManager.AppSettings["ComputerName"];
            }
            catch
            {

            }

            keyToGet = computerName + " " + key;

            string ret = ConfigurationManager.AppSettings[keyToGet];

            if (ret != null)
            {
                return ret;
            }
            else
            {
                ret = ConfigurationManager.AppSettings[key];
                if (ret == null) ret = "";
                return ret;
            }
        }
    }
}
