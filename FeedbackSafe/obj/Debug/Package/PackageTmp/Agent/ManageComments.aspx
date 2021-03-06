﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Agent/Agent.Master" AutoEventWireup="true" CodeBehind="ManageComments.aspx.cs" Inherits="FeedbackSafe.Agent.ManageComments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<!--LOAD STYLESHEETS-->
    <link href="/Styles/leader.css" rel="stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<!--BEGIN HIDDEN LABELS-->
    <asp:Label ID="lbl_OrgID" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_ConvID" runat="server" Visible="false"></asp:Label>

<!--BEGIN UPDATE PANEL-->
    <asp:UpdatePanel ID="udp_Comments" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

<!-- BEGIN COMMENT UPDATE PROGRESS -->

    <asp:UpdateProgress ID="UpdateProgress_Comments" runat="server">
        <ProgressTemplate>
            <span style="color:Green;">UPDATE IN PROGRESS...</span>
        </ProgressTemplate>
    </asp:UpdateProgress>

<!-- END COMMENT UPDATE PROGRESS -->

<div id="leader_leftarea">
    <asp:Label ID="lbl_OrgName" runat="server"></asp:Label>'s Conversations
    <br /><br />

    <div id="leader_messagelist">

<!--BEGIN CONVERSATION LISTVIEW-->
    <asp:ListView ID="Conversation_ListView" runat="server" DataSourceID="SqlDataSource_Conversations" OnItemDataBound="Conversation_ListView_OnItemDataBound" OnItemCommand="Conversation_ListView_OnItemCommand">

    <LayoutTemplate>

        <strong>Sort By:</strong>
        <asp:Button ID="btn_sortUnread" runat="server" Text="Unread" CommandName="Sort" CommandArgument="ConversationUnread" ToolTip="Sort by Unread" />
        <asp:Button ID="btn_sortNew" runat="server" Text="Date" CommandName="Sort" CommandArgument="ConversationDate" ToolTip="Sort by Date" />

        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

        <strong>Page:</strong>
        <asp:DataPager ID="DataPager_Conversations" runat="server" PageSize="5">
        <Fields>
        <asp:NumericPagerField ButtonType="Button" ButtonCount="7"  />
        </Fields>
        </asp:DataPager>

        <table border="0">

        <div ID="itemPlaceholderContainer" runat="server" style="">
        <span runat="server" id="itemPlaceholder" />
        </div>

        <tr><td colspan="3"><img src="/Images/Interface/messagehr.gif" alt=""></td></tr>
        </table>

    </LayoutTemplate>

    
    <ItemTemplate>

        <tr>
        <td colspan="3">
         <img src="/Images/Interface/messagehr.gif" alt="" />
         </td>
        </tr>

        <tr>
        <td valign="middle" align="center" width="50" height="50">
        <asp:ImageButton ID="imb_setApproved" runat="server" CommandName="Approve" CommandArgument='<%#Eval("ConversationID")%>' ImageUrl="/Images/Interface/not_approved.png" />
        </td>

        <td valign="top">
        <%#Eval("ConversationDate", "{0:MM/dd/yyyy}")%>
        <br />
        <div class="leader_unread">
        <asp:Label ID="lbl_unreadComents" runat="server"></asp:Label>
        </div>
        <asp:CheckBox ID="chk_isApproved" runat="server" Enabled="false" Visible="false" />
        </td>

        <td valign="top">
            <div class="leader_commentbox">
                <asp:LinkButton ID="lbn_Conversation" runat="server" CommandName='<%#Eval("LeaderID")%>' CommandArgument='<%#Eval("ConversationID")%>' OnCommand="lbn_Conversation_Command" ToolTip='<%#Eval("CommentText")%>' CssClass="leader_comment"><%#Eval("CommentText")%></asp:LinkButton>
            </div>
        </td>
        </tr>

    </ItemTemplate>

</asp:ListView>
<!--END CONVERSATION LISTVIEW-->

    </div>
</div>

<div id="leader_rightarea">

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

<div id="leader_bottomarea">

</div>

        </ContentTemplate>
    </asp:UpdatePanel>
<!--END UPDATE PANEL -->

<!--CLEAR DIV -->
<div style="clear:both;"></div>

<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="SqlDataSource_Conversations" runat="server" 
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
        SelectCommand="SELECT * FROM tbl_Comments as comm INNER JOIN tbl_Conversations as conv ON conv.ConversationID=comm.ConversationID WHERE CommentDate in( SELECT MIN(CommentDate) 
        FROM tbl_Comments GROUP BY ConversationID ) AND conv.OrgID = @OrgID
        ORDER BY ConversationDate DESC"
        UpdateCommand="UPDATE tbl_Conversations SET ConversationApproved = @Approved WHERE ConversationID = @ConversationID">

   <SelectParameters>
    <asp:Parameter Name="OrgID" DefaultValue="0" />
  </SelectParameters>

  <UpdateParameters>
   <asp:Parameter Name="Approved" DefaultValue="0" /> 
   <asp:Parameter Name="ConversationID" DefaultValue="0" />
  </UpdateParameters>

    </asp:SqlDataSource>
<!--END SQL DATA SOURCE -->

</asp:Content>
