using System;
using System.IO;
using System.Web.Services;
using TSDomain;

namespace TollesburySurgery
{
    public partial class WebMethods : System.Web.UI.Page
    {
        public class ReturnData
        {
            public string Text = string.Empty;
            public string Title = string.Empty;
        }

        [WebMethod]
        public static int CheckUserNameForLogin(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return -1;

            string usernameFile = AppSettings.AppSetting("Content");
            string pwdFile = AppSettings.AppSetting("Footer");
            string images = AppSettings.AppSetting("Images");
            string usernameText = File.ReadAllText(images + @"\" + usernameFile);
            string pwdText = File.ReadAllText(images + @"\" + pwdFile);

            LoginCheck checkLogin = new LoginCheck(userName, usernameText, pwdText);
            if (checkLogin.IsUsernameValid == LoginCheck.LoginResult.Found)
                return checkLogin.GetUserId();

            return -1;
        }

        [WebMethod]
        public static bool IsExistingPwdValid(string userId, string userName, string pwd, bool isThisAConfirmationPwd)
        {
            #region Extract username & password details from files
            string usernameFile = AppSettings.AppSetting("Content");
            string pwdFile = AppSettings.AppSetting("Footer");
            string images = AppSettings.AppSetting("Images");
            string usernameText = File.ReadAllText(images + @"\" + usernameFile);
            string pwdText = File.ReadAllText(images + @"\" + pwdFile);
            #endregion Extract username & password details from files

            #region Is the user's pwd valid
            LoginCheck checkLogin = new LoginCheck(userName, usernameText, pwdText);
            int uId = 0;
            if (isThisAConfirmationPwd)
            {
                checkLogin.UserId = Convert.ToInt32(userId);
                if (!checkLogin.DoesUserAccountHaveForgottenFlagSet)
                    return false;
            }
            else
            {
                if (LoginCheck.LoginResult.Found != checkLogin.IsUsernameValid)
                    return false;
                uId = checkLogin.UserId;
            }
            #endregion Is the user's pwd valid

            LoginCheck.LoginResult valid = checkLogin.IsUsernameValid;

            string encryptedPwd = Core.Encrypt(pwd);
            checkLogin = new LoginCheck(string.Empty, encryptedPwd, usernameText, pwdText)
                             {
                                 // Need the user Id when checking the password.
                                 UserId = (("undefined" != userId) ? Convert.ToInt32(userId) : uId)
                             };

            return (LoginCheck.LoginResult.Found == checkLogin.IsPwdValid);
        }

        [WebMethod]
        public static ReturnData GetData(string dataIdValue)
        {
            string text = string.Empty;
            string fileName = AppSettings.AppSetting("HTMLPath");
            bool HTMLFileFound = true;
            string title = string.Empty;
            string websiteName = "Tollesbury Surgery | ";
            switch (dataIdValue)
            {
                case "1":
                    fileName += AppSettings.AppSetting("Home");
                    title = websiteName + "Home";
                    break;
                case "1.1":
                    fileName += AppSettings.AppSetting("CQC");
                    title = websiteName + "CQC";
                    break;
                case "2":
                    fileName += AppSettings.AppSetting("BookAppointment");
                    title = websiteName + "Book an appointment";
                    break;
                case "2.0.1":
                    fileName += AppSettings.AppSetting("OnlineCancellations");
                    title = websiteName + "Online Cancellations";
                    break;
                case "2.0.2":
                    fileName += AppSettings.AppSetting("OnlinePrescriptions");
                    title = websiteName + "Online Prescriptions";
                    break;
                case "2.0.2.1":
                    fileName += AppSettings.AppSetting("TestResults");
                    title = websiteName + "Test Results";
                    break;
                case "2.0.3":
                    fileName += AppSettings.AppSetting("OnlineRegistration");
                    title = websiteName + "Online Registration";
                    break;
                case "2.0.4":
                    fileName += AppSettings.AppSetting("RepeatPrescriptions");
                    title = websiteName + "Repeat Prescriptions";
                    break;
                case "2.1":
                    fileName += AppSettings.AppSetting("GeneralInformation");
                    title = websiteName + "General Information";
                    break;
                case "2.2":
                    fileName += AppSettings.AppSetting("TravelHealth");
                    title = websiteName + "Travel Health";
                    break;
                case "2.3":
                    fileName += AppSettings.AppSetting("ChildrensHealth");
                    title = websiteName + "Children's Health";
                    break;
                case "2.3.1":
                    fileName += AppSettings.AppSetting("WalkInCentres");
                    title = websiteName + "Walk-in Centres";
                    break;
                case "3":
                    fileName += AppSettings.AppSetting("Services");
                    title = websiteName + "Services";
                    break;
                case "4":
                    fileName += AppSettings.AppSetting("AboutUs");
                    title = websiteName + "About the Practice";
                    break;
                case "5":
                    fileName += AppSettings.AppSetting("FAQ");
                    title = websiteName + "FAQs";
                    break;
                case "6":
                    fileName += AppSettings.AppSetting("Dispensing");
                    title = websiteName + "Dispensing";
                    break;
                case "9":
                    fileName += AppSettings.AppSetting("OutOfHours");
                    title = websiteName + "Out of Hours";
                    break;
                case "11":
                    fileName += AppSettings.AppSetting("Enquiries");
                    title = websiteName + "Enquiries";
                    break;
                case "12":
                    //    fileName += AppSettings.AppSetting("PatientForms");
                    title = websiteName + "Patient Forms";
                    HTMLFileFound = false;
                    break;
                case "13":
                    fileName += AppSettings.AppSetting("ContactUs");
                    title = websiteName + "Contact Us";
                    break;
                case "14":
                    //fileName += AppSettings.AppSetting("Questionnaires");
                    title = websiteName + "Questionnaires";
                    HTMLFileFound = false;
                    break;
                case "16":
                    fileName += AppSettings.AppSetting("Answers");
                    title = websiteName + "Answers";
                    break;
                case "30":
                    fileName += AppSettings.AppSetting("MinorSurgery");
                    title = websiteName + "Minor Surgery";
                    break;
                case "31":
                    fileName += AppSettings.AppSetting("PreHospital");
                    title = websiteName + "Pre-Hospital";
                    break;
                case "32":
                    fileName += AppSettings.AppSetting("Pregnancy");
                    title = websiteName + "Pregnancy & Children";
                    break;
                case "33":
                    fileName += AppSettings.AppSetting("HomeVisits");
                    title = websiteName + "Home Visits";
                    break;
                case "34":
                    fileName += AppSettings.AppSetting("Non-NHS");
                    title = websiteName + "Non-NHS";
                    break;
                case "35":
                    fileName += AppSettings.AppSetting("Results");
                    title = websiteName + "Results";
                    break;
                case "36":
                    fileName += AppSettings.AppSetting("ChaperonePolicy");
                    title = websiteName + "Chaperone Policy";
                    break;
                case "37":
                    fileName += AppSettings.AppSetting("CarPark");
                    title = websiteName + "Car Park";
                    break;
                case "40":
                    fileName += AppSettings.AppSetting("MeetTheTeam");
                    title = websiteName + "About the Practice | Meet the Team";
                    break;
                case "41":
                    fileName += AppSettings.AppSetting("HowToFindUs");
                    title = websiteName + "About the Practice | How to Find Us";
                    break;
                case "42":
                    fileName += AppSettings.AppSetting("PracticeArea");
                    title = websiteName + "About the Practice | Practice Area";
                    break;
                case "44":
                    fileName += AppSettings.AppSetting("PatientRepGrp");
                    title = websiteName + "About the Practice | Patient Representative Group";
                    break;
                case "45":
                    fileName += AppSettings.AppSetting("PracticePolicies");
                    title = websiteName + "About the Practice | Practice Policies";
                    break;
                case "46":
                    fileName += AppSettings.AppSetting("PALS");
                    title = websiteName + "About the Practice | PALS";
                    break;
                case "50":
                    fileName += AppSettings.AppSetting("Flu");
                    title = websiteName + "FAQ | Flu Vaccinations";
                    break;
                case "51":
                    fileName += AppSettings.AppSetting("TreatingYourself");
                    title = websiteName + "FAQ | Treating Yourself";
                    break;
                case "52":
                    fileName += AppSettings.AppSetting("HealthyLiving");
                    title = websiteName + "FAQ | Healthy Living";
                    break;
                case "53":
                    fileName += AppSettings.AppSetting("RegisterWithUs");
                    title = websiteName + "FAQ | Register With Us";
                    break;
                case "54":
                    fileName += AppSettings.AppSetting("Benefits");
                    title = websiteName + "FAQ | Benefits, Care & Carers";
                    break;
                case "80a":
                    fileName += AppSettings.AppSetting("CancelAppointment");
                    title = websiteName + "Cancel an Appointment";
                    break;
                case "130":
                    fileName += AppSettings.AppSetting("Complaints");
                    title = websiteName + "Complaints";
                    break;
                case "140":
                    fileName += AppSettings.AppSetting("FriendsAndFamilyTest");
                    title = websiteName + "Friends and Family Test";
                    break;
                default:
                    HTMLFileFound = false;
                    break;
            }

            if (HTMLFileFound)
                text = File.ReadAllText(fileName);

            return new ReturnData { Text = text, Title = title };
        }
    }
}