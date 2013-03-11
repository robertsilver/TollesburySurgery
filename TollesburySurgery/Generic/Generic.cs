using System.IO;
using TSDomain;

namespace TollesburySurgery.Generic
{
    public class Generic
    {
        public static LoginCheck CreateLoginCheck(string userName)
        {
            #region Extract username & password details from files

            string usernameFile = AppSettings.AppSetting("Content");
            string pwdFile = AppSettings.AppSetting("Footer");
            string images = AppSettings.AppSetting("Images");
            string usernameText = File.ReadAllText(images + @"\" + usernameFile);
            string pwdText = File.ReadAllText(images + @"\" + pwdFile);
            #endregion Extract username & password details from files

            return new LoginCheck(userName, usernameText, pwdText);
        }
    }
}