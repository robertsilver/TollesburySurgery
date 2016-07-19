<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Files.aspx.cs" Inherits="TollesburySurgery.Files" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        <asp:Literal ID="header" runat="server"></asp:Literal></h1>
        <asp:Literal ID="extraText" runat="server"></asp:Literal>
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="true" />
    <asp:Repeater ID="lstFiles" runat="server">
        <HeaderTemplate>
            <table>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <a style="width: 10px;" href='/FileUploads/<%# Eval("AssociatedFileName") %>'>Download - <%# Eval("DocumentName") %></a>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
