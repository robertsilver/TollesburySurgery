using System;
using TollesburySurgery.Generic;
using TSDomain;

namespace TS.Account
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated &&
                null == Request.QueryString["u"])
            {
                lblError.Text = "You're not authorised to view this page.  Please log in.";
                lblError.Visible = true;
                ChangePasswordPushButton.Visible = false;
                return;
            }
            userName.Value = Page.User.Identity.Name;
            ChangePasswordPushButton.Visible = true;
        }

        protected void CancelPushButton_OnClick(Object sender, EventArgs e)
        {
            Response.Redirect("/Default.aspx", true);
        }

        protected void ChangePasswordPushButton_OnClick(Object sender, EventArgs e)
        {
            if (!AreBothPwdsIdentical(NewPassword.Text, ConfirmNewPassword.Text))
            {
                lblError.Text = "Sorry, but we cannot verify your current password.  Please try again.";
                lblError.Visible = true;
                return;
            }

            LoginCheck save = Generic.CreateLoginCheck(Page.User.Identity.Name);

            bool resetForgottenPwdFlag = false;
            if (null != Request.QueryString["u"])
            {
                // The user is not logged in, so store the user Id.
                save.UserId = Convert.ToInt32(Request.QueryString["u"]);

                // Reset the flag, because the user has arrived
                // at this page from a forgotten password email link.
                resetForgottenPwdFlag = true;
            }
            else
                // User's logged in, but we need to get their Id for the save object.
                save.GetUserId();

            string images = AppSettings.AppSetting("Images");
            string pwdFile = AppSettings.AppSetting("Footer");
            bool successfulSave = save.SaveNewPwd(images + @"\" + pwdFile, Core.Encrypt(CurrentPassword.Text),
                                                  Core.Encrypt(ConfirmNewPassword.Text));

            if (successfulSave)
            {
                string username = save.GetUserName();
                Core.SetForgottenPwdFlagToYes(username);
                Response.Redirect("/Default.aspx");
            }
            else
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Pwd change",
                                                       "alert('Your password was not saved.  Please try again.')", true);
        }

        #region Private methods
        private bool AreBothPwdsIdentical(string newPwd, string confirmationPwd)
        {
            return newPwd == confirmationPwd;
        }

        #endregion Private methods
    }
}
