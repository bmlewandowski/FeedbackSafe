using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeedbackSafe.Agent
{
    public partial class Interact : Page
    {
        private readonly Random randNum = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            // First Load Events
            if (!IsPostBack)
            {
                if (Session["ConversationID"] != null)
                {
                    string sConversationID = Session["ConversationID"].ToString();
                    SqlDataSource_Comments.SelectParameters["ConversationID"].DefaultValue = sConversationID;
                    BindConversation();
                    SetUnread();
                }
                else
                {
                    Response.Redirect("/Default.aspx");
                }

                // Get Leader Login Name

                // Instantiate SQL String
                string SelectLeaderLoginSQL;

                string sLeaderID = lbl_LeaderID.Text;

                // Select SQL            
                SelectLeaderLoginSQL = "SELECT aspnetId FROM tbl_UsersMaster WHERE UserID ='" + sLeaderID + "'";

                // Use SQL Statement to Select Records from DB    
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd = new SqlCommand(SelectLeaderLoginSQL, sqlConn);

                cmd.Connection.Open();
                SqlDataReader rdr;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    // Create strings from reader
                    string sleaderAskedAspnet = rdr["aspnetId"].ToString();
                    lbl_leaderAskedAspnet.Text = sleaderAskedAspnet;
                }
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Check for image in User Links folder
                string imgDisplayPath =
                    Server.MapPath("~/Images/Users/" + lbl_leaderAskedAspnet.Text + "/user_defined.jpg");
                if (File.Exists(imgDisplayPath))
                {
                    // Set Image Control URL 
                    img_askedLeader.ImageUrl = "~/Images/Users/" + lbl_leaderAskedAspnet.Text + "/user_defined.jpg";
                }

                // Get Asked Leader Name
                PopulateLabels();
            }
        }

        //// SET CONVERSATION READ ////

        protected void SetUnread()
        {
            // Instantiate SQL String
            string SetReadSQL;
            // Get From Session
            string sConversationID = Session["ConversationID"].ToString();

            // Select SQL            
            SetReadSQL = "UPDATE tbl_Conversations SET ConversationUnread ='FALSE' WHERE ConversationID ='" +
                         sConversationID + "'";

            // Use SQL Statement to Select Records from DB    
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand(SetReadSQL, sqlConn);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        //// BEGIN LISTVIEW DATABIND ////

        protected void BindConversation()
        {
            Comment_ListView.DataBind();
        }

        //// END LISTVIEW DATABIND ////

        //// BEGIN DATALIST EVENTS ////

        // Declare Random Outside of Loop for New Seed

        protected void Comment_ListView_OnItemDataBound(Object sender, ListViewItemEventArgs e)
        {
            // Define Data Record
            var rec = (DataRowView) e.Item.DataItem;
            // Make sure that you have the data
            if (rec != null)
            {
                // Find Controls
                var lbl_leaderTitle = (Label) e.Item.FindControl("lbl_leaderTitle");
                var pnl_bubbleMain = (Panel) e.Item.FindControl("pnl_bubbleMain");
                var pnl_bubbleTop = (Panel) e.Item.FindControl("pnl_bubbleTop");
                var pnl_bubbleBottom = (Panel) e.Item.FindControl("pnl_bubbleBottom");
                var chk_isApproved = (CheckBox) e.Item.FindControl("chk_isApproved");
                var imb_setApproved = (ImageButton) e.Item.FindControl("imb_setApproved");
                var img_personPic = (Image) e.Item.FindControl("img_personPic");
                var img_leaderPic = (Image) e.Item.FindControl("img_leaderPic");
                var img_bubbleTail = (Image) e.Item.FindControl("img_bubbleTail");

                // Get Leader Status
                int sIsLeader = Convert.ToInt32(rec["isLeader"]);
                int sIsPrivate = Convert.ToInt32(rec["ConversationPrivate"]);
                string saspnetID = rec["aspnetId"].ToString();

                // Check for Leader
                if (sIsLeader == 1)
                {
                    // Change Bubble Color CSS
                    if (pnl_bubbleMain != null && pnl_bubbleTop != null && pnl_bubbleBottom != null)
                    {
                        pnl_bubbleMain.CssClass = "interact_lbubblemain";
                        pnl_bubbleTop.CssClass = "interact_lbubbletop";
                        pnl_bubbleBottom.CssClass = "interact_lbubblebottom";
                    }


                    // Show/Hide User Pics
                    if (img_personPic != null)
                    {
                        img_personPic.Visible = false;
                    }

                    if (img_leaderPic != null)
                    {
                        img_leaderPic.Visible = true;
                    }

                    // Adjust Bubble Tail
                    if (img_bubbleTail != null)
                    {
                        img_bubbleTail.CssClass = "interact_lbubbletail";
                        img_bubbleTail.ImageUrl = "~/Images/Interact/lbubbletail.jpg";
                    }


                    // Check for image in User Links folder
                    string imgDisplayPath = Server.MapPath("~/Images/Users/" + saspnetID + "/user_defined.jpg");
                    if (File.Exists(imgDisplayPath))
                    {
                        if (img_leaderPic != null)
                        {
                            // Set Image Control URL 
                            img_leaderPic.ImageUrl = "~/Images/Users/" + saspnetID + "/user_defined.jpg";
                        }
                    }


                    // Build Leader Name String
                    string sLeaderName = rec["UserTitle"] + " " + rec["UserFirstName"] + " " + rec["UserMiddleName"] +
                                         " " + rec["UserLastName"];

                    // Set Leader Name Label
                    if (lbl_leaderTitle != null)
                    {
                        lbl_leaderTitle.Text = sLeaderName;
                    }
                }

                else
                {
                    // Randomize User Image
                    if (img_personPic != null)
                    {
                        int picNumber;
                        picNumber = randNum.Next(1, 5);
                        img_personPic.ImageUrl = "~/Images/Interact/personPic_" + picNumber + ".jpg";
                    }
                }

                if (chk_isApproved != null)
                {
                    if (rec["CommentApproved"].ToString() == "True")
                    {
                        chk_isApproved.Checked = true;
                        imb_setApproved.ImageUrl = "~/Images/Interface/approved.png";
                        imb_setApproved.ToolTip = "Remove Approval";
                    }
                    else
                    {
                        chk_isApproved.Checked = false;
                        imb_setApproved.ImageUrl = "~/Images/Interface/not_approved.png";
                        imb_setApproved.ToolTip = "Approve";
                    }
                }


                // Set Private Badge
                if (img_private != null)
                {
                    if (sIsPrivate == 1)
                    {
                        img_private.Visible = true;
                    }
                }

                // Set Label to Retrieve for Email
                lbl_LeaderID.Text = rec["LeaderID"].ToString();
            }
        }


        protected void Comment_ListView_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                SqlDataSource_Comments.UpdateParameters["CommentID"].DefaultValue = e.CommandArgument.ToString();
            }

            if (e.CommandName == "Approve")
            {
                // SqlDataSource_Conversations.UpdateParameters["ConversationID"].DefaultValue = e.CommandArgument.ToString();

                var chk_isApproved = (CheckBox) e.Item.FindControl("chk_isApproved");
                var imb_setApproved = (ImageButton) e.Item.FindControl("imb_setApproved");
                string sToken = e.CommandArgument.ToString();
                // Instantiate SQL String
                string ApproveCommentSQL;

                if (chk_isApproved.Checked)
                {
                    // Update SQL            
                    ApproveCommentSQL = "UPDATE tbl_Comments SET CommentApproved = '0' WHERE CommentID = '" + sToken +
                                        "'";
                }
                else
                {
                    // Select SQL            
                    ApproveCommentSQL = "UPDATE tbl_Comments SET CommentApproved = '1' WHERE CommentID = '" + sToken +
                                        "'";
                }


                // Use SQL Statement to Update Records in DB    
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd = new SqlCommand(ApproveCommentSQL, sqlConn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Rebind Datalist
                BindConversation();
            }
        }

        //// END DATALIST EVENTS ////

        //// BEGIN BUTTONS ////

        protected void btn_addComment_Click(object sender, ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                // Insert List to DB and Return New ID
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd =
                    new SqlCommand(
                        "INSERT INTO tbl_Comments (ConversationID, UserID, CommentText, CommentApproved)VALUES(@ConversationID, @UserID, @CommentText, '1')",
                        sqlConn);
                cmd.Parameters.Add("@ConversationID", SqlDbType.VarChar, 255).Value =
                    Session["ConversationID"].ToString();
                cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 255).Value = Session["UserID"].ToString();
                cmd.Parameters.Add("@CommentText", SqlDbType.NVarChar, -1).Value = txt_addComment.Text;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Do Checks and Send User Email 
                // NEED CODE

                // Clear Textbox
                txt_addComment.Text = "";

                // Rebind Datalist
                BindConversation();
            }
        }

        //// END BUTTONS ////


        //// POPULATE LABELS ////

        public void PopulateLabels()
        {
            if (Session["AskedLeaderID"] != null)
            {
                string sAskedLeaderID = Session["AskedLeaderID"].ToString();

                // Select Lists from DB
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd =
                    new SqlCommand(
                        "SELECT ISNULL( UserTitle , '') + ' ' + ISNULL( UserFirstName , '') + ' ' + ISNULL( UserMiddleName , '') + ' ' + ISNULL( UserLastName , '') AS [FullUserName] FROM tbl_UsersMaster WHERE UserID ='" +
                        sAskedLeaderID + "'", sqlConn);
                cmd.Connection.Open();

                SqlDataReader rdr;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lbl_AskedLeader.Text = rdr["FullUserName"].ToString();
                }

                cmd.Connection.Close();
                cmd.Connection.Dispose();
            }
        }
    }
}