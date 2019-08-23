using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TSDomain;

namespace TollesburySurgery.Code
{
    public partial class Files : System.Web.UI.Page
    {
        private class FileDetails
        {
            public string AssociatedFileName { get; set; }
            public string DocumentFileType { get; set; }
            public string DocumentName { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DisplayExtraText();

            Page.Title = Request.QueryString["title"];
            string fileType = GetFileType();
            if (string.IsNullOrEmpty(fileType))
            {
                lblError.Text = "There are no files to view";
                lblError.Visible = true;
                header.Visible = false;
                return;
            }

            System.Xml.Linq.XDocument xmlDoc = System.Xml.Linq.XDocument.Load(AppSettings.AppSetting("UploadedDocuments"));
            var fileDetails = from w in xmlDoc.Elements("UploadedDocs").Elements("Document")
                        where (string)w.Attribute("WhichPartOfWebSite") == fileType &&
                        w.Element("DocumentVisible").Value == "true"
                        select w;

            List<FileDetails> displayData = fileDetails.Select(q => new FileDetails
                                            {
                                                AssociatedFileName = q.Element("AssociatedFileName").Value,
                                                DocumentFileType = q.Element("DocumentFileType").Value,
                                                DocumentName = q.Element("DocumentName").Value
                                            }).ToList();

            if (displayData.Count > 0)
            {
                lstFiles.DataSource = displayData;
                lstFiles.DataBind();
                lblError.Visible = false;
                header.Visible = true;
                header.Text = GetHeaderTitle();
            }
            else
            {
                lblError.Text = "There are no files to view";
                lblError.Visible = true;
                header.Visible = false;
            }
        }

        #region Private methods
        private void DisplayExtraText()
        {
            switch (Request.QueryString["q"])
            {
                case "1":
                    extraText.Visible = false;
                    break;
                case "2":
                    extraText.Visible = false;
                    break;
                case "3":
                    extraText.Visible = true;
                    extraText.Text = "<div style='margin-top: 10px; margin-bottom: 30px;'><p style='font-weight: bold; text-align: center;'>Patient Participation Group</p><p>Please ring the surgery to find out the dates for the next PPG meeting.  Patients can come along and help us improve the surgery experience for everyone.</p><p>We have a thriving Patient Group who prefer to meet face to face every 3 months. We meet to update the group on any planned changes and seek feedback on how we are doing and how we could improve our services.</p><p>The active group is made up of 15 core members, although we have at other times several people who have attended on and off. Dr Abazie is the practice lead and attends meetings when he is able too.</p><p>The group comprises of: 3 male members and 12 female members, White British<br />16 – 24 years = 0<br />25 – 34 years = 1<br />35 – 44 years = 3<br />45 – 54 years = 2<br />55 - 64 years = 4<br />65 and over = 5<br /></p> <p>We have tried recruiting more patients by advertising on the website, Practice leaflet, posters and flyers and in person.</p> <p>If you would like to become a member of this group or have your say please contact the surgery.</p> <p>Please contact the surgery by telephone on 01621 869204 or download & complete the joining form from <a href='http://www.tollesburysurgery.co.uk/FileUploads/Tollesbury%20Surgery%20Patient%20Participation%20Group%20Sign%20Up%20Form.doc' target='_blank'>our website</a>, and hand it in to reception.</p> <p>We welcome any suggestions or comments you may have about the services offered to help maintain the highest standard of care possible and meeting your needs. We also carry out annual patient surveys in addition to the nationally run patient surveys by the NHS.</p></div>";
                    break;
            }
        }

        private string GetFileType()
        {
            string fileType = string.Empty;

            switch (Request.QueryString["q"])
            {
                case "1":
                    fileType = "PatientForms";
                    break;
                case "2":
                    fileType = "Questionnaires";
                    break;
                case "3":
                    fileType = "PatientParticipation";
                    break;
            }

            return fileType;
        }

        private string GetHeaderTitle()
        {
            string headerTitle = string.Empty;

            switch (Request.QueryString["q"])
            {
                case "1":
                    headerTitle = "Patient Forms";
                    break;
                case "2":
                    headerTitle = "Questionnaires";
                    break;
                case "3":
                    headerTitle = "Patient Participation Minutes";
                    break;
            }

            return headerTitle;
        }
        #endregion Private methods
    }
}