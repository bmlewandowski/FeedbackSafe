<%@ Page Title="" Language="C#" MasterPageFile="~/Leader/Leader.Master" AutoEventWireup="true" CodeBehind="ManageOrg.aspx.cs" Inherits="FeedbackSafe.Leader.ManageOrg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Manage Organization</h2>

    <div style="float:right; padding:10px">
 
        <h3>Upload Leader Image</h3>

            <asp:Image ID="img_user" runat="server" ImageUrl="~/Images/Interact/leaderPic.jpg" />
            <br /><br />
            <asp:FileUpload ID="upl_userimage" runat="server" />
            <br />
            <asp:RegularExpressionValidator ID="val_validimage" ControlToValidate="upl_userimage" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$" Text="Invalid image type" runat="server" ForeColor="Red" ValidationGroup="UserImage" />
            <br /><br />
            <asp:Button ID="btn_UserImage" runat="server" Text="Upload Image" onclick="btn_UserImage_Click" ValidationGroup="UserImage" ToolTip="Upload New User Image"  />
             
    </div>

<!--BEGIN UPDATE PANEL-->
    <asp:UpdatePanel ID="udp_ManageOrg" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

<!-- BEGIN COMMENT UPDATE PROGRESS -->

    <asp:UpdateProgress ID="UpdateProgress_ManageOrg" runat="server">
        <ProgressTemplate>
            <span style="color:Green;">UPDATE IN PROGRESS...</span>
        </ProgressTemplate>
    </asp:UpdateProgress>

<asp:ListView ID="ManageOrg_ListView" runat="server" DataSourceID="SqlDataSource_ManageOrg" OnItemCommand="ManageOrg_ListView_OnItemCommand" OnItemDataBound="ManageOrg_ListView_OnItemDataBound" >

    <LayoutTemplate>

        <div ID="itemPlaceholderContainer" runat="server" style="">
            <span runat="server" id="itemPlaceholder" />
        </div>
 
    </LayoutTemplate>


    <ItemTemplate>
        <asp:ImageButton ID="imb_editOrg" runat="server" CommandName="Edit" ImageUrl="~/Images/Interface/pencil.png" ToolTip="Edit" AlternateText="Edit" />
        <asp:LinkButton ID="lbn_editOrg" runat="server" CommandName="Edit" ToolTip="Edit">EDIT</asp:LinkButton>
        <br /><br /><br />
        <strong>Auto-Approve</strong><br /><br />
        <asp:CheckBox ID="chk_approveConversation" runat="server" Enabled="false" />New Conversations |
        <asp:CheckBox ID="chk_approveComment" runat="server" Enabled="false" />New Comments<br />
        <br />
        <strong>Recieve Emails</strong><br /><br />
        <asp:CheckBox ID="chk_conversationEmail" runat="server" Enabled="false" />For New Conversations | 
        <asp:CheckBox ID="chk_commentEmail" runat="server" Enabled="false" />For New Comments<br />
        <br />
        <asp:CheckBox ID="chk_profanityFilter" runat="server" Enabled="false" />Profanity Filter
        <br /><br />
        <strong>Name:</strong>&nbsp;<%#Eval("OrgName")%><br /><br /><strong>Address:</strong>&nbsp;<%#Eval("OrgAddress")%><br /><br /><strong>City:</strong>&nbsp;<%#Eval("OrgCity")%><br /><br /><strong>State:</strong>&nbsp;<%#Eval("OrgState")%><br /><br /><strong>Zip:</strong>&nbsp;<%#Eval("OrgZip")%><br /><br /><strong>Phone:</strong>&nbsp;<%#Eval("OrgPhone")%><br /><br /><strong>Phone 2:</strong>&nbsp;<%#Eval("OrgPhone2")%><br /><br /><strong>Fax:</strong>&nbsp;<%#Eval("OrgFax")%><br /><br /><strong>Email:</strong>&nbsp;<%#Eval("OrgEmail")%><br /><br /><strong>Contact:</strong>&nbsp;<%#Eval("OrgContact")%><br /><br /><strong>Description:</strong>&nbsp;<%#Eval("OrgDescription")%><br />

    </ItemTemplate>


    <EditItemTemplate>
        <asp:ImageButton ID="imb_cancelEditOrg" runat="server" CommandName="Cancel" ImageUrl="~/Images/Interface/cancel.png" ToolTip="Cancel" AlternateText="Cancel" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:ImageButton ID="imb_updateEditOrg" runat="server" CommandName="Update" CommandArgument='<%#Eval("OrgID")%>' ImageUrl="~/Images/Interface/approved.png" ToolTip="Update" AlternateText="Update" />      

        <br /><br />
        <strong>Auto-Approve</strong><br /><br />
        <asp:CheckBox ID="chk_toggleapproveConversation" runat="server" />New Conversations |
        <asp:CheckBox ID="chk_toggleapproveComment" runat="server" />New Comments<br />
        <br />
        <strong>Recieve Emails</strong><br /><br />
        <asp:CheckBox ID="chk_toggleconversationEmail" runat="server" />For New Conversations | 
        <asp:CheckBox ID="chk_togglecommentEmail" runat="server" />For New Comments<br />
        <br />
        <asp:CheckBox ID="chk_toggleprofanityFilter" runat="server" />Profanity Filter
        <br /><br />
        Name:&nbsp;<asp:TextBox ID="txt_OrgName" runat="server" Text='<%#Bind("OrgName")%>'></asp:TextBox><br /><br />
        Address:&nbsp;<asp:TextBox ID="txt_OrgAddress" runat="server" Text='<%#Bind("OrgAddress")%>'></asp:TextBox><br /><br />
        City:&nbsp;<asp:TextBox ID="txt_OrgCity" runat="server" Text='<%#Bind("OrgCity")%>'></asp:TextBox><br /><br />
        State:&nbsp;<asp:TextBox ID="txt_OrgState" runat="server" Text='<%#Bind("OrgState")%>'></asp:TextBox><br /><br />
        Zip:&nbsp;<asp:TextBox ID="txt_OrgZip" runat="server" Text='<%#Bind("OrgZip")%>'></asp:TextBox><br /><br />
        Phone:&nbsp;<asp:TextBox ID="txt_OrgPhone" runat="server" Text='<%#Bind("OrgPhone")%>'></asp:TextBox><br /><br />
        Phone 2:&nbsp;<asp:TextBox ID="txt_OrgPhone2" runat="server" Text='<%#Bind("OrgPhone2")%>'></asp:TextBox><br /><br />
        Fax:&nbsp;<asp:TextBox ID="txt_OrgFax" runat="server" Text='<%#Bind("OrgFax")%>'></asp:TextBox><br /><br />
        Email:&nbsp;<asp:TextBox ID="txt_OrgEmail" runat="server" Text='<%#Bind("OrgEmail")%>'></asp:TextBox><br /><br />
        Contact:&nbsp;<asp:TextBox ID="txt_OrgContact" runat="server" Text='<%#Bind("OrgContact")%>'></asp:TextBox><br /><br />
        Description:&nbsp;<br /><asp:TextBox ID="txt_OrgDescription" runat="server" Text='<%#Bind("OrgDescription")%>' Height="200px" Width="450px" TextMode="MultiLine"></asp:TextBox>

    </EditItemTemplate>

</asp:ListView>


        </ContentTemplate>
    </asp:UpdatePanel>
<!--END UPDATE PANEL -->


<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="SqlDataSource_ManageOrg" runat="server" 
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
        SelectCommand="SELECT * FROM tbl_OrgsMaster WHERE OrgID = @OrgID"
        UpdateCommand="UPDATE tbl_OrgsMaster SET OrgName = @OrgName, OrgAddress = @OrgAddress, OrgCity = @OrgCity, OrgState = @OrgState, OrgZip = @OrgZip, OrgPhone = @OrgPhone, OrgPhone2 = @OrgPhone2, OrgFax = @OrgFax, OrgEmail = @OrgEmail, OrgContact = @OrgContact, OrgDescription = @OrgDescription, autoApproveConversation = @autoApproveConversation, autoApproveComment = @autoApproveComment, conversationEmail = @conversationEmail, commentEmail = @commentEmail, profanityFilter = @profanityFilter  WHERE OrgID = @OrgID">

   <SelectParameters>
    <asp:Parameter Name="OrgID" DefaultValue="0" />
  </SelectParameters>

  <UpdateParameters>
    <asp:Parameter Name="OrgID" DefaultValue="0" />
    <asp:Parameter Name="OrgName" DefaultValue="0" />
    <asp:Parameter Name="OrgAddress" DefaultValue="0" />
    <asp:Parameter Name="OrgCity" DefaultValue="0" />
    <asp:Parameter Name="OrgState" DefaultValue="0" />
    <asp:Parameter Name="OrgZip" DefaultValue="0" />
    <asp:Parameter Name="OrgPhone" DefaultValue="0" />
    <asp:Parameter Name="OrgPhone2" DefaultValue="0" />
    <asp:Parameter Name="OrgFax" DefaultValue="0" />
    <asp:Parameter Name="OrgEmail" DefaultValue="0" />
    <asp:Parameter Name="OrgContact" DefaultValue="0" />
    <asp:Parameter Name="OrgDescription" DefaultValue="0" />
    <asp:Parameter Name="autoApproveConversation" DefaultValue="0" />
    <asp:Parameter Name="autoApproveComment" DefaultValue="0" />
    <asp:Parameter Name="conversationEmail" DefaultValue="0" />
    <asp:Parameter Name="commentEmail" DefaultValue="0" />
    <asp:Parameter Name="profanityFilter" DefaultValue="0" />
  </UpdateParameters>

    </asp:SqlDataSource>

</asp:Content>
