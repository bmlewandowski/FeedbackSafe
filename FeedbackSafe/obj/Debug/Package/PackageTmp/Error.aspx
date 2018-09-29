<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="FeedbackSafe.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>An Error Has Occurred</h1>
<p>An unexpected error has occurred on the site.</p>
<p><asp:Label ID="lbl_errorMessage" runat="server" Font-Bold="true"></asp:Label></p>
<ul>
<li>  
<asp:HyperLink ID="hlk_backHome" runat="server" NavigateUrl="~/Default.aspx">Return to the homepage</asp:HyperLink>
</li>
</ul>
</asp:Content>
