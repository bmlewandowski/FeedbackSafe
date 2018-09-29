<%@ Page Title="" Language="C#" MasterPageFile="~/Agent/Agent.Master" AutoEventWireup="true" CodeBehind="ManageSales.aspx.cs" Inherits="FeedbackSafe.Agent.ManageSales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1>Manage Sales</h1>
<br />

<!--BEGIN UPDATE PANEL-->
    <asp:UpdatePanel ID="udp_ManageSales" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

<!-- BEGIN COMMENT UPDATE PROGRESS -->

    <asp:UpdateProgress ID="UpdateProgress_ManageSales" runat="server">
        <ProgressTemplate>
            <span style="color:Green;">UPDATE IN PROGRESS...</span>
        </ProgressTemplate>
    </asp:UpdateProgress>

<!--BEGIN HIDDEN LABELS-->
          <asp:Label ID="lbl_newSalesID" runat="server" Visible="False"></asp:Label>


<asp:LinkButton ID="lbn_addSalesDiv" runat="server" Font-Size="14px" Font-Bold="true" onclick="lbn_addSalesDiv_Click" ToolTip="Show Add Sales">Add New Sales</asp:LinkButton>
<br /><br />

<!--BEGIN ADD LEADER DIV-->
<div id="addSales" runat="server" visible="false">

    New Sales Email:<br /><asp:TextBox ID="txt_AddUserEmail" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserEmail" runat="server" ControlToValidate="txt_AddUserEmail" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    New Sales First Name:<br /><asp:TextBox ID="txt_AddUserFirstName" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserFirstName" runat="server" ControlToValidate="txt_AddUserFirstName" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    New Sales Middle Name:<br /><asp:TextBox ID="txt_AddUserMiddleName" runat="server"></asp:TextBox><br />
    New Sales Last Name:<br /><asp:TextBox ID="txt_AddUserLastName" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserLastName" runat="server" ControlToValidate="txt_AddUserLastName" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    New Sales Address:<br /><asp:TextBox ID="txt_AddUserAddress" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserAddress" runat="server" ControlToValidate="txt_AddUserAddress" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    New Sales City:<br /><asp:TextBox ID="txt_AddUserCity" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserCity" runat="server" ControlToValidate="txt_AddUserCity" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    New Sales State:<br /><asp:DropDownList ID="ddl_AddUserState" runat="server"></asp:DropDownList><br />
    New Sales Zip:<br /><asp:TextBox ID="txt_AddUserZip" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserZip" runat="server" ControlToValidate="txt_AddUserZip" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    New Sales Phone:<br /><asp:TextBox ID="txt_AddUserPhone" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserPhone" runat="server" ControlToValidate="txt_AddUserPhone" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator><br />
    New Sales Phone2:<br /><asp:TextBox ID="txt_AddUserPhone2" runat="server"></asp:TextBox><br />
    Comments:<br /><asp:TextBox ID="txt_AddUserSalesComments" runat="server" 
        TextMode="MultiLine" Height="100px" Width="300px"></asp:TextBox><br />

    New Sales Login Name:<br /><asp:TextBox ID="txt_AddUserLogin" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="val_addUserLogin" runat="server" ControlToValidate="txt_AddUserLogin" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="AddUser"></asp:RequiredFieldValidator>
    <asp:CustomValidator ID="val_checkUserLogin" runat="server" ErrorMessage="User Name Already Exists" ControlToValidate="txt_AddUserLogin" OnServerValidate="DuplicateNameCheck_ServerValidate" ForeColor="Red" ValidationGroup="AddUser"></asp:CustomValidator><br />
    New Sales Password:<br /><asp:TextBox ID="txt_addPassword" runat="server" TextMode="Password"></asp:TextBox>
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
    <asp:Button ID="btn_AddUser" runat="server" Text="Create New Sales" onclick="btn_AddUser_Click" ValidationGroup="AddUser" ToolTip="Create New Sales" />
</div>
<!--END ADD LEADER DIV-->


<br />

<asp:LinkButton ID="lbn_editSalesDiv" runat="server" Font-Size="14px" Font-Bold="true" onclick="lbn_editSalesDiv_Click" ToolTip="Show Edit Sales">Edit Sales</asp:LinkButton>
<br /><br />
<!--BEGIN EDIT LEADER DIV-->
<div id="editSales" runat="server" visible="false">           
    <br />
    <asp:ListView ID="ManageUser_ListView" runat="server" DataSourceID="SqlDataSource_ManageUser" OnItemCommand="ManageUser_ListView_OnItemCommand">

        <LayoutTemplate>

            <asp:Button ID="btn_sortNew" runat="server" Text="Sort by Date" CommandName="Sort" CommandArgument="SalesDate" ToolTip="Sort By Date" />
            <asp:Button ID="btn_sortFirstName" runat="server" Text="Sort by First Name" CommandName="Sort" CommandArgument="SalesFirstName" ToolTip="Sort By First Name" />
            <asp:Button ID="btn_sortLastName" runat="server" Text="Sort by Last Name" CommandName="Sort" CommandArgument="SalesLastName" ToolTip="Sort By Last Name" />
            <asp:Button ID="btn_sortEmail" runat="server" Text="Sort by Email" CommandName="Sort" CommandArgument="SalesEmail" ToolTip="Sort By Email" />
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
            <strong>Email:</strong>&nbsp;<%#Eval("SalesEmail")%>&nbsp;&nbsp;
            <strong>First Name:</strong>&nbsp;<%#Eval("SalesFirstName")%>&nbsp;&nbsp;
            <strong>Middle Name:</strong>&nbsp;<%#Eval("SalesMiddleName")%>&nbsp;&nbsp;
            <strong>Last Name:</strong>&nbsp;<%#Eval("SalesLastName")%>&nbsp;&nbsp;
            <strong>Address:</strong>&nbsp;<%#Eval("SalesAddress")%>&nbsp;&nbsp;
            <strong>City:</strong>&nbsp;<%#Eval("SalesCity")%>&nbsp;&nbsp;
            <strong>State:</strong>&nbsp;<%#Eval("SalesState")%>&nbsp;&nbsp;
            <strong>Zip:</strong>&nbsp;<%#Eval("SalesZip")%>&nbsp;&nbsp;
            <strong>Phone:</strong>&nbsp;<%#Eval("SalesPhone")%>&nbsp;&nbsp;
            <strong>Phone2:</strong>&nbsp;<%#Eval("SalesPhone2")%>&nbsp;&nbsp;
            <strong>Comments:</strong>&nbsp;<%#Eval("SalesComments")%>&nbsp;&nbsp;
            <hr />
        </ItemTemplate>


        <EditItemTemplate>

            <br />
            <asp:Label ID="lbl_RowUserID" runat="server" Text='<%#Eval("SalesID")%>' Visible="false"></asp:Label>
            <asp:ImageButton ID="imb_cancelEditUser" runat="server" CommandName="Cancel" ImageUrl="~/Images/Interface/cancel.png" ToolTip="Cancel" AlternateText="Cancel" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="imb_updateEditUser" runat="server" CommandName="Update" CommandArgument='<%#Eval("SalesID")%>' ImageUrl="~/Images/Interface/approved.png" ToolTip="Update" AlternateText="Update" />        
            <br /><br />
            Email:&nbsp;<asp:TextBox ID="txt_SalesEmail" runat="server" Text='<%#Bind("SalesEmail")%>'></asp:TextBox><br />
            First Name:&nbsp;<asp:TextBox ID="txt_SalesFirstName" runat="server" Text='<%#Bind("SalesFirstName")%>'></asp:TextBox><br />
            Middle Name:&nbsp;<asp:TextBox ID="txt_SalesMiddleName" runat="server" Text='<%#Bind("SalesMiddleName")%>'></asp:TextBox><br />
            Last Name:&nbsp;<asp:TextBox ID="txt_SalesLastName" runat="server" Text='<%#Bind("SalesLastName")%>'></asp:TextBox><br />
            Address:&nbsp;<asp:TextBox ID="txt_SalesAddress" runat="server" Text='<%#Bind("SalesAddress")%>'></asp:TextBox><br />
            City:&nbsp;<asp:TextBox ID="txt_SalesCity" runat="server" Text='<%#Bind("SalesCity")%>'></asp:TextBox><br />
            State:&nbsp;<asp:TextBox ID="txt_SalesState" runat="server" Text='<%#Bind("SalesState")%>'></asp:TextBox><br />
            Zip:&nbsp;<asp:TextBox ID="txt_SalesZip" runat="server" Text='<%#Bind("SalesZip")%>'></asp:TextBox><br />
            Phone:&nbsp;<asp:TextBox ID="txt_SalesPhone" runat="server" Text='<%#Bind("SalesPhone")%>'></asp:TextBox><br />
            Phone2:&nbsp;<asp:TextBox ID="txt_SalesPhone2" runat="server" Text='<%#Bind("SalesPhone2")%>'></asp:TextBox><br />
            Comments:<br /><asp:TextBox ID="txt_SalesComments" runat="server" TextMode="MultiLine" Height="100px" Width="300px" Text='<%#Bind("SalesComments")%>'></asp:TextBox><br />
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
        SelectCommand="SELECT * FROM tbl_SalesMaster"
        UpdateCommand="UPDATE tbl_SalesMaster SET SalesEmail = @SalesEmail, SalesFirstName = @SalesFirstName, SalesMiddleName = @SalesMiddleName, SalesLastName = @SalesLastName, SalesAddress = @SalesAddress, SalesCity = @SalesCity, SalesState = @SalesState, SalesZip = @SalesZip, SalesPhone = @SalesPhone, SalesPhone2 = @SalesPhone2, SalesComments = @SalesComments WHERE SalesID = @SalesID">

   <SelectParameters>
  </SelectParameters>

  <UpdateParameters>
    <asp:Parameter Name="SalesID" DefaultValue="0" />
    <asp:Parameter Name="SalesEmail" DefaultValue="0" />
    <asp:Parameter Name="SalesFirstName" DefaultValue="0" />
    <asp:Parameter Name="SalesMiddleName" DefaultValue="0" />
    <asp:Parameter Name="SalesLastName" DefaultValue="0" />
    <asp:Parameter Name="SalesAddress" DefaultValue="0" />
    <asp:Parameter Name="SalesCity" DefaultValue="0" />
    <asp:Parameter Name="SalesState" DefaultValue="0" />
    <asp:Parameter Name="SalesZip" DefaultValue="0" />
    <asp:Parameter Name="SalesPhone" DefaultValue="0" />
    <asp:Parameter Name="SalesPhone2" DefaultValue="0" />
    <asp:Parameter Name="SalesComments" DefaultValue="0" />
  </UpdateParameters>


    </asp:SqlDataSource>
</asp:Content>
