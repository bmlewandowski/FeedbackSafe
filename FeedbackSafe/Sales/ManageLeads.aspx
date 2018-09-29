<%@ Page Title="" Language="C#" MasterPageFile="~/Sales/Sales.Master" AutoEventWireup="true" CodeBehind="ManageLeads.aspx.cs" Inherits="FeedbackSafe.Sales.ManageLeads" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Manage Leads</h1>

<!--BEGIN UPDATE PANEL-->
    <asp:UpdatePanel ID="udp_Leads" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

<!-- BEGIN LEAD UPDATE PROGRESS -->

    <asp:UpdateProgress ID="UpdateProgress_Comments" runat="server">
        <ProgressTemplate>
            <span style="color:Green;">UPDATE IN PROGRESS...</span>
        </ProgressTemplate>
    </asp:UpdateProgress>

<!--BEGIN LEADS LISTVIEW-->
    <asp:ListView ID="Leads_ListView" runat="server" DataSourceID="SqlDataSource_Leads" OnItemDataBound="Leads_ListView_OnItemDataBound" OnItemCommand="Leads_ListView_OnItemCommand">

    <LayoutTemplate>

        <strong>Sort By:</strong>
        <asp:Button ID="btn_sortCompanyName" runat="server" Text="Company Name" CommandName="Sort" CommandArgument="CompanyName" ToolTip="Sort by Company Name" />
        <asp:Button ID="btn_sortSalesID" runat="server" Text="SalesID" CommandName="Sort" CommandArgument="SalesID" ToolTip="Sort by SalesID" />
        <asp:Button ID="btn_sortStatus" runat="server" Text="Status" CommandName="Sort" CommandArgument="LeadStatus" ToolTip="Sort by Status" />
        <asp:Button ID="btn_SortFollowUpDate" runat="server" Text="Follow-Up" CommandName="Sort" CommandArgument="FollowUpDate" ToolTip="Sort by Follow-Up Date" />

        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

        <strong>Page:</strong>
        <asp:DataPager ID="DataPager_Leads" runat="server" PageSize="25">
        <Fields>
        <asp:NumericPagerField ButtonType="Button" ButtonCount="10"  />
        </Fields>
        </asp:DataPager>
        <br /><br />
        <div ID="itemPlaceholderContainer" runat="server" style="">
        <span runat="server" id="itemPlaceholder" />
        </div>


    </LayoutTemplate>

    <ItemTemplate>
    <asp:ImageButton ID="imb_editLead" runat="server" CommandName="Edit" ImageUrl="~/Images/Interface/pencil.png" ToolTip="Edit" AlternateText="Edit"/>
    &nbsp;|
    <%#Eval("SalesID")%>&nbsp;|
    <%#Eval("LeadStatus")%>&nbsp;|
    <%#Eval("CompanyName")%>&nbsp;|
    <%#Eval("Address")%>&nbsp;|
    <%#Eval("City")%>,
    <%#Eval("State")%>,
    <%#Eval("Zip")%>&nbsp;|
    <%#Eval("Phone")%>

    <br />
    <hr />
    </ItemTemplate>

    <EditItemTemplate>

     <asp:Label ID="lbl_RowLeadID" runat="server" Text='<%#Eval("LeadID")%>' Visible="false"></asp:Label>
    <asp:ImageButton ID="imb_cancelEditLead" runat="server" CommandName="Cancel" ImageUrl="~/Images/Interface/cancel.png" ToolTip="Cancel" AlternateText="Cancel" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:ImageButton ID="imb_updateEditLead" runat="server" CommandName="Update" CommandArgument='<%#Eval("LeadID")%>' ImageUrl="~/Images/Interface/approved.png" ToolTip="Update" AlternateText="Update" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    Status:
    <asp:DropDownList ID="ddl_LeadStatus" runat="server">
        <asp:ListItem>OPEN</asp:ListItem>
        <asp:ListItem>ACTIVE</asp:ListItem>
        <asp:ListItem>SEND INFO</asp:ListItem>
        <asp:ListItem>CALLBACK</asp:ListItem>
        <asp:ListItem>MEET</asp:ListItem>
        <asp:ListItem>NO INTEREST</asp:ListItem>
    </asp:DropDownList>
       
      <br /><br />  
    
    SalesID: <%#Eval("SalesID")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

     <asp:CheckBox ID="chk_SalesClaim" runat="server" ToolTip="Checking this box puts this lead under your SalesID" />Claim
     <br /><br />
     <strong><%#Eval("CompanyName")%></strong><br />
    <%#Eval("Address")%><br />
    <%#Eval("City")%>, <%#Eval("State")%><br />
    <%#Eval("Zip")%>
    <br /><br />
    <strong>Contact:</strong>
    <br />
    <%#Eval("ContactPrefix")%> <%#Eval("ContactFirstName")%> <%#Eval("ContactLastName")%>, <%#Eval("ContactTitle")%>
    <br />
    <%#Eval("Phone")%>
    <br /><br />
    <a href='<%#Eval("Website")%>' target="_blank" title='<%#Eval("CompanyName")%>'><%#Eval("Website")%></a>
    <br />
    Industry: <%#Eval("Industry")%>
    <br /><br />
    Comments:<br />
    <asp:TextBox ID="txt_Comment" runat="server" Text='<%#Bind("Comment")%>' Height="200px" Width="450px" TextMode="MultiLine"></asp:TextBox>
    <br /><br />
    <asp:CheckBox ID="chk_markFollowup" runat="server" ToolTip="Checking this box enables the Calendar control below for setting a follow-up date" /> Follow-up&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#Eval("FollowUpDate")%>
    <br /><br />
    <asp:Calendar ID="cal_Followup" runat="server"></asp:Calendar>
    <br /><br />

    </EditItemTemplate>

    </asp:ListView>

<!--END LEADS LISTVIEW-->



        </ContentTemplate>
    </asp:UpdatePanel>
<!--END UPDATE PANEL -->


<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="SqlDataSource_Leads" runat="server" 
        ConnectionString="<%$ ConnectionStrings:FeedBackSalesDB %>" 
        SelectCommand="SELECT * FROM tbl_LeadsMaster LEFT JOIN tbl_LeadDetail ON tbl_LeadsMaster.LeadID=tbl_LeadDetail.LeadID"
        UpdateCommand="UPDATE tbl_LeadsMaster SET tbl_LeadsMaster.LeadStatus=@LeadStatus, tbl_LeadsMaster.SalesID=@SalesID,tbl_LeadsMaster.FollowUpDate =@FollowUpDate, tbl_LeadsMaster.Comment =@Comment WHERE LeadID = @LeadID">

   <SelectParameters>
    <asp:Parameter Name="OrgID" DefaultValue="0" />
  </SelectParameters>

  <UpdateParameters>
   <asp:Parameter Name="LeadStatus" DefaultValue="0" /> 
   <asp:Parameter Name="SalesID" DefaultValue="0" />
   <asp:Parameter Name="Comment" DefaultValue="0" />
   <asp:Parameter Name="FollowUpDate" DefaultValue="1900-01-01" />
   <asp:Parameter Name="LeadID" DefaultValue="0" />
  </UpdateParameters>

    </asp:SqlDataSource>
<!--END SQL DATA SOURCE -->

</asp:Content>
