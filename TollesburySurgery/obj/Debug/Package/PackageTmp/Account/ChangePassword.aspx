<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="TS.Account.ChangePassword" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Change Password
    </h2>
    <p>
        Use the form below to change your password.
    </p>
    <p>
        New passwords are required to be a minimum of
        <%= Membership.MinRequiredPasswordLength %>
        characters in length.
    </p>
    <asp:Label ID="lblError" runat="server" ClientIDMode="static" ForeColor="Red" Font-Bold="true" />
    <input id="userName" runat="server" type="hidden" clientidmode="static" />
    <asp:ValidationSummary ID="ChangeUserPasswordValidationSummary" runat="server" CssClass="failureNotification"
        ValidationGroup="ChangeUserPasswordValidationGroup" />
    <div class="accountInfo">
        <fieldset class="changePassword">
            <legend>Account Information</legend>
            <p>
                <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Old Password:</asp:Label>
                <asp:TextBox ID="CurrentPassword" runat="server" ClientIDMode="static" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                    CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Old Password is required."
                    ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">New Password:</asp:Label>
                <asp:TextBox ID="NewPassword" runat="server" ClientIDMode="static" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
               <%-- <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                    CssClass="failureNotification" ErrorMessage="New Password is required." ToolTip="New Password is required."
                    ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>--%>
            </p>
            <p>
                <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                <asp:TextBox ID="ConfirmNewPassword" runat="server" ClientIDMode="static" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
              <%--  <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                    CssClass="failureNotification" Display="Dynamic" ErrorMessage="Confirm New Password is required."
                    ToolTip="Confirm New Password is required." ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                    ControlToValidate="ConfirmNewPassword" CssClass="failureNotification" Display="Dynamic"
                    ErrorMessage="The Confirm New Password must match the New Password entry." ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:CompareValidator>--%>
            </p>
        </fieldset>
        <p class="submitButton">
            <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" Text="Cancel" OnClick="CancelPushButton_OnClick" />
            <asp:Button ID="ChangePasswordPushButton" runat="server" ClientIDMode="static" CommandName="ChangePassword" OnClientClick="ts.test()"
                Text="Change Password" ValidationGroup="ChangeUserPasswordValidationGroup" OnClick="ChangePasswordPushButton_OnClick" />
        </p>
    </div>
     <script type="text/javascript">
         $(function () {
             ts.setupCheckCurrentPasswordOnChange();
             ts.setupCheckNewPasswordsAreTheSame();
         });
    </script>
</asp:Content>
