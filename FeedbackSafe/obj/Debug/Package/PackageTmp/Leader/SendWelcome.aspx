<%@ Page Title="" Language="C#" MasterPageFile="~/Leader/Leader.Master" AutoEventWireup="true" CodeBehind="SendWelcome.aspx.cs" Inherits="FeedbackSafe.Leader.SendWelcome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Send Welcome</h2>
    <br />
    <br />
    <asp:Button ID="btn_sendMail" runat="server" Text="SEND WELCOME EMAIL TO ALL USERS" onclick="btn_sendMail_Click" />

</asp:Content>
