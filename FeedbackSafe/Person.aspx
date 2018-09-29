<%@ Page Title="" Language="C#" MasterPageFile="~/Private.Master" AutoEventWireup="true" CodeBehind="Person.aspx.cs" Inherits="FeedbackSafe.Person" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<!--LOAD STYLESHEETS-->
    <link href="/Styles/person.css" rel="stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<!--BEGIN HIDDEN LABELS-->
    <asp:Label ID="lbl_Token" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_UserID" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_OrgID" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_ConvID" runat="server" Visible="false"></asp:Label>

<!--BEGIN UPDATE PANEL-->
    <asp:UpdatePanel ID="udp_Conversations" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

            <div id="person_leftarea">

            <!-- BEGIN CONVERSATION UPDATE PROGRESS -->

                <asp:UpdateProgress ID="UpdateProgress_Conversations" runat="server">
                    <ProgressTemplate>
                        <span style="color:Green;">UPDATE IN PROGRESS...</span>
                    </ProgressTemplate>
                </asp:UpdateProgress>

            <!-- END CONVERSATION UPDATE PROGRESS -->

                <asp:LinkButton ID="lbn_publicConversations" runat="server" Font-Size="16px"  Font-Bold="true" ToolTip="Show Public Conversations"  onclick="lbn_publicConversations_Click">Public Conversations</asp:LinkButton>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="lbn_privateConversations" runat="server" Font-Size="16px" Font-Bold="true" ToolTip="Show Private Conversations"  onclick="lbn_privateConversations_Click">My Private Conversations</asp:LinkButton>

                <br /><br /><br />

        <div id="divPublicConv" runat="server">
            <div style="font-size:20px; padding-bottom:25px;">
                <asp:Label ID="lbl_OrgName" runat="server" Visible="false"></asp:Label>Public Conversations
            </div>

<!--BEGIN CONVERSATION LISTVIEW-->
            <asp:ListView ID="Conversation_ListView" runat="server" DataSourceID="SqlDataSource_Conversations" OnItemDataBound="Conversation_ListView_OnItemDataBound">

                <LayoutTemplate>
    
        
                    <asp:Button ID="btn_sortNew" runat="server" Text="Sort By Date" CommandName="Sort" CommandArgument="ConversationDate" ToolTip="Sort by Date" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;    
                    Page
                        <asp:DataPager ID="DataPager_Conversations" runat="server" PageSize="5">
                            <Fields>
                            <asp:NumericPagerField ButtonType="Button" />
                            </Fields>          
                        </asp:DataPager>    
                        <br />
                        <img src="/Images/Interface/messagehr.gif" alt="">
                        <table>
                    <div ID="itemPlaceholderContainer" runat="server" style="">
                        <span runat="server" id="itemPlaceholder" />
                    </div>
                        </table>
                </LayoutTemplate>


                <ItemTemplate>

                    <tr>
                        <td>
                            <div class="person_date">
                                <%#Eval("ConversationDate", "{0:MM/dd/yyyy}")%>
                            </div>
                        </td>
                        <td>            
                            <div class="person_commentbox">
                                <asp:LinkButton ID="lbn_Conversation" runat="server" CommandArgument='<%#Eval("ConversationID")%>' OnCommand="lbn_Conversation_Command" ToolTip='<%#Eval("CommentText")%>' CssClass="person_comment"><%#Eval("CommentText")%></asp:LinkButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <img src="/Images/Interface/messagehr.gif" alt="">         
                        </td>
                    </tr>  

                </ItemTemplate>

            </asp:ListView>
            <!--END CONVERSATION LISTVIEW-->
        </div>

        <div id="divPrivateConv" runat="server">

            <div style="font-size:20px; padding-bottom:25px;">
                My Private Conversations
            </div>

<!--BEGIN PRIVATE CONVERSATION LISTVIEW-->
            <asp:ListView ID="Private_ListView" runat="server" DataSourceID="SqlDataSource_Private" OnItemDataBound="Private_ListView_OnItemDataBound">

                <LayoutTemplate>
    
        
                    <asp:Button ID="btn_sortNew" runat="server" Text="Sort By Date" CommandName="Sort" CommandArgument="ConversationDate" ToolTip="Sort by Date" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;    
                    Page
                        <asp:DataPager ID="DataPager_Conversations" runat="server" PageSize="5">
                            <Fields>
                            <asp:NumericPagerField ButtonType="Button" />
                            </Fields>
                        </asp:DataPager>
                        <br />
                        <img src="/Images/Interface/messagehr.gif" alt="">
                        <table>
                    <div ID="itemPlaceholderContainer" runat="server" style="">
                        <span runat="server" id="itemPlaceholder" />
                    </div>
                        </table>
                </LayoutTemplate>


                <ItemTemplate>

                    <tr>
                        <td>
                            <div class="person_date">
                                <%#Eval("ConversationDate", "{0:MM/dd/yyyy}")%>
                            </div>
                        </td>
                        <td>            
                            <div class="person_commentbox">
                            <asp:LinkButton ID="lbn_PrivateConversation" runat="server" CommandArgument='<%#Eval("ConversationID")%>' OnCommand="lbn_Conversation_Command" ToolTip='<%#Eval("CommentText")%>' CssClass="person_comment"><%#Eval("CommentText")%></asp:LinkButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <img src="/Images/Interface/messagehr.gif" alt="">         
                        </td>
                    </tr>  

                </ItemTemplate>

            </asp:ListView>
            <!--END PRIVATE LISTVIEW-->

            </div>

        </div>

        </ContentTemplate>
    </asp:UpdatePanel>
<!--END UPDATE PANEL -->

<div id="person_rightarea">
<h3>Ask Leader</h3>
<br />
<asp:Repeater ID="Repeater_Leaders" runat="server" OnItemDataBound="Repeater_Leaders_OnItemDataBound">
    <ItemTemplate>
        <div class="leaderBox">
            <asp:ImageButton ID="imb_pickLeader" runat="server" OnCommand="imb_pickLeader_Command" ImageUrl="~/Images/Interact/leaderPic.jpg" CommandName='<%# DataBinder.Eval(Container.DataItem, "FullUserName")%>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "UserID")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "FullUserName")%>'  />
            <br />
            <asp:LinkButton ID="lbn_pickLeader" runat="server" OnCommand="imb_pickLeader_Command" CommandName='<%# DataBinder.Eval(Container.DataItem, "FullUserName")%>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "UserID")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "FullUserName")%>'><%# DataBinder.Eval(Container.DataItem, "FullUserName")%></asp:LinkButton>
        </div>
    </ItemTemplate>
    
    <SeparatorTemplate>
    <br />
    </SeparatorTemplate>
</asp:Repeater>

</div>


<div id="person_bottomarea">
    &nbsp;
</div>


<!--CLEAR DIV -->
<div style="clear:both;"></div>

<!--BEGIN SQL DATA SOURCE -->
    <asp:SqlDataSource ID="SqlDataSource_Conversations" runat="server" 
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
        SelectCommand="SELECT * FROM tbl_Comments as comm INNER JOIN tbl_Conversations as conv ON conv.ConversationID=comm.ConversationID WHERE CommentDate in( SELECT MIN(CommentDate) 
        FROM tbl_Comments GROUP BY ConversationID ) AND conv.OrgID = @OrgID AND conv.ConversationApproved = '1' AND conv.ConversationPrivate = 'False'
        ORDER BY ConversationDate DESC">

   <SelectParameters>
    <asp:Parameter Name="OrgID" DefaultValue="0" />
  </SelectParameters>

    </asp:SqlDataSource>
<!--END SQL DATA SOURCE -->

<!--BEGIN PRIVATE SQL DATA SOURCE -->
    <asp:SqlDataSource ID="SqlDataSource_Private" runat="server" 
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
        SelectCommand="SELECT * FROM tbl_Comments as comm INNER JOIN tbl_Conversations as conv ON conv.ConversationID=comm.ConversationID WHERE CommentDate in( SELECT MIN(CommentDate) 
        FROM tbl_Comments GROUP BY ConversationID ) AND conv.OrgID = @OrgID AND conv.UserID = @UserID AND conv.ConversationPrivate = 'True'
        ORDER BY ConversationDate DESC">

   <SelectParameters>
    <asp:Parameter Name="OrgID" DefaultValue="0" />
    <asp:Parameter Name="UserID" DefaultValue="0" />
  </SelectParameters>

    </asp:SqlDataSource>
<!--END PRIVATE SQL DATA SOURCE -->

</asp:Content>
