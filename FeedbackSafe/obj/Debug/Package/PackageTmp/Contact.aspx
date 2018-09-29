<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="FeedbackSafe.Contact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="/Images/Interact/leaderPic.jpg" style="vertical-align:middle; float:right; margin-right:20px;" alt="" />
<h1>Contact</h1>

<p><strong>Feedback Safe</strong><br />
PO Box 5078<br />
Fort Wayne, IN 46895<br />
<a href=mailto:info@feedbacksafe.com" title="Email Feedback Safe">info@feedbacksafe.com</a>
</p>
<br />
    <div id="div_fb_form" runat="server" visible="true">
    <h2>Contact Form</h2>
    
        Name:<br />
            <asp:TextBox ID="txt_fb_name" runat="server"></asp:TextBox>&nbsp;(optional)
                <br /><br />
                    Email:<br />
                    <asp:TextBox ID="txt_fb_email" runat="server"></asp:TextBox>&nbsp;(optional)
                <br /><br />
                    Comments:
                    <asp:RequiredFieldValidator ID="val_fb_comments" runat="server" ControlToValidate="txt_fb_comments" ErrorMessage="* Required" ToolTip="Content Required" ValidationGroup="Feedback" ForeColor="Red"><strong>X</strong>&nbsp;</asp:RequiredFieldValidator>
                <br />
                    <asp:TextBox ID="txt_fb_comments" runat="server" Height="80px" Width="400px" TextMode="MultiLine" />
                <br /><br /><br />
        
                    <asp:Button ID="btn_submit_feedback" runat="server" Text="Send Feedback" onclick="btn_submit_feedback_Click" ValidationGroup="Feedback" ToolTip="Send"  />   
    </div>

    <div id="div_fb_success" runat="server" visible="false">
        <br />
            <strong>Success. Thank you for your Feedback!</strong>
    </div>

</asp:Content>
