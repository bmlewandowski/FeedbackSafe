<%@ Page Title="" Language="C#" MasterPageFile="~/Agent/Agent.Master" AutoEventWireup="true" CodeBehind="Interact.aspx.cs" Inherits="FeedbackSafe.Agent.Interact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<!--LOAD STYLESHEETS-->
    <link href="/Styles/interact.css" rel="stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<!--BEGIN UPDATE PANEL-->
    <asp:UpdatePanel ID="udp_Comments" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

<!--BEGIN HIDDEN LABELS-->            
    <asp:Label ID="lbl_LeaderID" runat="server" Text="Label" Visible="false"></asp:Label>
    <asp:Label ID="lbl_leaderAskedAspnet" runat="server" Visible="false"></asp:Label>

<!--BEGIN PRIVATE BADGE-->
    <asp:Image ID="img_private" runat="server" ImageUrl="~/Images/Interface/private.png" style="vertical-align:middle;" alt="PRIVATE" ToolTip="PRIVATE" Visible="false"  />

<!--BEGIN LEADER INFO-->
    <asp:Image ID="img_askedLeader" runat="server" ImageUrl="~/Images/Interact/leaderPic.jpg" style="vertical-align:middle; height:100px;" alt=""  />
    <strong>Conversation with <asp:Label ID="lbl_AskedLeader" runat="server" Visible="true"></asp:Label></strong>
    <br /><br />

<!-- BEGIN COMMENT UPDATE PROGRESS -->

    <asp:UpdateProgress ID="UpdateProgress_Comments" runat="server">
        <ProgressTemplate>
            <span style="color:Green;">UPDATE IN PROGRESS...</span>
        </ProgressTemplate>
    </asp:UpdateProgress>

<!-- END LIST COMMENT PROGRESS -->

<asp:TextBox ID="txt_addComment" runat="server" Height="200px" Width="450px" TextMode="MultiLine"></asp:TextBox>
&nbsp;
<asp:ImageButton ID="btn_addComment" runat="server" ImageUrl="~/Images/Interface/add.png" ValidationGroup="AddComment" ToolTip="Add Comment" onclick="btn_addComment_Click" />
<asp:RequiredFieldValidator ID="val_addComment" runat="server" ControlToValidate="txt_addComment" ErrorMessage="* Required" ValidationGroup="AddComment"></asp:RequiredFieldValidator>
<br /><br /><br />


<!--BEGIN COMMENT LISTVIEW-->
<asp:ListView ID="Comment_ListView" runat="server" DataSourceID="SqlDataSource_Comments" OnItemDataBound="Comment_ListView_OnItemDataBound" OnItemCommand="Comment_ListView_OnItemCommand">
    
    <LayoutTemplate>

        <div style="">
            <asp:Button ID="btn_sortNew" runat="server" Text="Sort By Date" CommandName="Sort" CommandArgument="CommentDate" />
            <strong>Select Page:</strong>
                <asp:DataPager ID="DataPager_Comments" runat="server" PageSize="5">
                    <Fields>
                    <asp:NumericPagerField ButtonType="Button" />
                    </Fields>
                </asp:DataPager>
            <div>
                <br />
            </div>
        </div>
        <div ID="itemPlaceholderContainer" runat="server" style="">
            <span runat="server" id="itemPlaceholder" />
        </div>

    </LayoutTemplate>
    
    <ItemTemplate>

        <div class="interact_message">
            <div class="interact_leaderpic">
                <asp:Image ID="img_leaderPic" runat="server" ImageUrl="~/Images/Interact/leaderPic.jpg" AlternateText="" Visible="false" />&nbsp;
                <br />
                <asp:Label ID="lbl_leaderTitle" runat="server" Text=""></asp:Label>
                <br />
                <asp:CheckBox ID="chk_isApproved" runat="server" Enabled="false" Visible="false" />                
            </div>
            <div class="interact_messagemain">
<!--BEGIN BUBBLE TOP-->
                <asp:Panel id="pnl_bubbleTop" runat="server" CssClass="interact_pbubbletop">
                </asp:Panel>
<!--BEGIN BUBBLE BODY-->
                <asp:Panel id="pnl_bubbleMain" runat="server" CssClass="interact_pbubblemain">
                <asp:ImageButton ID="imb_setApproved" runat="server" CommandName="Approve" CommandArgument='<%#Eval("CommentID")%>' ImageUrl="/Images/Interface/not_approved.png" Width="30px" CssClass="interact_approve" />
                    <asp:Image ID="img_bubbleTail" runat="server" ImageUrl="~/Images/Interact/pbubbletail.jpg" CssClass="interact_pbubbletail" AlternateText="" />
                        <%#Eval("CommentText")%>
                </asp:Panel>
<!--BEGIN BUBBLE BOTTOM-->
                <asp:Panel id="pnl_bubbleBottom" runat="server" CssClass="interact_pbubblebottom">

                    <div class="interact_date">
                        <%#Eval("CommentDate")%> 
                    </div>
                 
                    <div class="interact_edit">
                        <asp:LinkButton ID="lbn_editComment" runat="server" CommandName="Edit" ToolTip="Edit Comment">edit</asp:LinkButton>
                    </div>
                </asp:Panel>
<!--BEGIN BUBBLE SPACER-->
                <div class="interact_messagespacer"> </div>		
            </div>
				<div class="interact_personpic">	
					 <asp:Image ID="img_personPic" runat="server" ImageUrl="~/Images/Interact/personPic.jpg" AlternateText="" Visible="true" />&nbsp;
				</div>	
				<div class="interact_messagespacer"></div>		
			</div>        

    </ItemTemplate>


    <EditItemTemplate>

        <asp:TextBox ID="txt_editComment" runat="server" Height="150px" Width="450px" TextMode="MultiLine" Text='<%# Bind("CommentText") %>'></asp:TextBox>
        <br />
            <asp:ImageButton ID="imb_cancelEditComment" runat="server" CommandName="Cancel" ImageUrl="~/Images/Interface/cancel.png" ToolTip="Cancel" AlternateText="Cancel" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="imb_updateEditCommment" runat="server" CommandName="Update" CommandArgument='<%#Eval("CommentID")%>' ImageUrl="~/Images/Interface/approved.png" ToolTip="Update" AlternateText="Update" />
        <br /><br />
    </EditItemTemplate>

</asp:ListView>
<!--END COMMENT LISTVIEW-->

        </ContentTemplate>
    </asp:UpdatePanel>
<!--END UPDATE PANEL -->

<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="SqlDataSource_Comments" runat="server" 
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
        SelectCommand="SELECT tbl_UsersMaster.UserID, tbl_UsersMaster.UserTitle, tbl_UsersMaster.UserFirstName, tbl_UsersMaster.UserMiddleName, tbl_UsersMaster.UserLastName, tbl_UsersMaster.isLeader, tbl_Comments.CommentID, tbl_Comments.ConversationID, tbl_Comments.UserID, tbl_Comments.CommentText, tbl_Comments.CommentPrivate, tbl_Comments.CommentApproved, tbl_Comments.CommentDate, tbl_Conversations.ConversationID, tbl_Conversations.OrgID, tbl_Conversations.UserID, tbl_Conversations.LeaderID, tbl_Conversations.ConversationPrivate, tbl_Conversations.ConversationApproved, tbl_Conversations.ConversationDate, tbl_UsersMaster.aspnetId 
        FROM tbl_Comments, tbl_Conversations, tbl_UsersMaster 
        WHERE tbl_Comments.ConversationID =@ConversationID AND tbl_Comments.ConversationID = tbl_Conversations.ConversationID AND tbl_Comments.UserID = tbl_UsersMaster.UserID AND tbl_UsersMaster.UserID = tbl_Comments.UserID 
        ORDER BY CommentDate ASC"
        UpdateCommand="Update tbl_Comments SET CommentApproved = @Approved, CommentText = @CommentText WHERE CommentID = @CommentID">

   <SelectParameters>
    <asp:Parameter Name="ConversationID" DefaultValue="0" />
  </SelectParameters>

   <UpdateParameters>
   <asp:Parameter Name="Approved" DefaultValue="0" /> 
   <asp:Parameter Name="CommentID" DefaultValue="0" />
   <asp:Parameter Name="CommentText" />
  </UpdateParameters>

    </asp:SqlDataSource>
<!--END SQL DATA SOURCE -->

</asp:Content>

