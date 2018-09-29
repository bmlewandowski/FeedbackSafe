<%@ Page Title="" Language="C#" MasterPageFile="~/Sales/Sales.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FeedbackSafe.Sales.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<!--LOAD STYLESHEETS-->
    <link href="/Styles/leader.css" rel="stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2>Sales Dashboard</h2>

<div style="float:right;">

<div id="leader_statbar">
    <h3>Organization Statistics</h3>
    <asp:Label ID="lbl_stat_approved" runat="server" Font-Bold="true"></asp:Label>  Number of Approved Conversations<br />
<br />
    <asp:Label ID="lbl_stat_unread" runat="server" Font-Bold="true"></asp:Label>  Number of Unread Conversations<br />
<br />
    <asp:Label ID="lbl_stat_leaders" runat="server" Font-Bold="true"></asp:Label>  Number of Leaders<br />
<br />  
    <asp:Label ID="lbl_stat_users" runat="server" Font-Bold="true"></asp:Label>  Number of Users<br />
<br />  
    <asp:Label ID="lbl_stat_optoutconversations" runat="server" Font-Bold="true"></asp:Label>  Users Opted Out of Conversations<br />
<br />  
     <asp:Label ID="lbl_stat_optoutcomments" runat="server" Font-Bold="true"></asp:Label>  Users Oped out Comments<br />
<br />  
     <asp:Label ID="lbl_optout" runat="server" Font-Bold="true"></asp:Label>  Users Oped out all Mail<br />
          
 
</div>

</div>

<img src="/Images/safe.jpg" alt="" style="vertical-align:middle" />
<strong>AUTHORIZED ACCESS ONLY:</strong>
&nbsp;IP Logging On 
<br />
<asp:DropDownList ID="ddl_selectOrg" runat="server" OnSelectedIndexChanged="ddl_selectOrg_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>

</asp:Content>
