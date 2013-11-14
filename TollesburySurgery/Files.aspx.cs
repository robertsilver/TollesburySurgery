﻿using System;
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
                    extraText.Text = "<strong>Patient Participation Group</strong>: If you are interested in joining this group please contact the Practice Manager Rohita Rajapakse for details.<br /><br />";
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