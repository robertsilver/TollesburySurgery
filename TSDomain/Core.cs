using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace TSDomain
{
    public class Core
    {
        /// <summary>
        /// Creates a new password for the user.  It will store the
        /// value back in the password file.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newPwd"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool CreateTemporaryPassword(string username, out string newPwd, out int userId)
        {
            string usernameFilename = AppSettings.AppSetting("Content");
            string pwdFilename = AppSettings.AppSetting("Footer");
            string images = AppSettings.AppSetting("Images");
            string usernameFile = File.ReadAllText(images + @"\" + usernameFilename);
            string pwdFile = File.ReadAllText(images + @"\" + pwdFilename);

            string unEncryptedPwd = "@Tr3mendo8s" + DateTime.Now.Minute.ToString();
            newPwd = unEncryptedPwd;
            LoginCheck login = new LoginCheck(username, usernameFile, pwdFile);
            userId = login.GetUserId();

            try
            {
                login.SaveTempPwd(images + @"\" + pwdFilename, Encrypt(unEncryptedPwd));
            }
            catch (Exception ex)
            {
                SaveErrorToFile("Core.cs", "CreateTemporaryPassword()", ex.Message);
                return false;
            }
            return true;
        }

        public static string Decrypt(string encrypted)
        {
            const string password = "#TreesEatBerriesAndMakeNewStones!";

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] pwdhash = hashmd5.ComputeHash(Encoding.ASCII.GetBytes(password));
            hashmd5 = null;

            //implement DES3 encryption
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider { Key = pwdhash, Mode = CipherMode.ECB };

            //the key is the secret password hash.

            //the mode is the block cipher mode which is basically the
            //details of how the encryption will work. There are several
            //kinds of ciphers available in DES3 and they all have benefits
            //and drawbacks. Here the Electronic Codebook cipher is used
            //which means that a given bit of text is always encrypted
            //exactly the same when the same password is used.
            //CBC, CFB
            byte[] buff = Convert.FromBase64String(encrypted);

            //decrypt DES 3 encrypted byte buffer and return ASCII string
            return Encoding.ASCII.GetString(
                des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length)
                );
        }

        public static string Encrypt(string original)
        {
            const string password = "#TreesEatBerriesAndMakeNewStones!";

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] pwdhash = hashmd5.ComputeHash(Encoding.ASCII.GetBytes(password));
            hashmd5 = null;

            //implement DES3 encryption
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider
                                                     {Key = pwdhash, Mode = CipherMode.ECB};

            //the key is the secret password hash.

            //the mode is the block cipher mode which is basically the
            //details of how the encryption will work. There are several
            //kinds of ciphers available in DES3 and they all have benefits
            //and drawbacks. Here the Electronic Codebook cipher is used
            //which means that a given bit of text is always encrypted
            //exactly the same when the same password is used.
            //CBC, CFB

            //----- encrypt an un-encrypted string ------------
            //the original string, which needs encrypted, must be in byte
            //array form to work with the des3 class. everything will because
            //most encryption works at the byte level so you'll find that
            //the class takes in byte arrays and returns byte arrays and
            //you'll be converting those arrays to strings.
            byte[] buff = Encoding.ASCII.GetBytes(original);

            //encrypt the byte buffer representation of the original string
            //and base64 encode the encrypted string. the reason the encrypted
            //bytes are being base64 encoded as a string is the encryption will
            //have created some weird characters in there. Base64 encoding
            //provides a platform independent view of the encrypted string 
            //and can be sent as a plain text string to wherever.
            return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
        }

        /// <summary>
        /// Retrieves the next Id.  If no value exits, then zero is returned.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static int GetNextId(XDocument xmlDoc, string childName)
        {
            int id = 0;

            if (!xmlDoc.Root.HasElements)
                return id;

            XElement lastDoc = xmlDoc.Descendants(childName).Last();

            if (null != lastDoc)
            {
                try
                {
                    id = Convert.ToInt32(lastDoc.FirstAttribute.Value);
                    id++;
                }
                catch
                {
                }
            }
            return id;
        }

        /// <summary>
        /// Saves an error the XML file.
        /// </summary>
        /// <param name="errorInFilename"></param>
        /// <param name="errorInMethodName"></param>
        /// <param name="error"></param>
        public static void SaveErrorToFile(string errorInFilename, string errorInMethodName, string error)
        {
            XDocument xmlDoc = XDocument.Load(AppSettings.AppSetting("ErrorLog"));

            int id = GetNextId(xmlDoc, "ErrorItem");

            #region Add new element
            XElement newDoc = xmlDoc.Descendants("ErrorItems").Last();

            newDoc.Add(
                new XElement("ErrorItem", // This is the value for the element.
                             new XAttribute("ID", id),
                             new XElement("ErrorInFilename", errorInFilename),
                             new XElement("ErrorInMethodName", errorInMethodName),
                             new XElement("DateAndTime", DateTime.UtcNow.ToString()),
                             new XElement("Error", error)));

            xmlDoc.Save(AppSettings.AppSetting("ErrorLog"));
            #endregion Add new element
        }

        public static void SetForgottenPwdFlagToYes(string username)
        {
            string usernameFilename = AppSettings.AppSetting("Content");
            string pwdFilename = AppSettings.AppSetting("Footer");
            string images = AppSettings.AppSetting("Images");
            string usernameFileContents = File.ReadAllText(images + @"\" + usernameFilename);
            string pwdFileContents = File.ReadAllText(images + @"\" + pwdFilename);

            LoginCheck login = new LoginCheck(username, usernameFileContents, pwdFileContents);
            login.GetUserId();

            login.SetForgottenPwdFlag(images + @"\" + usernameFilename, "Y");
        }
    }
}
