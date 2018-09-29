<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="FeedbackSafe.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Log In
    </h2>

    <div style="float:right; padding-right:100px;">

    <img src="/Images/talk.png" alt="" />

    </div>

    <p>
       Enter your username and password and click the arrow.
    </p>
    <br />
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false" 
        RenderOuterTable="false" DestinationPageUrl="~/Catch.aspx">
        <LayoutTemplate>
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="LoginUserValidationGroup"/>
            <div class="accountInfo">
                <div style="float:right; margin-right:190px; vertical-align:text-top;">
                    <asp:ImageButton ID="LoginButton" runat="server" CommandName="Login" ImageUrl="~/Images/Interface/next.png" ValidationGroup="LoginUserValidationGroup" AlternateText="Login" ToolTip="Login" />
               </div>          
                  
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Username:</asp:Label>
                        <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                             CssClass="failureNotification" Font-Bold="true" ForeColor="Red" ErrorMessage="User Name is required." ToolTip="User Name is required." 
                             ValidationGroup="LoginUserValidationGroup">X</asp:RequiredFieldValidator>
               <br /><br />
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:&nbsp;</asp:Label>
                        <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                             CssClass="failureNotification" Font-Bold="true" ForeColor="Red" ErrorMessage="Password is required." ToolTip="Password is required." 
                             ValidationGroup="LoginUserValidationGroup">X</asp:RequiredFieldValidator>
                  <br /><br /> 
                    <p>
                        <asp:CheckBox ID="RememberMe" runat="server"/>
                        <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">Keep me logged in</asp:Label>
                    </p>


                <br />
                <a href="ForgotPassword.aspx" title="Forgot Password?">Forgot Password?</a>
            </div>
        </LayoutTemplate>
    </asp:Login>
</asp:Content>
