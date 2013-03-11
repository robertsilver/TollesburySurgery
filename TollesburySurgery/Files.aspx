<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Files.aspx.cs" Inherits="TollesburySurgery.Files" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Documents</h1>
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="true" />
    <asp:Repeater ID="lstFiles" runat="server">
        <HeaderTemplate>
            <table>
                <tr>
                    <th>
                        Minutes of Meetings
                    </th>
                </tr>
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
