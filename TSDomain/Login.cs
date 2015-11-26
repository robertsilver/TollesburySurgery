using System;
using System.IO;

namespace TSDomain
{
    public class LoginCheck
    {
        // This value relates to the length of the following:
        // <value id="1" ForgottenPwd="Y"
        // So once the code has found the user email (joe@example.com), it must
        // then decrement the figure below, to find the user Id.
        private const int LengthOfIdAndForgottenPwd = 32;
        private const int LengthOfIdOnly = 14;
        private string _username { get; set; }
        private string _password { get; set; }
        private string _usernameTextToLookThrough { get; set; }
        private string _pwdTextToLookThrough { get; set; }

        public enum LoginResult
        {
            EmptyValue = 0,
            Found = 1,
            IncorrectPassword = 2,
            NotFound = 3
        }

        public LoginCheck(string username, string usernameTextToLookThrough, string pwdTextToLookThrough)
        {
            _pwdTextToLookThrough = pwdTextToLookThrough;
            _usernameTextToLookThrough = usernameTextToLookThrough;
            _username = username;
        }

        public LoginCheck(string username, string password, string usernameTextToLookThrough, string pwdTextToLookThrough)
        {
            _password = password;
            _pwdTextToLookThrough = pwdTextToLookThrough;
            _usernameTextToLookThrough = usernameTextToLookThrough;
            _username = username;            
        }

        public bool DoesUserAccountHaveForgottenFlagSet { get { return DoesUserHaveForgottenPwdFlagSet(); } }

        public LoginResult IsPwdValid
        {
            get
            {
                if (string.IsNullOrEmpty(_password))
                    return LoginResult.EmptyValue;

                int userIdIndex = _pwdTextToLookThrough.IndexOf("id=\"" + UserId + "\"");
                int index = _pwdTextToLookThrough.IndexOf(_password, userIdIndex);
                if (index >= 0)
                {
                    int userId = -1;
                    userId = GetUserId(index - LengthOfIdOnly, _pwdTextToLookThrough);
                    if (-1 == userId)
                        return LoginResult.NotFound;

                    return UserId == userId ? LoginResult.Found : LoginResult.NotFound;
                }
                return LoginResult.IncorrectPassword;
            }
        }

        public LoginResult IsUsernameValid
        {
            get
            {
                if (string.IsNullOrEmpty(_username))
                    return LoginResult.EmptyValue;

                int index = _usernameTextToLookThrough.IndexOf(_username);
                if (index >= 0)
                {
                    int userId = -1;
                    userId = GetUserId(index - LengthOfIdAndForgottenPwd, _usernameTextToLookThrough);
                    if (userId == -1)
                        return LoginResult.NotFound;

                    UserId = userId;
                    return LoginResult.Found;
                }

                return LoginResult.NotFound;
            }
        }

        public int UserId { get; set; }

        #region Public methods
        /// <summary>
        /// Returns the user's Id
        /// </summary>
        /// <returns></returns>
        public int GetUserId()
        {
             int index = _usernameTextToLookThrough.IndexOf(_username);
             if (index >= 0)
             {
                 UserId = GetUserId(index - LengthOfIdAndForgottenPwd, _usernameTextToLookThrough);
             }

             return UserId;
        }

        /// <summary>
        /// Returns the user name for the current user Id.
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            int index = _usernameTextToLookThrough.IndexOf("<value id=\"" + UserId + "\"");
            string username = string.Empty;
            if (index >= 0)
            {
                int secondGreaterThanSymbol = _usernameTextToLookThrough.IndexOf("<", index + (LengthOfIdAndForgottenPwd - 1));
                if (secondGreaterThanSymbol >= 0)
                    username = _usernameTextToLookThrough.Substring(index + (LengthOfIdAndForgottenPwd - 1), secondGreaterThanSymbol - (LengthOfIdAndForgottenPwd - 1));
            }

            return username;
        }

        /// <summary>
        /// Saves the new password into the file.  Returns true if successful
        /// otherwise false.
        /// </summary>
        /// <param name="pwdFilePath"></param>
        /// <param name="oldEncryptedPwd"></param>
        /// <param name="newEncryptedPwd"></param>
        /// <returns></returns>
        public bool SaveNewPwd(string pwdFilePath, string oldEncryptedPwd, string newEncryptedPwd)
        {
            int index = _pwdTextToLookThrough.IndexOf("<value id=\"" + UserId + "\">");

            if (index < 0)
                return false;

            int indexOfLastGreaterThan = _pwdTextToLookThrough.IndexOf(">", index + LengthOfIdOnly);

            // Extract the entire user info, e.g. <value id="2">93lcVFytiTQ=</value>
            string oldPwdText = _pwdTextToLookThrough.Substring(index, (indexOfLastGreaterThan + 1) - index);

            // Replace the old pwd, with the new, e.g.
            // from: <value id="2">93lcVFytiTQ=</value>
            // to:   <value id="2">O+lkiwrhjvw=</value>
            string newPwdTextToSave = oldPwdText.Replace(oldEncryptedPwd, newEncryptedPwd);

            // Now replace the old pwd text in the entire file, with the new pwd text.
            _pwdTextToLookThrough = _pwdTextToLookThrough.Replace(oldPwdText, newPwdTextToSave);

            File.WriteAllText(pwdFilePath, _pwdTextToLookThrough);

            return true;
        }

        /// <summary>
        /// Saves the temporary password in the file.
        /// </summary>
        /// <param name="pwdFilePath"></param>
        /// <param name="newEncryptedPwd"></param>
        /// <returns></returns>
        public bool SaveTempPwd(string pwdFilePath, string newEncryptedPwd)
        {
            int index = _pwdTextToLookThrough.IndexOf("<value id=\"" + UserId + "\">");

            if (index < 0)
                return false;

            int indexOfLastGreaterThanClosingTag = _pwdTextToLookThrough.IndexOf(">", index);

            int indexOfClosingTag = _pwdTextToLookThrough.IndexOf("</value>", indexOfLastGreaterThanClosingTag);

            string oldPwd = _pwdTextToLookThrough.Substring(indexOfLastGreaterThanClosingTag + 1, (indexOfClosingTag - 1) - indexOfLastGreaterThanClosingTag);

            // Extract the entire user info, e.g. <value id="2">93lcVFytiTQ=</value>
            int indexOfLastGreaterThanClosingTagAfterUserId = _pwdTextToLookThrough.IndexOf(">", index + LengthOfIdOnly);
            string oldPwdText = _pwdTextToLookThrough.Substring(index, (indexOfLastGreaterThanClosingTagAfterUserId + 1) - index);

            // Replace the old pwd, with the new, e.g.
            // from: <value id="2">93lcVFytiTQ=</value>
            // to:   <value id="2">O+lkiwrhjvw=</value>
            string newPwdTextToSave = oldPwdText.Replace(oldPwd, newEncryptedPwd);

            // Now replace the old pwd text in the entire file, with the new pwd text.
            _pwdTextToLookThrough = _pwdTextToLookThrough.Replace(oldPwdText, newPwdTextToSave);

            try
            {
                File.WriteAllText(pwdFilePath, _pwdTextToLookThrough);
            }
            catch (Exception ex)
            {
                Core.SaveErrorToFile("Login.cs", "SaveTempPwd", ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the forgotten password flag, within the user file,
        /// to the value that's passed in.
        /// </summary>
        /// <param name="usernameFilePath"></param>
        /// <param name="newFlagValue"></param>
        /// <returns></returns>
        public bool SetForgottenPwdFlag(string usernameFilePath, string newFlagValue)
        {
            int index = _usernameTextToLookThrough.IndexOf("<value id=\"" + UserId + "\" ForgottenPwd=");

            if (index < 0)
                return false;

            int indexOfLastGreaterThan = _usernameTextToLookThrough.IndexOf(">", index + LengthOfIdOnly);

            // Extract the entire user info, e.g. <value id="2" ForgottenPwd="Y">93lcVFytiTQ=</value>
            string entireUserDetails = _usernameTextToLookThrough.Substring(index, (indexOfLastGreaterThan + 1) - index);

            // Replace the old pwd, with the new, e.g.
            // from: <value id="2">93lcVFytiTQ=</value>
            // to:   <value id="2">O+lkiwrhjvw=</value>
            string oldFlagValue = "N";
            if (newFlagValue.ToUpper() == "N")
                oldFlagValue = "Y";

            string newPwdTextToSave = entireUserDetails.Replace("ForgottenPwd=\"" + oldFlagValue + "\"", "ForgottenPwd=\"" + newFlagValue + "\"");

            // Now replace the old pwd text in the entire file, with the new pwd text.
            _usernameTextToLookThrough = _usernameTextToLookThrough.Replace(entireUserDetails, newPwdTextToSave);

            try
            {
                File.WriteAllText(usernameFilePath, _usernameTextToLookThrough);
            }
            catch (Exception ex)
            {
                Core.SaveErrorToFile("Login.cs", "SaveTempPwd", ex.Message);
                return false;
            }

            return true;
        }

        #endregion Public methods

        #region Private methods
        /// <summary>
        /// Returns true if the forgotten password flag has been set to Y.
        /// </summary>
        /// <returns></returns>
        private bool DoesUserHaveForgottenPwdFlagSet()
        {
            int index = _usernameTextToLookThrough.IndexOf("<value id=\"" + UserId);
            if (index >= 0)
            {
                return IsForgottenPwdFlagSetToTrue(index, _usernameTextToLookThrough);
            }
            return false;
        }

        private int GetUserId(int indexOfUsername, string textToLookIn)
        {
            int firstQuote = textToLookIn.IndexOf('"', indexOfUsername);
            int secondQuote = textToLookIn.IndexOf('"', firstQuote + 1);
            string userId = textToLookIn.Substring((firstQuote + 1), (secondQuote - 1) - firstQuote);
            int uId;
            if (!Int32.TryParse(userId, out uId))
                uId = -1;
            return uId;
        }

        private bool IsForgottenPwdFlagSetToTrue(int indexOfUserId, string textToLookIn)
        {
            int forPwd = textToLookIn.IndexOf("ForgottenPwd", indexOfUserId);
            if (-1 == forPwd)
                return false;

            int firstQuote = textToLookIn.IndexOf('"', forPwd);
            int secondQuote = textToLookIn.IndexOf('"', firstQuote + 1);
            string forgottenPwdFlag = textToLookIn.Substring((firstQuote + 1), (secondQuote - 1) - firstQuote);
            return "Y".ToLower() == forgottenPwdFlag.ToLower();
        }

        #endregion Private methods
    }
}
