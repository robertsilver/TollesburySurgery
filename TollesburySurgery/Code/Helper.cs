
using System;
using System.Configuration;
using System.IO;

namespace TS
{
    public class Helper
    {
        public static string AppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static void SaveError(string errorInFilename, string info)
        {
            if (AppSetting("Error.Save").ToLower() == "true")
            {
                var filename = AppSetting("Error.Filename");

                using (var file = new StreamWriter(filename, true))
                {
                    file.WriteLine("Date: " + DateTime.Now + ". File: " + errorInFilename + ".  Desc: " + info + "\r\n===============================================================");
                }
            }
        }
    }
}