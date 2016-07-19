using System;
using System.IO;
using System.Web.Security;
using TollesburySurgery.Generic;
using TSDomain;

namespace TS.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void LoginButton_OnClick(Object sender, EventArgs e)
        {
            if (ForgottenPwd.Checked)
            {
                string newPwd = string.Empty;
                int userId = -1;
                if (CreateNewPwd(out newPwd, out userId))
                {
                    Core.SetForgottenPwdFlagToYes(UserName.Text);
                    int portNumber = AppSettings.AppSetting("Email.EmailPort") == string.Empty ? 0 : Convert.ToInt32(AppSettings.AppSetting("Email.EmailPort"));
                    Email em = new Email(
                        AppSettings.AppSetting("FromEmailAddress"), 
                        AppSettings.AppSetting("Email.EmailServer"),
                        portNumber, 
                        AppSettings.AppSetting("Email.BCCEmailAddress"),
                        Convert.ToBoolean(AppSettings.AppSetting("Email.AreWeOnLiveServer")));
                    string emailBody = File.ReadAllText(AppSettings.AppSetting("EmailText"));
                    emailBody = string.Format(emailBody, newPwd, "2");
                    em.EmailCustomer(emailBody, "robsilver@umail.net" /*UserName.Text*/, string.Empty, "New password");
                    lblError.Text = "We have sent you an email to " + UserName.Text +
                                    ", which contains your new password.";
                    lblError.Visible = true;
                    ForgottenPwd.Checked = false;
                }

                return;
            }

            string usernameFile = AppSettings.AppSetting("Content");
            string pwdFile = AppSettings.AppSetting("Footer");
            string images = AppSettings.AppSetting("Images");
            string usernameText = File.ReadAllText(images + @"\" + usernameFile);
            string pwdText = File.ReadAllText(images + @"\" + pwdFile);

            string encryptedPwd = Core.Encrypt(Password.Text);
            LoginCheck checkLogin = new LoginCheck(UserName.Text, encryptedPwd, usernameText, pwdText);
            if (checkLogin.IsUsernameValid != LoginCheck.LoginResult.Found)
            {
                lblError.Text = "Username not found";
                lblError.Visible = true;
                return;
            }

            switch (checkLogin.IsPwdValid)
            {
                case LoginCheck.LoginResult.IncorrectPassword:
                    lblError.Text = "Incorrect password entered";
                    lblError.Visible = true;
                    return;
                case LoginCheck.LoginResult.NotFound:
                    lblError.Text = "Password not found";
                    lblError.Visible = true;
                    return;
            }

            FormsAuthentication.RedirectFromLoginPage(UserName.Text, false);
        }

        #region Private methods

        private bool CreateNewPwd(out string newPwd, out int userId)
        {
            newPwd = string.Empty;
            userId = -1;
            if (string.IsNullOrEmpty(UserName.Text))
            {
                lblError.Text = "The user name cannot be blank.  Please try again.";
                lblError.Visible = true;
                return false;
            }

            if (!IsUserNameValid())
            {
                lblError.Text = "We cannot find your user name.  Please try again.";
                lblError.Visible = true;
                return false;
            }

            return Core.CreateTemporaryPassword(UserName.Text, out newPwd, out userId);
        }

        private bool IsUserNameValid()
        {
            return LoginCheck.LoginResult.Found == Generic.CreateLoginCheck(UserName.Text).IsUsernameValid;
        }
        #endregion Private methods
    }
}
