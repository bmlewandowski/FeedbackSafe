<%@ Page Title="" Language="C#" MasterPageFile="~/Agent/Agent.Master" AutoEventWireup="true" CodeBehind="ManageLeaders.aspx.cs" Inherits="FeedbackSafe.Agent.ManageLeaders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1>Manage Leaders</h1>
<br />

<!--BEGIN UPDATE PANEL-->
    <asp:UpdatePanel ID="udp_ManageLeaders" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

<!-- BEGIN COMMENT UPDATE PROGRESS -->

    <asp:UpdateProgress ID="UpdateProgress_ManageLeaders" runat="server">
        <ProgressTemplate>
            <span style="color:Green;">UPDATE IN PROGRESS...</span>
        </ProgressTemplate>
    </asp:UpdateProgress>

<!--BEGIN HIDDEN LABELS-->
          <asp:Label ID="lbl_newLeaderID" runat="server" Visible="False"></asp:Label>


<asp:LinkButton ID="lbn_addLeaderDiv" runat="server" Font-Size="14px" Font-Bold="true" onclick="lbn_addLeaderDiv_Click" ToolTip="Show Add Leader">Add New Leader</asp:LinkButton>
<br /><br />

<!--BEGIN ADD LEADER DIV-->
<div id="addLeader" runat="server" visible="false">
    New Leader Email:<br /><asp:TextBox ID="txt_AddUserEmail" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserEmail" runat="server" ControlToValidate="txt_AddUserEmail" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    New Leader Title:<br /><asp:TextBox ID="txt_AddUserTitle" runat="server"></asp:TextBox><br />
    New Leader First Name:<br /><asp:TextBox ID="txt_AddUserFirstName" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserFirstName" runat="server" ControlToValidate="txt_AddUserFirstName" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    New Leader Middle Name:<br /><asp:TextBox ID="txt_AddUserMiddleName" runat="server"></asp:TextBox><br />
    New Leader Last Name:<br /><asp:TextBox ID="txt_AddUserLastName" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserLastName" runat="server" ControlToValidate="txt_AddUserLastName" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    New Leader Login Name:<br /><asp:TextBox ID="txt_AddUserLogin" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserLogin" runat="server" ControlToValidate="txt_AddUserLogin" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator>
    <asp:CustomValidator ID="val_checkUserLogin" runat="server" ErrorMessage="User Name Already Exists" ControlToValidate="txt_AddUserLogin" OnServerValidate="DuplicateNameCheck_ServerValidate" ForeColor="Red" ValidationGroup="AddUser"></asp:CustomValidator><br />
    New Leader Password:<br /><asp:TextBox ID="txt_addPassword" runat="server" TextMode="Password"></asp:TextBox>
    <asp:RequiredFieldValidator ID="val_addPassword" runat="server" ControlToValidate="txt_addPassword" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="val_PasswordLength" runat="server" ControlToValidate="txt_addPassword" ErrorMessage="* Minimum password length is 7" ValidationExpression=".{7}.*"  ForeColor="Red" ValidationGroup="AddUser" />
    <br />    
    Confirm Password:<br /><asp:TextBox ID="txt_confirmPassword" runat="server" TextMode="Password"></asp:TextBox>
     <asp:RequiredFieldValidator ControlToValidate="txt_confirmPassword" CssClass="failureNotification" ErrorMessage="Confirm Password is required." ID="ConfirmPasswordRequired" runat="server" 
                                     ToolTip="Confirm Password is required." ValidationGroup="AddUser" ForeColor="Red"><strong>X</strong></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="txt_addPassword" ControlToValidate="txt_confirmPassword" 
                                     CssClass="failureNotification" ErrorMessage="The Password and Confirmation Password must match."
                                      ForeColor="Red" ValidationGroup="AddUser"><strong>X</strong></asp:CompareValidator>
    <br /><br />
    <asp:Button ID="btn_AddUser" runat="server" Text="Create New Leader" onclick="btn_AddUser_Click" ValidationGroup="AddUser" ToolTip="Create New Leader" />
</div>
<!--END ADD LEADER DIV-->


<br />

<asp:LinkButton ID="lbn_editLeaderDiv" runat="server" Font-Size="14px" Font-Bold="true" onclick="lbn_editLeaderDiv_Click" ToolTip="Show Edit Leaders">Edit Leaders</asp:LinkButton>
<br /><br />
<!--BEGIN EDIT LEADER DIV-->
<div id="editLeaders" runat="server" visible="false">           

    <asp:ListView ID="ManageUser_ListView" runat="server" DataSourceID="SqlDataSource_ManageUser" OnItemCommand="ManageUser_ListView_OnItemCommand">

        <LayoutTemplate>

            <asp:Button ID="btn_sortNew" runat="server" Text="Sort by Date" CommandName="Sort" CommandArgument="UserDate" ToolTip="Sort By Date" />
            <asp:Button ID="btn_sortFirstName" runat="server" Text="Sort by First Name" CommandName="Sort" CommandArgument="UserFirstName" ToolTip="Sort By First Name" />
            <asp:Button ID="btn_sortLastName" runat="server" Text="Sort by Last Name" CommandName="Sort" CommandArgument="UserLastName" ToolTip="Sort By Last Name" />
            <asp:Button ID="btn_sortEmail" runat="server" Text="Sort by Email" CommandName="Sort" CommandArgument="UserEmail" ToolTip="Sort By Email" />
            <div>
            <br />
            </div>
        <div ID="itemPlaceholderContainer" runat="server" style="">
            <span runat="server" id="itemPlaceholder" />
        </div>
            <div>
            <br />
            </div>
             <asp:DataPager ID="DataPager_Users" runat="server" PageSize="20">
                <Fields>
                <asp:NumericPagerField ButtonType="Button" />
                </Fields>
            </asp:DataPager>

        </LayoutTemplate>


        <ItemTemplate>
        <asp:ImageButton ID="imb_editUser" runat="server" CommandName="Edit" ImageUrl="~/Images/Interface/pencil.png" ToolTip="Edit" AlternateText="Edit" />

            &nbsp;&nbsp;
            <strong>Email:</strong>&nbsp;<%#Eval("UserEmail")%>&nbsp;&nbsp;
            <strong>Title:</strong>&nbsp;<%#Eval("UserTitle")%>&nbsp;&nbsp;
            <strong>First Name:</strong>&nbsp;<%#Eval("UserFirstName")%>&nbsp;&nbsp;
            <strong>Middle Name:</strong>&nbsp;<%#Eval("UserMiddleName")%>&nbsp;&nbsp;
            <strong>Last Name:</strong>&nbsp;<%#Eval("UserLastName")%>&nbsp;&nbsp;
            <hr />
        </ItemTemplate>


        <EditItemTemplate>

            <br />
            <asp:Label ID="lbl_RowUserID" runat="server" Text='<%#Eval("UserID")%>' Visible="false"></asp:Label>
            <asp:ImageButton ID="imb_cancelEditUser" runat="server" CommandName="Cancel" ImageUrl="~/Images/Interface/cancel.png" ToolTip="Cancel" AlternateText="Cancel" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="imb_updateEditUser" runat="server" CommandName="Update" CommandArgument='<%#Eval("OrgID")%>' ImageUrl="~/Images/Interface/approved.png" ToolTip="Update" AlternateText="Update" />        
            <br /><br />
            Email:&nbsp;<asp:TextBox ID="txt_UserEmail" runat="server" Text='<%#Bind("UserEmail")%>'></asp:TextBox><br />
            Title:&nbsp;<asp:TextBox ID="txt_UserTitle" runat="server" Text='<%#Bind("UserTitle")%>'></asp:TextBox><br />
            First Name:&nbsp;<asp:TextBox ID="txt_UserFirstName" runat="server" Text='<%#Bind("UserFirstName")%>'></asp:TextBox><br />
            Middle Name:&nbsp;<asp:TextBox ID="txt_UserMiddleName" runat="server" Text='<%#Bind("UserMiddleName")%>'></asp:TextBox><br />
            Last Name:&nbsp;<asp:TextBox ID="txt_UserLastName" runat="server" Text='<%#Bind("UserLastName")%>'></asp:TextBox><br />
            <br /><br />

        </EditItemTemplate>

    </asp:ListView>

</div>
<!--END EDIT LEADER DIV-->
        </ContentTemplate>
    </asp:UpdatePanel>
<!--END UPDATE PANEL -->



<!--CLEAR DIV -->
<div style="clear:both;"></div>

<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="SqlDataSource_ManageUser" runat="server" 
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
        SelectCommand="SELECT * FROM tbl_UsersMaster WHERE OrgID = @OrgID AND isLeader = 'True'"
        UpdateCommand="UPDATE tbl_UsersMaster SET UserEmail = @UserEmail, UserTitle = @UserTitle, UserFirstName = @UserFirstName, UserMiddleName = @UserMiddleName, UserLastName = @UserLastName WHERE UserID = @UserID">

   <SelectParameters>
    <asp:Parameter Name="OrgID" DefaultValue="0" />
  </SelectParameters>

  <UpdateParameters>
    <asp:Parameter Name="UserID" DefaultValue="0" />
    <asp:Parameter Name="UserEmail" DefaultValue="0" />
    <asp:Parameter Name="UserTitle" DefaultValue="0" />
    <asp:Parameter Name="UserFirstName" DefaultValue="0" />
    <asp:Parameter Name="UserMiddleName" DefaultValue="0" />
    <asp:Parameter Name="UserLastName" DefaultValue="0" /> 
  </UpdateParameters>


    </asp:SqlDataSource>
</asp:Content>
