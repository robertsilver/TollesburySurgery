using System;
using System.Net.Mail;

namespace TSDomain
{
    public class Email
    {
        private readonly string _bccEmail = string.Empty;
        //private string _emailHostName = string.Empty;
        private readonly int _portNumber = 0;
        private readonly string _emailServer = string.Empty;
        private readonly string _fromMailAddress = string.Empty;

        public Email(string fromMailAddress, string emailServer, int portNumber, string bccEmail)
        {
            _bccEmail = bccEmail;
            //_emailHostName = emailHostName;
            _portNumber = portNumber;
            _emailServer = emailServer;
            _fromMailAddress = fromMailAddress;
        }

        public void EmailCustomer(string emailBodyText, string toEmailAddress, string CCMailAddress, string subject)
        {
            if (string.IsNullOrEmpty(_fromMailAddress))
                return;

            try
            {
                MailAddress fromMail = new MailAddress(_fromMailAddress);
                MailAddress toMail = new MailAddress(toEmailAddress);

                MailMessage msgDetails = new MailMessage(fromMail, toMail)
                {
                    Body = emailBodyText,
                    IsBodyHtml = true,
                    Subject = subject
                };

                #region Add the CC mail addresses
                if (CCMailAddress.Contains(";"))
                {
                    string[] ccMailAddresses = CCMailAddress.Split(';');
                    foreach (string c in ccMailAddresses)
                        msgDetails.CC.Add(c);
                }
                else if (!string.IsNullOrEmpty(CCMailAddress))
                    msgDetails.CC.Add(CCMailAddress);
                #endregion Add the CC mail addresses

                #region Add the BCC mail addresses
                if (_bccEmail.Contains(";"))
                {
                    string[] bccMailAddresses = _bccEmail.Split(';');
                    foreach (string b in bccMailAddresses)
                        msgDetails.Bcc.Add(b);
                }
                else if (!string.IsNullOrEmpty(_bccEmail))
                    msgDetails.Bcc.Add(_bccEmail);
                #endregion Add the BCC mail addresses

                SmtpClient client = new SmtpClient(_emailServer, _portNumber);
                if (_portNumber != 0)
                    client.Port = _portNumber;

                // Send the email
                client.Send(msgDetails);
            }
            catch(Exception ex)
            {
                Core.SaveErrorToFile("Email.cs", "EmailCustomer()", ex.Message);
            }
        }

        #region Private methods
       

        #endregion private methods
    }
}
