<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="TS.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Log In
    </h2>
    <div class="accountInfo">
        <asp:Label ID="lblError" runat="server" Visible="true" ClientIDMode="static" ForeColor="Red" Font-Bold="true"/>
        <fieldset class="login">
            <legend>Account Information</legend>
            <p>
                <input id="user-Id" type="hidden"/>
                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Username:</asp:Label>
                <asp:TextBox ID="UserName" runat="server" ClientIDMode="static" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                    CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required."
                    ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="PasswordLabel" runat="server" ClientIDMode="static" AssociatedControlID="Password">Password:</asp:Label>
                <asp:TextBox ID="Password" runat="server" CausesValidation="true" ClientIDMode="static" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ClientIDMode="static" ControlToValidate="Password"
                    CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required."
                    ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:CheckBox ID="ForgottenPwd" runat="server" ClientIDMode="static" />
                <asp:Label ID="ForgottenPwdLabel" runat="server" AssociatedControlID="ForgottenPwd" CssClass="inline">Forgot your password?</asp:Label>
            </p>
        </fieldset>
        <p class="submitButton">
            <asp:Button ID="LoginButton" runat="server" ClientIDMode="static" CommandName="Login" Text="Log In" ValidationGroup="LoginUserValidationGroup"
                OnClick="LoginButton_OnClick" />
        </p>
    </div>
     <script type="text/javascript">
         $(function () {
             ts.setupForgottenPassword();
             ts.setupLoginUserNameOnChange();
         });
    </script>
</asp:Content>
