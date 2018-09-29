using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeedbackSafe.Agent
{
    public partial class ManageComments : Page
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
                //GetOrgId();
                lbl_OrgID.Text = Session["OrgID"].ToString();
                GetOrgDetails();
                BindConvList();
            }

            // If Session Variable is Lost
            if (Session["OrgID"] == null)
            {
                Session["OrgID"] = lbl_OrgID.Text;
            }
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
                string sOrgName = OrgIdRdr["OrgName"].ToString();
                lbl_OrgName.Text = sOrgName;
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
                var imb_setApproved = (ImageButton) e.Item.FindControl("imb_setApproved");

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
                // SqlDataSource_Conversations.UpdateParameters["ConversationID"].DefaultValue = e.CommandArgument.ToString();

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
            Response.Redirect("/Agent/Interact.aspx");
        }

        //// END LINKBUTTONS ////


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