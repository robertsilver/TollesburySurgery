<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FriendsAndFamily.aspx.cs" Inherits="TS.Code.FriendsAndFamily" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
      <script type="text/javascript">
     
      function contentPageLoad() {
          var strMessage = $("#textboxMessage").val();

          if (strMessage != "") {
              var strMessage1;
              if (strMessage.indexOf('Validation') == 0) {
                  var length = strMessage.length;
                  strMessage1 = strMessage.substring(11);
                  strMessage1 = strMessage1.replace(/\\n/g, "\n");

                  $.popup.show("Validation", strMessage1);
                  $("#textboxMessage").val("");
              }
              else if (strMessage.indexOf('Error') == 0) {
                  var length = strMessage.length;
                  strMessage1 = strMessage.substring(6);
                  strMessage1 = strMessage1.replace(/\\n/g, "\n");

                  $.popup.show("Error", strMessage1);
                  $("#textboxMessage").val("");
              } else if (strMessage.indexOf('Warning') == 0) {
                  var length = strMessage.length;
                  strMessage1 = strMessage.substring(8);
                  strMessage1 = strMessage1.replace(/\\n/g, "\n");

                  $.popup.show("Error", strMessage1);
                  $("#textboxMessage").val("");
              }
              else {
                  strMessage = strMessage.replace(/\\n/g, "\n");
                  $.popup.show("Confirmation", strMessage);
                  $("#textboxMessage").val("");
              }
          }
      }

      $(document).ready(function () {
          contentPageLoad();
      });
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:TextBox ID="textboxMessage" runat="server" style="display: none;" ClientIDMode="Static"></asp:TextBox>
    <h1>NHS Friends and Family Test</h1>
    1. We would like you to think about your recent experience of our service. "How likely are you to recommend the Tollesbury Surgery to friends and family if they needed similar care or treatment?"
    <div style="margin: 15px 0">
        <asp:RadioButtonList ID="Qu1" runat="server" RepeatDirection="Vertical">
            <asp:ListItem Text="Extremely likely" Value="ExtremelyLikely"></asp:ListItem>
            <asp:ListItem Text="Neither likely or unlikely" Value="NeitherLikelyUnlikely"></asp:ListItem>
            <asp:ListItem Text="Unlikely" Value="Unlikely"></asp:ListItem>
            <asp:ListItem Text="Extremely unlikely" Value="ExtremelyUnlikely"></asp:ListItem>
            <asp:ListItem Text="Don't know" Value="DontKnow"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <div style="margin: 15px 0">
        Please tell us the main reason for selecting your statement
        <asp:TextBox ID="Reason" runat="server" MaxLength="2000" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
    </div>
    <div style="margin: 15px 0">
        2. If you wish to give your name and contact details, please do so below.
        <asp:TextBox ID="ContactDetails" runat="server" MaxLength="1000" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
    </div>
    <div style="margin: 15px 0">
        <asp:Button ID="Submit" runat="server" Text="Submit answers" OnClick="Submit_OnClick" />
    </div>
</asp:Content>
