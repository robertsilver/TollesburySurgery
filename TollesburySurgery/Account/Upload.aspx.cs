using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using TSDomain;

namespace TollesburySurgery.Account
{
    public partial class Upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            BindDataGrid();
        }

        protected void grdUploadData_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdUploadData.EditIndex = -1;
            BindDataGrid();
        }

        protected void grdUploadData_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HiddenField hidId = (HiddenField)grdUploadData.Rows[e.RowIndex].Cells[0].Controls[1];
            XDocument xmlDoc = XDocument.Load(AppSettings.AppSetting("UploadedDocuments"));
            XElement elemToDelete = xmlDoc.Descendants("Document").Where(x => x.Attribute("ID").Value == hidId.Value).Single();
            string fileName = elemToDelete.Element("AssociatedFileName").Value;
            File.Delete(Server.MapPath("~/FileUploads/") + fileName);
            elemToDelete.Remove();
            xmlDoc.Save(AppSettings.AppSetting("UploadedDocuments"));
            hidId.Dispose();

            BindDataGrid();
        }

        protected void grdUploadData_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            grdUploadData.EditIndex = e.NewEditIndex;
            BindDataGrid();
        }

        protected void grdUploadData_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            grdUploadData.EditIndex = -1;
         
            // Hidden field
            HiddenField hidId = (HiddenField)grdUploadData.Rows[e.RowIndex].Cells[0].Controls[1];
            TextBox txtNewDocName = (TextBox)grdUploadData.Rows[e.RowIndex].Cells[0].Controls[3];
            CheckBox chkVisible = (CheckBox)grdUploadData.Rows[e.RowIndex].Cells[1].Controls[1];
            DropDownList ddlFileType = (DropDownList)grdUploadData.Rows[e.RowIndex].Cells[2].Controls[1];

            EditUploadedDocuments(hidId.Value, txtNewDocName.Text, chkVisible.Checked.ToString().ToLower(), GetUserChosenModifiedFileType(ddlFileType));

            hidId.Dispose();
            txtNewDocName.Dispose();
            chkVisible.Dispose();

            BindDataGrid();
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
           if (IsTheFileExtensionCorrect())
           {
                lblError.Text = "The file that you're trying to upload is not a PDF or Word document.  Please try again.";
                lblError.Visible = true;
                return;
           }

            string filename = Path.GetFileName(fileNewDocument.FileName);
            if (DoesTheUploadedFileAlreadyExist(filename))
            {
                lblError.Text = "A file with that name already exists.  Please try again.";
                lblError.Visible = true;
                return;
            }
           
            lblError.Visible = false;
            
            fileNewDocument.SaveAs(Server.MapPath("~/FileUploads/") + filename);

            DocumentTypes newFileType = new DocumentTypes();
            if (ddlNewDocFileType.SelectedValue == DocumentTypes.PDF.ToString())
                newFileType = DocumentTypes.PDF;
            else if (ddlNewDocFileType.SelectedValue == DocumentTypes.Word.ToString())
                newFileType = DocumentTypes.Word;
            else if (ddlNewDocFileType.SelectedValue == DocumentTypes.Excel.ToString())
                newFileType = DocumentTypes.Excel;

            AddNewElementToXml(txtNewDocName.Text, newFileType, filename, radWhichPartOWebSite.SelectedValue);
            
            txtNewDocName.Text = string.Empty;
            ddlNewDocFileType.SelectedIndex = 0;

            BindDataGrid(); 
        }

        #region Private methods
        private void AddNewElementToXml(string documentName, DocumentTypes docType, string associatedFileName, string whichPartOfWebSite)
        {
            XDocument xmlDoc = XDocument.Load(AppSettings.AppSetting("UploadedDocuments"));

            int id = Core.GetNextId(xmlDoc, "Document");

            #region Add new element
            XElement newDoc = xmlDoc.Descendants("UploadedDocs").Last();
            newDoc.Add(
                new XElement("Document", // This is the value for the element.
                    new XAttribute("ID", id),
                    new XAttribute("WhichPartOfWebSite", whichPartOfWebSite),
                    new XElement("DocumentVisible", "true"),
                    new XElement("DocumentFileType", docType),
                    new XElement("DocumentName", documentName),
                    new XElement("AssociatedFileName", associatedFileName)));

            xmlDoc.Save(AppSettings.AppSetting("UploadedDocuments"));
            #endregion Add new element
        }

        private void BindDataGrid()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppSettings.AppSetting("UploadedDocuments"));
            DataSet theDataSet = new DataSet();
            theDataSet.ReadXml(AppSettings.AppSetting("UploadedDocuments"));
            if (theDataSet.Tables.Count > 0)
            {
                grdUploadData.DataSource = theDataSet;
                grdUploadData.DataBind();
                theDataSet.Dispose();
                lblNoGridData.Visible = false;
            }
            else
            {
                grdUploadData.DataSource = null;
                grdUploadData.DataBind();
                lblNoGridData.Visible = true;
            }
        }

        /// <summary>
        /// Returns true if the file name already exists, otherwise false.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool DoesTheUploadedFileAlreadyExist(string filename)
        {
             if (File.Exists(Server.MapPath("~/FileUploads/" + filename)))
                return true;
            return false;
        }

        private void EditUploadedDocuments(string id, string newDocName, string visible, DocumentTypes fileType)
        {
            XDocument xmlDoc = XDocument.Load(AppSettings.AppSetting("UploadedDocuments"));
            xmlDoc.Descendants("Document").Single(x =>
                                                      {
                                                          var xAttribute = x.Attribute("ID");
                                                          return xAttribute != null && xAttribute.Value == id;
                                                      }).SetElementValue("DocumentVisible", visible);
            xmlDoc.Descendants("Document").Single(x =>
                                                      {
                                                          var attribute = x.Attribute("ID");
                                                          return attribute != null && attribute.Value == id;
                                                      }).SetElementValue("DocumentFileType", fileType.ToString());
            xmlDoc.Descendants("Document").Single(x =>
                                                      {
                                                          var xAttribute1 = x.Attribute("ID");
                                                          return xAttribute1 != null && xAttribute1.Value == id;
                                                      }).SetElementValue("DocumentName", newDocName);
            xmlDoc.Save(AppSettings.AppSetting("UploadedDocuments"));
        }

        /// <summary>
        /// Retrieves the DocumentType that the user has chosen,
        /// when modifying an uploaded file in the grid view.
        /// </summary>
        /// <param name="ddlUserChosenFileType"></param>
        /// <returns></returns>
        private DocumentTypes GetUserChosenModifiedFileType(DropDownList ddlUserChosenFileType)
        {
            if (ddlUserChosenFileType.SelectedValue == DocumentTypes.PDF.ToString())
                return DocumentTypes.PDF;
            if (ddlUserChosenFileType.SelectedValue == DocumentTypes.Word.ToString())
                return DocumentTypes.Word;
            if (ddlUserChosenFileType.SelectedValue == DocumentTypes.Excel.ToString())
                return DocumentTypes.Excel;

            return DocumentTypes.PDF;
        }

        /// <summary>
        /// Returns true if the file extension is PDF or doc or docx.  
        /// Otherwise, false is returned.
        /// </summary>
        /// <returns></returns>
        private bool IsTheFileExtensionCorrect()
        {
            if (System.IO.Path.GetExtension(fileNewDocument.FileName).ToLower() != ".pdf" &&
               System.IO.Path.GetExtension(fileNewDocument.FileName).ToLower() != ".doc" &&
               System.IO.Path.GetExtension(fileNewDocument.FileName).ToLower() != ".docx" &&
                System.IO.Path.GetExtension(fileNewDocument.FileName).ToLower() != ".xls" &&
               System.IO.Path.GetExtension(fileNewDocument.FileName).ToLower() != ".xlsx")
                return true;

            return false;
        }
        #endregion Private methods
    }
}