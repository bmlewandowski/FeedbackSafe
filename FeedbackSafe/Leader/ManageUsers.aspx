<%@ Page Title="" Language="C#" MasterPageFile="~/Leader/Leader.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="FeedbackSafe.Leader.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1>Manage Users</h1>
<br />
<!--BEGIN UPDATE PANEL-->
    <asp:UpdatePanel ID="udp_ManageUsers" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

<!-- BEGIN COMMENT UPDATE PROGRESS -->

    <asp:UpdateProgress ID="UpdateProgress_ManageUsers" runat="server">
        <ProgressTemplate>
            <span style="color:Green;">UPDATE IN PROGRESS...</span>
        </ProgressTemplate>
    </asp:UpdateProgress>


<asp:LinkButton ID="lbn_addUserDiv" runat="server" Font-Size="14px" Font-Bold="true" onclick="lbn_addUserDiv_Click" ToolTip="Show Add User">Add New User</asp:LinkButton>
<br /><br />

<!--BEGIN ADD USER DIV-->
<div id="addUser" runat="server" visible="false">

    User Email:<br /><asp:TextBox ID="txt_AddUserEmail" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserEmail" runat="server" ControlToValidate="txt_AddUserEmail" ErrorMessage="* Required" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    User Title:<br /><asp:TextBox ID="txt_AddUserTitle" runat="server"></asp:TextBox><br />
    User First Name:<br /><asp:TextBox ID="txt_AddUserFirstName" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserFirstName" runat="server" ControlToValidate="txt_AddUserFirstName" ErrorMessage="* Required" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    User Middle Name:<br /><asp:TextBox ID="txt_AddUserMiddleName" runat="server"></asp:TextBox><br />
    User Last Name:<br /><asp:TextBox ID="txt_AddUserLastName" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserLastName" runat="server" ControlToValidate="txt_AddUserLastName" ErrorMessage="* Required" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    <br />    
    <asp:Button ID="btn_AddUser" runat="server" Text="Add and Notify New User" onclick="btn_AddUser_Click" ValidationGroup="AddUser" ToolTip="Add and Notify New User" />
    <br /><br />
</div>
<!--END ADD USER DIV-->

<br />

<asp:LinkButton ID="lbn_editUserDiv" runat="server" Font-Size="14px" Font-Bold="true" ToolTip="Show Edit Users" onclick="lbn_editUserDiv_Click">Edit Users</asp:LinkButton>
<br /><br />

<!--BEGIN EDIT USER DIV-->
<div id="editUsers" runat="server" visible="false">

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
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;        
            <asp:ImageButton ID="lbn_sendWelcome" runat="server" CommandName="Email" ImageUrl="~/Images/Interface/mail.png" ToolTip="Email User" AlternateText="Email User" />     
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;        
            <asp:ImageButton ID="imb_deleteUser" runat="server" CommandName="Delete" CommandArgument='<%#Eval("UserID")%>' OnClientClick="return confirm('Are you SURE you want to Delete this User?');" ImageUrl="~/Images/Interface/delete.png" ToolTip="Delete User" AlternateText="Delete User" />
     
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
<!--END EDIT USER DIV-->

        </ContentTemplate>
    </asp:UpdatePanel>
<!--END UPDATE PANEL -->

<!--CLEAR DIV -->
<div style="clear:both;"></div>

<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="SqlDataSource_ManageUser" runat="server" 
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
        SelectCommand="SELECT * FROM tbl_UsersMaster WHERE OrgID = @OrgID AND isLeader = 'False'"
        UpdateCommand="UPDATE tbl_UsersMaster SET UserEmail = @UserEmail, UserTitle = @UserTitle, UserFirstName = @UserFirstName, UserMiddleName = @UserMiddleName, UserLastName = @UserLastName WHERE UserID = @UserID"
        DeleteCommand="DELETE FROM tbl_UsersMaster WHERE UserID = @UserID">

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

  <DeleteParameters>
    <asp:Parameter Name="UserID" DefaultValue="0" />
  </DeleteParameters>

    </asp:SqlDataSource>

</asp:Content>
