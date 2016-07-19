using System;
using System.Text;
using TSDomain;

namespace TollesburySurgery.Code
{
    public partial class FriendsAndFamily : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected virtual void Submit_OnClick(Object sender, EventArgs e)
        {
            if (Qu1.SelectedValue == string.Empty)
            {
                textboxMessage.Text = "Error#Please tell us why you would recommend Tollesbury Surgery.  Thank you.";
                return;
            }

            if (string.IsNullOrEmpty(Reason.Text.Trim()))
            {
                textboxMessage.Text = "Error#Please tell us why you chose that answer.  Thank you.";
                return;
            }

            var portNumber = AppSettings.AppSetting("Email.EmailPort") == string.Empty ? 0 : Convert.ToInt32(AppSettings.AppSetting("Email.EmailPort"));

            var email = new Email(
                AppSettings.AppSetting("Email.FromEmailAddress"),
                AppSettings.AppSetting("Email.EmailServer"),
                portNumber,
                AppSettings.AppSetting("Email.BCCEmailAddress"),
                Convert.ToBoolean(AppSettings.AppSetting("Email.AreWeOnLiveServer")));

            var body = new StringBuilder();
            switch(Qu1.SelectedValue)
            {
                case "ExtremelyLikely":
                    body.AppendLine("<strong>Answer 1</strong>: Extremely likely" + "<br /><br />");
                    break;
                case "NeitherLikelyUnlikely":
                    body.AppendLine("<strong>Answer 1</strong>: Neither likely or unlikely" + "<br /><br />");
                    break;
                case "Unlikely":
                    body.AppendLine("<strong>Answer 1</strong>: Unlikely" + "<br /><br />");
                    break;
                case "ExtremelyUnlikely":
                    body.AppendLine("<strong>Answer 1</strong>: Extremely unlikely" + "<br /><br />");
                    break;
                case "DontKnow":
                    body.AppendLine("<strong>Answer 1</strong>: Don't know" + "<br /><br />");
                    break;
            }
            body.AppendLine("<strong>Reason</strong>: " + Reason.Text + "<br /><br />");

            if (string.IsNullOrEmpty(ContactDetails.Text))
                body.AppendLine("<strong>Contact details</strong>: not provided");
            else
                body.AppendLine("<strong>Contact details</strong>: " + ContactDetails.Text);

            var result = email.EmailCustomer(body.ToString(), AppSettings.AppSetting("Email.ToEmailAddress"), string.Empty, "Friends and family test");

            if (result)
            {
                textboxMessage.Text = "Thank you for your answers.";
                Qu1.SelectedIndex = -1;
                Reason.Text = string.Empty;
                ContactDetails.Text = string.Empty;
            }
            else
                textboxMessage.Text = "Error#There was a problem.  Please try again.";
        }
    }
}