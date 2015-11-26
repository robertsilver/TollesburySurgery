using System;
using System.Text;
using System.Web.Security;
using System.Xml;
using TSDomain;

namespace TS
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            StringBuilder leftMenu = new StringBuilder();
            StringBuilder tempMenu = new StringBuilder();
            string menuFileName = AppSettings.AppSetting("Menus");
            XmlTextReader reader = new XmlTextReader(menuFileName);
            leftMenu.AppendLine("<ul>");
            bool isThisAHeaderNode = false;
            bool showMenu = false;
            string anchor = string.Empty;
            while (reader.Read())
            {
                string parentId = string.Empty;

                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.Name == "Menu")
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                if (reader.Name == "ShowMenu" && reader.Value == "Yes")
                                {
                                    showMenu = true;
                                    break;
                                }
                                else
                                    showMenu = false;
                            }

                            if (showMenu)
                            {
                                leftMenu.AppendLine("<li ");
                                reader.MoveToFirstAttribute();
                                // You must issue the MoveToFirstAttribute(),
                                // because in the showMenu loop above, you've
                                // just cycled around the attributes, so set
                                // setup it back to the beginning.
                                do
                                {
                                    // The reason you have to use the do while is because, if you do while() {},
                                    // and you issue the MoveToFirstAttribute(), too, the move will bypass
                                    // the ID attribute within the menus, meaning that when the user clicks
                                    // a menu, nothing will happen because the jScript doesn't have an ID
                                    // to fetch a HTML document.  If you don't issue the MoveToFirsAttribute above,
                                    // and run the while loop, the code says, oh, we are at the end of the attributes
                                    // because of the showMenu loop above, there's nothing to process.
                                    switch (reader.Name)
                                    {
                                        case "Header":
                                            tempMenu.Append("data-header='" + reader.Value + "' ");

                                            if (reader.Value == "No")
                                            {
                                                tempMenu.Append("data-parent-data-id='" + parentId + "' ");
                                                isThisAHeaderNode = false;
                                            }
                                            else
                                                isThisAHeaderNode = true;
                                            break;
                                        case "ID":
                                            tempMenu.Append("data-id='" + reader.Value + "' ");
                                            break;
                                        case "ParentId":
                                            parentId = reader.Value;
                                            break;
                                        case "DefaultMenu":
                                            tempMenu.Append("data-default-item='" + reader.Value + "' ");
                                            break;
                                        case "Link":
                                            tempMenu.Append("data-link='" + reader.Value.Replace(",", "&") + "' ");
                                            break;
                                    }
                                } while (reader.MoveToNextAttribute()); // Read the attributes.
                            }
                            else
                                showMenu = false;
                        }
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        if (showMenu)
                        {
                            if (isThisAHeaderNode)
                                leftMenu.AppendLine("class='menu-header' ");
                            else
                                leftMenu.AppendLine("class='menu-child' ");

                            leftMenu.Append(tempMenu.ToString());
                            leftMenu.Append(">" + reader.Value);
                            tempMenu = new StringBuilder();
                        }
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        if (showMenu)
                        {
                            if (reader.Name == "Menu")
                                leftMenu.AppendLine("</li>");
                        }
                        break;
                }
            }
            leftMenu.AppendLine("</ul>");

            reader.Close();
            reader = null;

            StringBuilder userLoggedIn = new StringBuilder();
            if (Request.IsAuthenticated)
            {
                
                userLoggedIn.AppendLine("<li class='menu-header'><a style='text-decoration: none; color: red; font-weight: bold; font-size: 1.25em;' href='/Account/Upload.aspx'>Upload Files</a></li>");
                userLoggedIn.AppendLine("<li class='menu-header'><a style='text-decoration: none; color: red; font-weight: bold; font-size: 1.25em;' href='/Account/ChangePassword.aspx'>Change Password</a></li>");
            }

            leftMenu = leftMenu.Replace("<ul>", "<ul>" + userLoggedIn);

            litLeftMenu.Text = leftMenu.ToString();
            leftMenu = null;
            tempMenu = null;
        }

        protected void LogOut_OnLoggedOut(Object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}
