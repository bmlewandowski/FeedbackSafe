using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeedbackSafe
{
    public partial class Person : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // First Load Events
            if (!IsPostBack)
            {
                // IF LOGGED IN GRAB DETAILS AND POPULATE WITH ASP.NET ID
                if (User.Identity.IsAuthenticated)
                {
                    lbl_UserID.Text = FetchUser.UserID();
                    GetOrgUserFromMembership();
                    GetOrgDetails();
                    GetLeaders();
                    BindConvList();
                }
                
                    // ELSE IF QUERYSTRING EXISTS USE IT TO GRAB DETAILS AND POPULATE
                else if (!String.IsNullOrEmpty(Request.QueryString["Token"]))
                {
                    string sToken = Request.QueryString["Token"];
                    Session["Token"] = sToken;
                    lbl_Token.Text = Session["Token"].ToString();

                    GetOrgUserFromToken();
                    GetOrgDetails();
                    GetLeaders();
                    BindConvList();
                }

                    // ELSE IF SESSION VARIABLE IS POPULATED
                else if (Session["UserID"] != null)
                {
                    GetOrgUserFromSession();
                    GetOrgDetails();
                    GetLeaders();
                    BindConvList();
                }

                else
                {
                    // ELSE IF NOT LOGGED IN NOR USING TOKEN NOR SESSION SEND TO DEFAULT PAGE
                    Response.Redirect("/Default.aspx");
                }

                // Show Divs
                divPublicConv.Visible = true;
                divPrivateConv.Visible = false;
                lbn_publicConversations.ForeColor = Color.OrangeRed;
                lbn_privateConversations.ForeColor = Color.CadetBlue;
            }

            // If Session Variable is Lost
            if (Session["UserID"] == null)
            {
                // IF LOGGED IN GRAB DETAILS AND POPULATE WITH ASP.NET ID
                if (User.Identity.IsAuthenticated)
                {
                    lbl_UserID.Text = FetchUser.UserID();
                    GetOrgUserFromMembership();
                    GetOrgDetails();
                    GetLeaders();
                    BindConvList();
                }

                    // ELSE IF QUERYSTRING EXISTS USE IT TO GRAB DETAILS AND POPULATE
                else if (!String.IsNullOrEmpty(Request.QueryString["Token"]))
                {
                    string sToken = Request.QueryString["Token"];
                    Session["Token"] = sToken;
                    lbl_Token.Text = Session["Token"].ToString();

                    GetOrgUserFromToken();
                    GetOrgDetails();
                    GetLeaders();
                    BindConvList();
                }
                
                    // ELSE IF SESSION VARIABLE IS POPULATED
                else if (Session["UserID"] != null)
                {
                    GetOrgUserFromSession();
                    GetOrgDetails();
                    GetLeaders();
                    BindConvList();
                }

                else
                {
                    // ELSE IF NOT LOGGED IN NOR USING TOKEN NOR SESSION SEND TO DEFAULT PAGE
                    Response.Redirect("/Default.aspx");
                }
            }
        }


        //// GET USERID AND ORGID FROM USER TOKEN ////
        protected void GetOrgUserFromToken()
        {
            // Instantiate SQL String
            string SelectOrgIDSQL;

            // Get Token from Session 
            string sToken = Session["Token"].ToString();

            // Select SQL            
            SelectOrgIDSQL = "SELECT UserID, OrgID FROM tbl_UsersMaster WHERE UserToken ='" + sToken + "'";

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
                lbl_UserID.Text = sUserID;
                // Set Session
                Session["UserID"] = sUserID;
            }
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        //// GET USERID AND ORGID FROM SESSION VARIABLE ////
        protected void GetOrgUserFromSession()
        {
            // Instantiate SQL String
            string SelectOrgIDSQL;

            // Get Token from Session 
            string sUserID = Session["UserID"].ToString();

            // Select SQL            
            SelectOrgIDSQL = "SELECT UserID, OrgID FROM tbl_UsersMaster WHERE UserID ='" + sUserID + "'";

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

                //string sUserID = OrgIdRdr["UserID"].ToString();
                lbl_UserID.Text = sUserID;
                // Set Session
                Session["UserID"] = sUserID;
            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        //// GET ORGID AND USERID FROM ASPNET MEMBERSHIP STATUS ////
        protected void GetOrgUserFromMembership()
        {
            // Instantiate SQL String
            string SelectOrgIDSQL;

            // Select SQL            
            SelectOrgIDSQL =
                "SELECT tbl_UsersMaster.UserID, tbl_UsersMaster.OrgID FROM tbl_UsersMaster, aspnet_Users WHERE tbl_UsersMaster.aspnetId ='" +
                FetchUser.UserID() + "'";

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
                lbl_UserID.Text = sUserID;
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
            // Get From Session
            string sOrgID = Session["OrgID"].ToString();

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
                // Create strings from reader
                string sOrgName = OrgIdRdr["OrgName"].ToString();
                string sautoApproveConversation = OrgIdRdr["autoApproveConversation"].ToString();
                string sautoApproveComment = OrgIdRdr["autoApproveComment"].ToString();
                string sconversationEmail = OrgIdRdr["conversationEmail"].ToString();
                string scommentEmail = OrgIdRdr["commentEmail"].ToString();
                string sprofanityFilter = OrgIdRdr["profanityFilter"].ToString();
                string sorgEnabled = OrgIdRdr["OrgEnabled"].ToString().ToLower();

                lbl_OrgName.Text = sOrgName;

                // Set Sessions with created strings
                Session["OrgName"] = sOrgName;
                Session["autoApproveConversation"] = sautoApproveConversation;
                Session["autoApproveComment"] = sautoApproveComment;
                Session["conversationEmail"] = sconversationEmail;
                Session["commentEmail"] = scommentEmail;
                Session["profanityFilter"] = sprofanityFilter;

                // Set Master Org Label
                var lbl_masterOrg = (Label) Master.FindControl("lbl_masterOrg");
                lbl_masterOrg.Text = sOrgName;

                // Set Org Enabled Label
                var lbl_orgEnabled = (Label)Master.FindControl("lbl_orgEnabled");
                lbl_orgEnabled.Text = sorgEnabled;
            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        //// GET LEADERS FROM ORGID SESSION VARIABLE //// 
        protected void GetLeaders()
        {
            // Get From Session
            string sOrgID = Session["OrgID"].ToString();
            // Instantiate SQL String
            string LeaderSelectSQL =
                "SELECT ISNULL( UserTitle , '') + ' ' + ISNULL( UserFirstName , '') + ' ' + ISNULL( UserMiddleName , '') + ' ' + ISNULL( UserLastName , '') AS [FullUserName], UserID, aspnetId FROM tbl_UsersMaster WHERE OrgID ='" +
                sOrgID + "' AND isLeader = '1' ORDER BY UserID ASC";

            // Use SQL Statement to Select Records from DB    
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand(LeaderSelectSQL, sqlConn);
            cmd.Connection.Open();
            Repeater_Leaders.DataSource = cmd.ExecuteReader();
            Repeater_Leaders.DataBind();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }


        //// BEGIN LISTVIEW DATABIND ////

        protected void BindConvList()
        {
            string sOrgID = lbl_OrgID.Text;
            SqlDataSource_Conversations.SelectParameters["OrgID"].DefaultValue = sOrgID;
            Conversation_ListView.DataBind();

            // BIND PRIVATE LIST 
            BindPrivateList();
        }


        protected void BindPrivateList()
        {
            string sOrgID = lbl_OrgID.Text;
            string sUserID = Session["UserID"].ToString();
            SqlDataSource_Private.SelectParameters["OrgID"].DefaultValue = sOrgID;
            SqlDataSource_Private.SelectParameters["UserID"].DefaultValue = sUserID;
            Conversation_ListView.DataBind();
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
                string sCommentText = rec["CommentText"].ToString();
                string sFilteredText;

                // Check if Org has enabled Profanity Filter
                string sprofanityFilter = Session["profanityFilter"].ToString();

                if (sprofanityFilter == "True")
                {
                    // Run text through filter before sending to label
                    sFilteredText = FilterWords.ChangeBadWords(sCommentText);
                }

                else
                {
                    // Bind as is
                    sFilteredText = sCommentText;
                }

                // Get Length of Filtered String
                int stringLength = sFilteredText.Length;

                // Hide If First Comment is Empty
                if (stringLength < 1)
                {
                    e.Item.Visible = false;
                }

                // Trim Descriptions to 275 Characters and add "..."
                if (stringLength > 275)
                {
                    lbn_Conversation.Text = sFilteredText.Substring(0, Math.Min(275, sFilteredText.Length)) + "...";
                }

                else
                {
                    // List Description is Under 275 Char
                    lbn_Conversation.Text = sFilteredText;
                }
            }
        }

        protected void Private_ListView_OnItemDataBound(Object sender, ListViewItemEventArgs e)
        {
        }

        //// END DATALIST EVENTS ////

        //// BEGIN LINKBUTTONS ////

        protected void lbn_Conversation_Command(object sender, CommandEventArgs e)
        {
            // Save ConversationID to Session
            Session["ConversationID"] = e.CommandArgument.ToString();

            // Set Label Just to Verify
            lbl_ConvID.Text = e.CommandArgument.ToString();

            // Send to Conversation Thread
            Response.Redirect("/Conversation.aspx");
        }

        protected void imb_pickLeader_Command(object sender, CommandEventArgs e)
        {
            // Set Session and Redirect
            Session["AskLeaderName"] = e.CommandName;
            Session["AskLeaderID"] = e.CommandArgument.ToString();
            Response.Redirect("Initiate.aspx");
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

        //// END LINKBUTTONS ////


        //// BEGIN LEADER REPEATER EVENTS ////

        protected void Repeater_Leaders_OnItemDataBound(Object sender, RepeaterItemEventArgs e)
        {
            var rec = (DbDataRecord) e.Item.DataItem;

            // Make sure that you have the data
            if (rec != null)
            {
                string saspnetId = rec["aspnetId"].ToString();

                // Find Display Image
                var imb_pickLeader = (ImageButton) e.Item.FindControl("imb_pickLeader");

                // Check for image in User Links folder
                string imgDisplayPath = Server.MapPath("~/Images/Users/" + saspnetId + "/user_defined.jpg");

                if (File.Exists(imgDisplayPath))
                {
                    // Set Image Control URL 
                    imb_pickLeader.ImageUrl = "~/Images/Users/" + saspnetId + "/user_defined.jpg";
                }
            }
        }

        //// END LEADER REPEATER EVENTS ////
    }
}