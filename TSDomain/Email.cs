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
        private readonly string _fromEmailAddress = string.Empty;
        private readonly bool _onLiveServer;

        public Email(string fromMailAddress, string EmailServer, int portNumber, string bccEmail, bool onLiveServer)
        {
            _bccEmail = bccEmail;
            //_emailHostName = emailHostName;
            _portNumber = portNumber;
            _emailServer = EmailServer;
            _fromEmailAddress = fromMailAddress;
            _onLiveServer = onLiveServer;
        }

        public bool EmailCustomer(string emailBodyText, string ToEmailAddress, string CCMailAddress, string subject)
        {
            if (string.IsNullOrEmpty(_fromEmailAddress))
                return false;

            try
            {
                MailAddress fromMail = new MailAddress(_fromEmailAddress);
                MailAddress toMail = new MailAddress(ToEmailAddress);

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

                var client = new SmtpClient(_emailServer, _portNumber);

                if (!_onLiveServer)
                {
                    // We need to provide the credentials if we're not on the live server.
                    client.Credentials = new System.Net.NetworkCredential("blackhole@tollesburysurgery.co.uk", "ogaziechi");
                    if (_portNumber != 0)
                        client.Port = _portNumber;
                }

                // Send the email
                client.Send(msgDetails);
                return true;
            }
            catch(Exception ex)
            {
                Core.SaveErrorToFile("Email.cs", "EmailCustomer()", ex.Message);
                return false;
            }
        }

        #region Private methods
       

        #endregion private methods
    }
}
