<%@ Page Title="" Language="C#" MasterPageFile="~/Private.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="FeedbackSafe.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="/Images/Interact/personPic.jpg" style="vertical-align:middle; float:right; margin-right:20px;" alt="" />
<h1>Personal Preferences</h1>

<asp:Label ID="lbl_userEmail" runat="server" Visible="false"></asp:Label>
<br />
<div id="addPassword" runat="server" visible="true">

    <h3>Add a Password</h3>
    <p>Add a password to your email account to login to dashboard without use of email key.
    <br />This does not affect your privacy as you still remain completely anonymous.
    <br />This simply makes it easier to access your dashboard.</p>
    New Password:&nbsp;<asp:TextBox ID="txt_addPassword" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="val_addPassword" runat="server" ControlToValidate="txt_addPassword" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddPassword"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="val_PasswordLength" runat="server" ControlToValidate="txt_addPassword" ErrorMessage="* Minimum password length is 7" ValidationExpression=".{7}.*"  ForeColor="Red" ValidationGroup="AddPassword" />
    <br />
    Confirm Password:&nbsp;<asp:TextBox ID="txt_confirmPassword" runat="server"></asp:TextBox>
     <asp:RequiredFieldValidator ControlToValidate="txt_confirmPassword" CssClass="failureNotification" ErrorMessage="Confirm Password is required." ID="ConfirmPasswordRequired" runat="server" 
                                     ToolTip="Confirm Password is required." ValidationGroup="AddPassword" ForeColor="Red"><strong>X</strong></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="txt_addPassword" ControlToValidate="txt_confirmPassword" 
                                     CssClass="failureNotification" ErrorMessage="The Password and Confirmation Password must match."
                                      ForeColor="Red" ValidationGroup="AddPassword"><strong>X</strong></asp:CompareValidator>
    <br /><br />
    <asp:Button ID="btn_addPassword" runat="server" Text="Add Password" 
        ToolTip="Add Password" onclick="btn_addPassword_Click" ValidationGroup="AddPassword" />
    <br /><br /><br /><br />    
    
</div>

<div id="changePreferences" runat="server" visible="true">

    <h3>Email Preferences</h3>
    <p>
    <asp:CheckBox ID="chk_optOutConversation" runat="server" Text="Opt Out of New Conversation Email Notifications" Font-Bold="true" /><br /><br />
    Checking this option disables email receipts from new conversations.
    </p>
    <br />
    <p>
        <asp:CheckBox ID="chk_optOutComment" runat="server" Text="Opt Out of New Comment Email Notifications" Font-Bold="true" /><br /><br />
        Checking this option disables email receipts from new comments.
        </p>
        <br />
        <p>
        <asp:CheckBox ID="chk_optOut" runat="server" Text="Opt Out of ALL Email" Font-Bold="true" /><br /><br />
        Checking this option removes you from any email communications except recovering your personal key.
        </p>

     <br />
    <asp:Button ID="btn_savePreferences" runat="server" Text="Save Email Preferences" 
        ToolTip="Save Email Preferences" onclick="btn_savePreferences_Click" />
   
</div>

<div id="savedPreferences" runat="server" visible="false">
    <p>Your Preferences Have Been Saved</p>
</div>
    
</asp:Content>

