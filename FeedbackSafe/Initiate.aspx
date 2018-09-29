<%@ Page Title="" Language="C#" MasterPageFile="~/Private.Master" AutoEventWireup="true" CodeBehind="Initiate.aspx.cs" Inherits="FeedbackSafe.Initiate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<!--BEGIN HIDDEN LABELS-->
    <asp:Label ID="lbl_ConvID" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_leaderAsked" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_leaderAspnetId" runat="server" Visible="false"></asp:Label>


<!--BEGIN UPDATE PANEL-->
    <asp:UpdatePanel ID="udp_Conversations" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

<!-- BEGIN CONVERSATION UPDATE PROGRESS -->

    <asp:UpdateProgress ID="UpdateProgress_Conversations" runat="server">
        <ProgressTemplate>
            <span style="color:Green;">UPDATE IN PROGRESS...</span>
        </ProgressTemplate>
    </asp:UpdateProgress>


<!--BEGIN LEADER INFO-->
    <asp:Image ID="img_askedLeader" runat="server" ImageUrl="~/Images/Interact/leaderPic.jpg" style="vertical-align:middle; height:100px;" alt=""  />
    <strong>Conversation with <asp:Label ID="lbl_leaderAskedName" runat="server"></asp:Label></strong>
    <br />
    <br />
    <asp:TextBox ID="txt_askedQuestion" runat="server" Height="200px" Width="450px" TextMode="MultiLine"></asp:TextBox>
    &nbsp;
    <asp:ImageButton ID="btn_askQuestion" runat="server" Text="Start Conversation" onclick="btn_askQuestion_Click" ImageUrl="~/Images/Interface/add.png" ValidationGroup="AskQuestion" />
    <asp:RequiredFieldValidator ID="val_askedQuestion" runat="server" ControlToValidate="txt_askedQuestion" ErrorMessage="* Required" ValidationGroup="AskQuestion"></asp:RequiredFieldValidator>
    <br /><br />
     <asp:Image ID="img_markPrivate" runat="server" ImageUrl="~/Images/Interface/private_small_faded.png" style="vertical-align:middle;" alt="Mark Private" />
    <asp:CheckBox ID="chk_isPrivate" runat="server" AutoPostBack="true" 
                oncheckedchanged="chk_isPrivate_CheckedChanged" /> Mark Private
    <br /><br />

        </ContentTemplate>
    </asp:UpdatePanel>
<!--END UPDATE PANEL -->

</asp:Content>
