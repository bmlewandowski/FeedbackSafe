<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="Recover.aspx.cs" Inherits="FeedbackSafe.Recover" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="/Images/Interact/personPic.jpg" style="vertical-align:middle; float:right; margin-right:20px;" alt="" />
<h1>Lost your Email Key?</h1>

<div id="div_requestkey" runat="server">
Email Address:
<asp:TextBox ID="txt_userEmail" runat="server" Width="200px"></asp:TextBox>
<asp:RequiredFieldValidator ID="val_userEmail" runat="server" ControlToValidate="txt_userEmail" ErrorMessage="X" Font-Bold="true" ForeColor="Red" ValidationGroup="GetKey"></asp:RequiredFieldValidator>
<asp:ImageButton ID="btn_recoverKey" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/Interface/next.png" Width="50px" onclick="btn_recoverKey_Click" AlternateText="Retrieve Anonymous Key" ToolTip="Retrieve Anonymous Key" ValidationGroup="GetKey" />
</div>

<div id="div_requestkeysuccess" runat="server" visible="false">
Anonymous Key has been emailed to the address you provided if account exists.
</div>
<br />
<p style="width:350px;">* You can always add a password to your email account from your Preferences and use it to login to your dashboard and still remain anonymous.</p> 
</asp:Content>
