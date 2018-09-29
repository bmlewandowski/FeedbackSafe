using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeedbackSafe.Leader
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If not logged in redirect to public
            if (User.Identity.IsAuthenticated == false)
            {
                Server.Transfer("~/Default.aspx");
            }


            // First Load Events
            if (!IsPostBack)
            {
                GetOrgId();
                GetOrgDetails();
                BindConvList();

                // Show Divs
                divPublicConv.Visible = true;
                divPrivateConv.Visible = false;
                lbn_publicConversations.ForeColor = Color.OrangeRed;
                lbn_privateConversations.ForeColor = Color.CadetBlue;
            }

            // If Session Variable is Lost
            if (Session["OrgID"] == null)
            {
                GetOrgId();
                GetOrgDetails();
                BindConvList();
            }
        }

        //// GET ORGID WITH LEADERS ASPNET EMAIL ////
        protected void GetOrgId()
        {
            // Instantiate SQL String
            string SelectOrgIDSQL;

            // Select SQL            
            SelectOrgIDSQL = "SELECT UserID, OrgID FROM tbl_UsersMaster WHERE UserEmail ='" + FetchUser.UserEmail() +
                             "'";

            // Use SQL Statement to Select Records from DB    
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand(SelectOrgIDSQL, sqlConn);

            cmd.Connection.Open();
            SqlDataReader OrgIdRdr;
            OrgIdRdr = cmd.ExecuteReader();
            while (OrgIdRdr.Read())
            {
                string sOrgID = OrgIdRdr["OrgID"].ToString();
                lbl_OrgID.Text = sOrgID;

                // Set Session
                Session["OrgID"] = sOrgID;

                string sUserID = OrgIdRdr["UserID"].ToString();
                // Set Session
                Session["UserID"] = sUserID;
            }
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        //// GET ORG DETAILS FROM ORGID SESSION VARIABLE //// 
        protected void GetOrgDetails()
        {
            // Instantiate SQL String
            string SelectOrgDetailsSQL;
            string sOrgID = lbl_OrgID.Text;

            // Select SQL            
            SelectOrgDetailsSQL = "SELECT * FROM tbl_OrgsMaster WHERE OrgID ='" + sOrgID + "'";

            // Use SQL Statement to Select Records from DB    
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand(SelectOrgDetailsSQL, sqlConn);

            cmd.Connection.Open();
            SqlDataReader OrgIdRdr;
            OrgIdRdr = cmd.ExecuteReader();
            while (OrgIdRdr.Read())
            {
                // Set Page Org Name
                string sOrgName = OrgIdRdr["OrgName"].ToString();
                lbl_OrgName.Text = sOrgName;

                // Get Enabled Status
                string sorgEnabled = OrgIdRdr["OrgEnabled"].ToString().ToLower();

                // Set Master Org Label
                var lbl_masterOrg = (Label) Master.FindControl("lbl_masterOrg");
                lbl_masterOrg.Text = sOrgName;

                // Set Org Enabled Label
                var lbl_orgEnabled = (Label)Master.FindControl("lbl_orgEnabled");
                lbl_orgEnabled.Text = sorgEnabled;

                // Set Session
                Session["OrgName"] = sOrgName;
            }
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }


        //// BEGIN LISTVIEW DATABIND ////

        protected void BindConvList()
        {
            string sOrgID = lbl_OrgID.Text;
            SqlDataSource_Conversations.SelectParameters["OrgID"].DefaultValue = sOrgID;
            Conversation_ListView.DataBind();

            // Bind Private Datalist
            BindPrivateList();

            // Populate User Stats
            GetStats();
        }

        protected void BindPrivateList()
        {
            string sOrgID = lbl_OrgID.Text;
            string sUserID = Session["UserID"].ToString();

            SqlDataSource_Private.SelectParameters["OrgID"].DefaultValue = sOrgID;
            SqlDataSource_Private.SelectParameters["UserID"].DefaultValue = sUserID;
            Private_ListView.DataBind();
            // Populate User Stats
            GetStats();
        }

        //// END DATALIST DATABIND ////


        //// BEGIN DATALIST EVENTS ////

        protected void Conversation_ListView_OnItemDataBound(Object sender, ListViewItemEventArgs e)
        {
            var rec = (DataRowView) e.Item.DataItem;

            // Make sure that you have the data
            if (rec != null)
            {
                var lbn_Conversation = (LinkButton) e.Item.FindControl("lbn_Conversation");
                var lbl_unreadComents = (Label) e.Item.FindControl("lbl_unreadComents");
                var chk_isApproved = (CheckBox) e.Item.FindControl("chk_isApproved");
                var chk_isFlagged = (CheckBox) e.Item.FindControl("chk_isFlagged");
                var imb_setApproved = (ImageButton) e.Item.FindControl("imb_setApproved");
                var imb_setFlagged = (ImageButton) e.Item.FindControl("imb_setFlagged");

                string sCommentText = rec["CommentText"].ToString();
                int stringLength = sCommentText.Length;

                // Hide If First Comment is Empty
                if (stringLength < 1)
                {
                    e.Item.Visible = false;
                }

                // Trim Descriptions to 310 Characters and add "..."
                if (stringLength > 310)
                {
                    lbn_Conversation.Text = sCommentText.Substring(0, Math.Min(310, sCommentText.Length)) + "...";
                }

                else
                {
                    // List Description is Under 310 Char
                    lbn_Conversation.Text = sCommentText;
                }

                // Set Hidden Checkbox for Approve Image Button
                if (chk_isApproved != null)
                {
                    if (rec["ConversationApproved"].ToString() == "True")
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


                // Set Hidden Checkbox for Flag Image Button
                if (chk_isFlagged != null)
                {
                    if (rec["ConversationFlagged"].ToString() == "True")
                    {
                        chk_isFlagged.Checked = true;
                        imb_setFlagged.ImageUrl = "~/Images/Interface/flagged.png";
                        imb_setFlagged.ToolTip = "Remove Flag";
                    }
                    else
                    {
                        chk_isFlagged.Checked = false;
                        imb_setFlagged.ImageUrl = "~/Images/Interface/not_flagged.png";
                        imb_setFlagged.ToolTip = "Flag";
                    }
                }

                // Set Unread Label
                if (rec["ConversationUnread"].ToString() == "True")
                {
                    lbl_unreadComents.Text = "unread";
                }
            }
        }


        protected void Conversation_ListView_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve")
            {
                var chk_isApproved = (CheckBox) e.Item.FindControl("chk_isApproved");
                var imb_setApproved = (ImageButton) e.Item.FindControl("imb_setApproved");
                string sArg = e.CommandArgument.ToString();
                // Instantiate SQL String
                string ApproveConversationSQL;

                if (chk_isApproved.Checked)
                {
                    // Update SQL            
                    ApproveConversationSQL =
                        "UPDATE tbl_Conversations SET ConversationApproved = '0' WHERE ConversationID = '" + sArg + "'";
                }
                else
                {
                    // Select SQL            
                    ApproveConversationSQL =
                        "UPDATE tbl_Conversations SET ConversationApproved = '1' WHERE ConversationID = '" + sArg + "'";
                }

                // Use SQL Statement to Update Records in DB    
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd = new SqlCommand(ApproveConversationSQL, sqlConn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Rebind Datalist
                BindConvList();
            }

            if (e.CommandName == "Flag")
            {
                var chk_isFlagged = (CheckBox) e.Item.FindControl("chk_isFlagged");
                var imb_setFlag = (ImageButton) e.Item.FindControl("imb_setFlag");
                string sArg = e.CommandArgument.ToString();
                // Instantiate SQL String
                string FlagConversationSQL;

                if (chk_isFlagged.Checked)
                {
                    // Update SQL            
                    FlagConversationSQL =
                        "UPDATE tbl_Conversations SET ConversationFlagged = '0' WHERE ConversationID = '" + sArg + "'";
                }
                else
                {
                    // Select SQL            
                    FlagConversationSQL =
                        "UPDATE tbl_Conversations SET ConversationFlagged = '1' WHERE ConversationID = '" + sArg + "'";
                }

                // Use SQL Statement to Update Records in DB    
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd = new SqlCommand(FlagConversationSQL, sqlConn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Rebind Datalist
                BindConvList();
            }
        }

        protected void Private_ListView_OnItemDataBound(Object sender, ListViewItemEventArgs e)
        {
            var rec = (DataRowView) e.Item.DataItem;

            // Make sure that you have the data
            if (rec != null)
            {
                var lbn_Conversation = (LinkButton) e.Item.FindControl("lbn_Conversation");
                var lbl_unreadComents = (Label) e.Item.FindControl("lbl_unreadComents");
                var chk_isApproved = (CheckBox) e.Item.FindControl("chk_isApproved");
                var chk_isFlagged = (CheckBox) e.Item.FindControl("chk_isFlagged");
                var imb_setApproved = (ImageButton) e.Item.FindControl("imb_setApproved");
                var imb_setFlagged = (ImageButton) e.Item.FindControl("imb_setFlagged");

                string sCommentText = rec["CommentText"].ToString();
                int stringLength = sCommentText.Length;

                // Hide If First Comment is Empty
                if (stringLength < 1)
                {
                    e.Item.Visible = false;
                }

                // Trim Descriptions to 310 Characters and add "..."
                if (stringLength > 310)
                {
                    lbn_Conversation.Text = sCommentText.Substring(0, Math.Min(310, sCommentText.Length)) + "...";
                }

                else
                {
                    // List Description is Under 310 Char
                    lbn_Conversation.Text = sCommentText;
                }

                // Set Hidden Checkbox for Approve Image Button
                if (chk_isApproved != null)
                {
                    if (rec["ConversationApproved"].ToString() == "True")
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


                // Set Hidden Checkbox for Flag Image Button
                if (chk_isFlagged != null)
                {
                    if (rec["ConversationFlagged"].ToString() == "True")
                    {
                        chk_isFlagged.Checked = true;
                        imb_setFlagged.ImageUrl = "~/Images/Interface/flagged.png";
                        imb_setFlagged.ToolTip = "Remove Flag";
                    }
                    else
                    {
                        chk_isFlagged.Checked = false;
                        imb_setFlagged.ImageUrl = "~/Images/Interface/not_flagged.png";
                        imb_setFlagged.ToolTip = "Flag";
                    }
                }

                // Set Unread Label
                if (rec["ConversationUnread"].ToString() == "True")
                {
                    lbl_unreadComents.Text = "unread";
                }
            }
        }

        /////////// PRIVATE LISTVIEW ///////////////

        protected void Private_ListView_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve")
            {
                var chk_isApproved = (CheckBox) e.Item.FindControl("chk_isApproved");
                var imb_setApproved = (ImageButton) e.Item.FindControl("imb_setApproved");
                string sArg = e.CommandArgument.ToString();
                // Instantiate SQL String
                string ApproveConversationSQL;

                if (chk_isApproved.Checked)
                {
                    // Update SQL            
                    ApproveConversationSQL =
                        "UPDATE tbl_Conversations SET ConversationApproved = '0' WHERE ConversationID = '" + sArg + "'";
                }
                else
                {
                    // Select SQL            
                    ApproveConversationSQL =
                        "UPDATE tbl_Conversations SET ConversationApproved = '1' WHERE ConversationID = '" + sArg + "'";
                }

                // Use SQL Statement to Update Records in DB    
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd = new SqlCommand(ApproveConversationSQL, sqlConn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Rebind Datalist
                BindConvList();
            }

            if (e.CommandName == "Flag")
            {
                var chk_isFlagged = (CheckBox) e.Item.FindControl("chk_isFlagged");
                var imb_setFlag = (ImageButton) e.Item.FindControl("imb_setFlag");
                string sArg = e.CommandArgument.ToString();
                // Instantiate SQL String
                string FlagConversationSQL;

                if (chk_isFlagged.Checked)
                {
                    // Update SQL            
                    FlagConversationSQL =
                        "UPDATE tbl_Conversations SET ConversationFlagged = '0' WHERE ConversationID = '" + sArg + "'";
                }
                else
                {
                    // Select SQL            
                    FlagConversationSQL =
                        "UPDATE tbl_Conversations SET ConversationFlagged = '1' WHERE ConversationID = '" + sArg + "'";
                }

                // Use SQL Statement to Update Records in DB    
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd = new SqlCommand(FlagConversationSQL, sqlConn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Rebind Datalist
                BindConvList();
            }
        }

        //// END DATALIST EVENTS ////


        //// BEGIN LINKBUTTONS ////

        protected void lbn_Conversation_Command(object sender, CommandEventArgs e)
        {
            // Save ConversationID and LeaderID to Session
            Session["ConversationID"] = e.CommandArgument.ToString();
            Session["AskedLeaderID"] = e.CommandName;

            // Set Label Just to Verify
            lbl_ConvID.Text = e.CommandArgument.ToString();

            // Send to Conversation Thread
            Response.Redirect("/Leader/Interact.aspx");
        }

        //// END LINKBUTTONS ////


        //// BEGIN BUTTONS ////

        protected void btn_askQuestion_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Insert new Conversation to DB and Return New ID
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd =
                    new SqlCommand(
                        "INSERT INTO tbl_Conversations (OrgID, UserID, LeaderID, ConversationApproved, ConversationUnread)VALUES(@OrgID, @UserID, @LeaderID, '1', '1');SELECT @@IDENTITY",
                        sqlConn);
                cmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 255).Value = Session["OrgID"].ToString();
                cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 255).Value = Session["UserID"].ToString();
                cmd.Parameters.Add("@LeaderID", SqlDbType.VarChar, 255).Value = Session["UserID"].ToString();
                cmd.Connection.Open();
                Int32 NewConvID = Convert.ToInt32(cmd.ExecuteScalar());
                lbl_ConvID.Text = NewConvID.ToString();
                cmd.Connection.Close();
                cmd.Connection.Dispose();


                // Insert Comment to DB using New ConversationID
                var sqlConn2 =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd2 =
                    new SqlCommand(
                        "INSERT INTO tbl_Comments (ConversationID, UserID, CommentText, CommentApproved)VALUES(@ConversationID, @UserID, @CommentText, '1')",
                        sqlConn2);
                cmd2.Parameters.Add("@ConversationID", SqlDbType.VarChar, 255).Value = lbl_ConvID.Text;
                cmd2.Parameters.Add("@UserID", SqlDbType.VarChar, 255).Value = Session["UserID"].ToString();
                cmd2.Parameters.Add("@CommentText", SqlDbType.NVarChar, -1).Value = txt_askedQuestion.Text;
                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
                cmd2.Connection.Dispose();

                // TO DO Send Email to Users
                if (chk_emailAll.Checked)
                {
                    // Instantiate SQL String
                    string SelectUsersSQL;

                    // Get Token from Session 
                    string sOrgID = Session["OrgID"].ToString();

                    // Select SQL            
                    SelectUsersSQL =
                        "SELECT UserEmail, UserTitle, UserFirstName, UserMiddleName, UserLastName, UserToken FROM tbl_UsersMaster WHERE OrgID ='" +
                        sOrgID + "' AND isLeader = '0' AND optOut = '0'";


                    // Use SQL Statement to Select Records from DB    
                    var sqlConn3 =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                    var cmd3 = new SqlCommand(SelectUsersSQL, sqlConn3);

                    cmd3.Connection.Open();
                    SqlDataReader rdr;
                    rdr = cmd3.ExecuteReader();
                    while (rdr.Read())
                    {
                        // Get OrgName
                        string sOrgName = lbl_OrgName.Text;

                        var mail = new MailMessage();
                        mail.To.Add(rdr[0].ToString());
                        mail.From = new MailAddress("info@feedbacksafe.com");
                        mail.Subject = "Feedback Safe: A New Conversation has been started with " + sOrgName;
                        mail.IsBodyHtml = true;
                        mail.BodyEncoding = Encoding.UTF8;
                        string body = FetchTemplate.ReadFile("/Templates/leaderAddConversation.htm");
                        body = body.Replace("{UserName}", rdr[1] + " " + rdr[2] + " " + rdr[3] + " " + rdr[4]);
                        body = body.Replace("{Organization}", sOrgName);
                        body = body.Replace("{Token1}", rdr[5].ToString());
                        body = body.Replace("{Token2}", rdr[5].ToString());
                        body = body.Replace("{LeaderComment}", txt_askedQuestion.Text);
                        mail.Body = body;
                        var smtp = new SmtpClient();
                        smtp.Send(mail);
                    }

                    cmd3.Connection.Close();
                    cmd3.Connection.Dispose();
                }

                // Clear Textbox
                txt_askedQuestion.Text = "";
                // Uncheck Mail Checkbox
                chk_emailAll.Checked = false;
                // Rebind Conversation List
                BindConvList();
            }
        }

        protected void lbn_publicConversations_Click(object sender, EventArgs e)
        {
            // Show Divs
            divPublicConv.Visible = true;
            divPrivateConv.Visible = false;
            lbn_publicConversations.ForeColor = Color.OrangeRed;
            lbn_privateConversations.ForeColor = Color.CadetBlue;
        }

        protected void lbn_privateConversations_Click(object sender, EventArgs e)
        {
            // Show Divs
            divPublicConv.Visible = false;
            divPrivateConv.Visible = true;
            lbn_publicConversations.ForeColor = Color.CadetBlue;
            lbn_privateConversations.ForeColor = Color.OrangeRed;
        }


        //// END BUTTONS ////

        //// GET STATISTICS ////

        public void GetStats()
        {
            string SelectSQLStats = "SELECT " +
                                    "Count_Approved=(SELECT Count(ConversationID) FROM tbl_Conversations WHERE OrgID = @OrgID AND ConversationApproved = '1')," +
                                    "Count_Unread=(SELECT Count(ConversationID) FROM tbl_Conversations WHERE OrgID = @OrgID AND ConversationUnread = '1')," +
                                    "Count_Leaders=(SELECT Count(UserID) FROM tbl_UsersMaster WHERE OrgID = @OrgID AND isLeader = '1')," +
                                    "Count_Users=(SELECT Count(UserID) FROM tbl_UsersMaster WHERE OrgID = @OrgID AND isLeader = '0')," +
                                    "Count_optOut=(SELECT Count(optOut) FROM tbl_UsersMaster WHERE OrgID = @OrgID AND optOut = '1')," +
                                    "Count_optOutConversation =(SELECT Count(optOutConversation) FROM tbl_UsersMaster WHERE OrgID = @OrgID AND optOutConversation = '1')," +
                                    "Count_optOutComment=(SELECT Count(optOutComment) FROM tbl_UsersMaster WHERE OrgID = @OrgID AND optOutComment = '1')";

            // Select Lists from DB
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand(SelectSQLStats, sqlConn);
            cmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 255).Value = Session["OrgID"].ToString();
            cmd.Connection.Open();

            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lbl_stat_approved.Text = rdr["Count_Approved"].ToString();
                lbl_stat_unread.Text = rdr["Count_Unread"].ToString();
                lbl_stat_leaders.Text = rdr["Count_Leaders"].ToString();
                lbl_stat_users.Text = rdr["Count_Users"].ToString();
                lbl_optout.Text = rdr["Count_optOut"].ToString();
                lbl_stat_optoutconversations.Text = rdr["Count_optOutConversation"].ToString();
                lbl_stat_optoutcomments.Text = rdr["Count_optOutComment"].ToString();
            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
    }
}