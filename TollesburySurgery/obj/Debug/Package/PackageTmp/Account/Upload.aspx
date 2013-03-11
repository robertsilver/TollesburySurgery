<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Upload.aspx.cs" Inherits="TollesburySurgery.Account.Upload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Upload Documents</h1>
    <br />
    <h2>
        New document</h2>
    <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red" />
    <div class="row">
        <label for="NameOfDoc">
            Name of new document:</label>
        <span class="field">
            <asp:TextBox ID="txtNewDocName" runat="server" MaxLength="70" ValidationGroup="FileUpload" />
            <asp:RequiredFieldValidator ID="reqDocName" runat="server" Font-Size="Small" ForeColor="Red"
                ValidationGroup="FileUpload" SetFocusOnError="true" Display="static" ControlToValidate="txtNewDocName"
                Text="New document name required" />
        </span>
    </div>
    <div class="row">
        <label for="f-fileType">
            File Type:</label>
        <span class="field">
            <asp:DropDownList ID="ddlNewDocFileType" runat="server" ValidationGroup="FileUpload">
                <asp:ListItem Text="(Please select)" Value="0" />
                <asp:ListItem Text="PDF" Value="PDF" />
                <asp:ListItem Text="Word" Value="Word" />
                <asp:ListItem Text="Excel" Value="Excel" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="reqFileType" runat="server" InitialValue="0" Font-Size="Small"
                ForeColor="Red" ValidationGroup="FileUpload" SetFocusOnError="true" Display="Static"
                ControlToValidate="ddlNewDocFileType" Text="Document type must be chosen" />
        </span>
    </div>
    <div class="row">
        <label for="f-uploadDoc">
            Upload the new document:</label>
        <span class="field">
            <asp:FileUpload ID="fileNewDocument" runat="server" ValidationGroup="FileUpload" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Font-Size="Small"
                ForeColor="Red" ValidationGroup="FileUpload" SetFocusOnError="true" Display="Static"
                ControlToValidate="fileNewDocument" Text="A file must be chosen" />
        </span>
    </div>
    <div class="row">
        <label for="f-uploadWhichPartOfWebSite">
            Is this a:</label>
        <span class="field">
            <asp:RadioButtonList ID="radWhichPartOWebSite" runat="server">
                <asp:ListItem Text="Patient Form?" Value="PatientForms" />
                <asp:ListItem Text="Questionnaire?" Value="Questionnaires"/>
                <asp:ListItem Text="Patient Participation Forum?" Value="PatientParticipation"/>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="reqWhichWebSite" runat="server" Font-Size="Small"
                ForeColor="Red" ValidationGroup="FileUpload" SetFocusOnError="true" Display="Static"
                ControlToValidate="radWhichPartOWebSite" Text="You must choose which part of the web site the document is to be placed" />
        </span>
    </div>
    <div class="row">
        <label for="f-uploadDocBlank">
            &nbsp;</label>
        <span class="field">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_OnClick" ValidationGroup="FileUpload" />
        </span>
    </div>
    <h2>
        Update documents</h2>
        <asp:Label ID="lblNoGridData" runat="server" Text="No files have been uploaded" ForeColor="Red" />
    <asp:GridView ID="grdUploadData" runat="server" AutoGenerateColumns="false" OnRowCancelingEdit="grdUploadData_OnRowCancelingEdit"
        OnRowEditing="grdUploadData_OnRowEditing" OnRowDeleting="grdUploadData_OnRowDeleting" OnRowUpdating="grdUploadData_OnRowUpdating">
        <Columns>
            <asp:TemplateField HeaderText="File name">
                <EditItemTemplate>
                    <asp:HiddenField ID="hidID" runat="server" Value='<%# Eval("ID") %>' />
                    <asp:TextBox ID="txtDocName" runat="server" MaxLength="70" Text='<%# Eval("DocumentName") %>' />
                </EditItemTemplate>
                <ItemStyle HorizontalAlign="Center"/>
                <ItemTemplate>
                    <asp:HiddenField ID="hidID" runat="server" Value='<%# Eval("ID") %>' />
                    <%# Eval("DocumentName") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Is the file visible to the patient?">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:CheckBox ID="chkDocVisible" runat="server" Enabled="false" Checked='<%# ((Eval("DocumentVisible").ToString().ToLower() == "true") ? true : false) %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBox ID="chkDocVisible" runat="server" Enabled="true" Checked='<%# ((Eval("DocumentVisible").ToString().ToLower() == "true") ? true : false) %>' />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File Type">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <%# Eval("DocumentFileType") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlDocType" runat="server">
                        <asp:ListItem Text="PDF" Value="PDF" />
                        <asp:ListItem Text="Word" Value="Word" />
                        <asp:ListItem Text="Excel" Value="Excel" />
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="Which part of the web site?">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <%# Eval("WhichPartOfWebSite")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Associated with filename">
            <ItemTemplate>
                <%# Eval("AssociatedFileName") %>
            </ItemTemplate>
            </asp:TemplateField>
            <%--  <asp:TemplateField>
               <ItemTemplate>
                   <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
               </ItemTemplate>
           </asp:TemplateField>--%>
            <asp:CommandField ButtonType="link" CancelText="Cancel" EditText="Edit" ShowEditButton="true" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
</asp:Content>
