using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TSDomain;

namespace TollesburySurgery
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
            Page.Title = Request.QueryString["title"];
            string fileType = GetFileType();
            if (string.IsNullOrEmpty(fileType))
            {
                lblError.Text = "There are no files to view";
                lblError.Visible = true;
                return;
            }

            XDocument xmlDoc = XDocument.Load(AppSettings.AppSetting("UploadedDocuments"));
            var fileDetails = from w in xmlDoc.Elements("UploadedDocs").Elements("Document")
                        where (string)w.Attribute("WhichPartOfWebSite") == fileType
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
            }
            else
            {
                lblError.Text = "There are no files to view";
                lblError.Visible = true;
            }
        }

        #region Private methods
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

        #endregion Private methods

        #region Public methods
        public string GetImage(string fileType)
        {
            if (fileType == "PDF")
                return "/Content/Images/PDF_Image.jpg";

            if (fileType == "Word")
                return "/Content/Images/Word_Image.jpg";

            if (fileType == "Excel")
                return "/Content/Images/Excel_Image.png";

            return string.Empty;
        }

        public string GetImageName(string fileType)
        {
            if (fileType == "PDF")
                return "PDF image";

            if (fileType == "Word")
                return "Word image";

            if (fileType == "Excel")
                return "Excel image";

            return string.Empty;
        }
        #endregion Public methods
    }
}