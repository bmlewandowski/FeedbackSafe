using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeedbackSafe
{
    public partial class Conversation : Page
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
                var lbl_CommentText = (Label) e.Item.FindControl("lbl_CommentText");
                var pnl_bubbleMain = (Panel) e.Item.FindControl("pnl_bubbleMain");
                var pnl_bubbleTop = (Panel) e.Item.FindControl("pnl_bubbleTop");
                var pnl_bubbleBottom = (Panel) e.Item.FindControl("pnl_bubbleBottom");
                var img_personPic = (Image) e.Item.FindControl("img_personPic");
                var img_leaderPic = (Image) e.Item.FindControl("img_leaderPic");
                var img_bubbleTail = (Image) e.Item.FindControl("img_bubbleTail");

                // Get Leader Status
                int sIsLeader = Convert.ToInt32(rec["isLeader"]);
                int sIsPrivate = Convert.ToInt32(rec["ConversationPrivate"]);
                string saspnetID = rec["aspnetId"].ToString();

                // Check if Org has enabled Profanity Filter
                string sprofanityFilter = Session["profanityFilter"].ToString();

                if (sprofanityFilter == "True")
                {
                    // Run text through filter before sending to label
                    lbl_CommentText.Text = FilterWords.ChangeBadWords(rec["CommentText"].ToString());
                }

                else
                {
                    // Bind to label as is
                    lbl_CommentText.Text = rec["CommentText"].ToString();
                }

                // Check for Leader
                if (sIsLeader == 1)
                {
                    //// Change Bubble Color CSS
                    if (pnl_bubbleMain != null && pnl_bubbleTop != null && pnl_bubbleBottom != null)
                    {
                        pnl_bubbleMain.CssClass = "interact_lbubblemain";
                        pnl_bubbleTop.CssClass = "interact_lbubbletop";
                        pnl_bubbleBottom.CssClass = "interact_lbubblebottom";
                    }


                    //// Show/Hide User Pics
                    if (img_personPic != null)
                    {
                        img_personPic.Visible = false;
                    }

                    if (img_leaderPic != null)
                    {
                        img_leaderPic.Visible = true;
                    }

                    //// Adjust Bubble Tail
                    if (img_bubbleTail != null)
                    {
                        img_bubbleTail.CssClass = "interact_lbubbletail";
                        img_bubbleTail.ImageUrl = "~/Images/Interact/lbubbletail.jpg";
                    }


                    // Check for image in User Links folder
                    string imgDisplayPath = Server.MapPath("~/Images/Users/" + saspnetID + "/user_defined.jpg");
                    if (File.Exists(imgDisplayPath))
                    {
                        // Set Image Control URL 
                        img_leaderPic.ImageUrl = "~/Images/Users/" + saspnetID + "/user_defined.jpg";
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

        //// END DATALIST EVENTS ////


        //// SET CONVERSATION READ ////

        protected void SetUnread()
        {
            // Instantiate SQL String
            string SetReadSQL;
            // Get From Session
            string sConversationID = Session["ConversationID"].ToString();

            // Select SQL            
            SetReadSQL = "UPDATE tbl_Conversations SET ConversationUnread ='TRUE' WHERE ConversationID ='" +
                         sConversationID + "'";

            // Use SQL Statement to Select Records from DB    
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand(SetReadSQL, sqlConn);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        //// SET CONVERSATION FLAGGED ////

        protected void SetFlagged()
        {
            // Instantiate SQL String
            string SetFlagSQL;
            // Get From Session
            string sConversationID = Session["ConversationID"].ToString();

            // Select SQL            
            SetFlagSQL = "UPDATE tbl_Conversations SET ConversationFlagged ='TRUE' WHERE ConversationID ='" +
                         sConversationID + "'";

            // Use SQL Statement to Select Records from DB    
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand(SetFlagSQL, sqlConn);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }


        //// ADD COMMENT ////

        protected void btn_addComment_Click(object sender, ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                string sCommentText = txt_addComment.Text;
                string sFlag;

                // Check if Org has enabled Profanity Filter
                string sprofanityFilter = Session["profanityFilter"].ToString();

                if (sprofanityFilter == "True")
                {
                    // Run Comment Through Filter and Return TRUE/FALSE
                    sFlag = FilterWords.HasBadWords(sCommentText).ToString();
                }

                else
                {
                    // Default to False
                    sFlag = "False";
                }


                // Instantiate SQL String
                string InsertCommentSQL;

                // Get Session Variable
                string sautoApproveComment = Session["autoApproveComment"].ToString();

                // Set SQL based on AutoApproveComment Session variable
                if (sautoApproveComment == "True")
                {
                    InsertCommentSQL =
                        "INSERT INTO tbl_Comments (ConversationID, UserID, CommentText, CommentApproved, Flag, IpAddress)VALUES(@ConversationID, @UserID, @CommentText, '1', @Flag, @IpAddress)";
                }
                else
                {
                    InsertCommentSQL =
                        "INSERT INTO tbl_Comments (ConversationID, UserID, CommentText, CommentApproved, Flag, IpAddress)VALUES(@ConversationID, @UserID, @CommentText, '0', @Flag, @IpAddress)";
                }

                // Insert List to DB and Return New ID
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd = new SqlCommand(InsertCommentSQL, sqlConn);
                cmd.Parameters.Add("@ConversationID", SqlDbType.VarChar, 255).Value =
                    Session["ConversationID"].ToString();
                cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 255).Value = Session["UserID"].ToString();
                cmd.Parameters.Add("@CommentText", SqlDbType.NVarChar, -1).Value = sCommentText;
                cmd.Parameters.Add("@IpAddress", SqlDbType.VarChar, 255).Value = IpAddress();

                // Change Parameter to update Comment Flag based on if Profanity Filter tripped
                if (sFlag == "True")
                {
                    cmd.Parameters.Add("@Flag", SqlDbType.VarChar, 255).Value = "1";
                }
                else
                {
                    cmd.Parameters.Add("@Flag", SqlDbType.VarChar, 255).Value = "0";
                }

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Set Conversation Unread
                SetUnread();


                // Set Conversation Flagged
                if (sFlag == "True")
                {
                    SetFlagged();
                }

                // Send Receipt to Leader

                // Instantiate SQL String
                string SelectEmailSQL;
                // Select SQL            
                SelectEmailSQL = "SELECT UserEmail, optOut FROM tbl_UsersMaster WHERE UserID = @LeaderID";

                // Use SQL Statement to Select Records from DB    
                var sqlConn2 =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd2 = new SqlCommand(SelectEmailSQL, sqlConn2);
                cmd2.Parameters.Add("@LeaderID", SqlDbType.VarChar, 255).Value = lbl_LeaderID.Text;

                cmd2.Connection.Open();
                SqlDataReader rdr;
                rdr = cmd2.ExecuteReader();
                while (rdr.Read())
                {
                    // Check if Org has disabled Conversation Emails
                    string scommentEmail = Session["commentEmail"].ToString();

                    // Check if Leader has Opted Out of Email
                    string soptOut = rdr[1].ToString();

                    if (scommentEmail == "True" & soptOut == "False")
                    {
                        // Send Leader a Receipt
                        var mail = new MailMessage();
                        mail.To.Add(rdr[0].ToString());
                        mail.From = new MailAddress("info@feedbacksafe.com");
                        mail.Subject = "Feedback Safe New Comment Added";
                        mail.IsBodyHtml = true;
                        mail.BodyEncoding = Encoding.UTF8;
                        string body = FetchTemplate.ReadFile("/Templates/commentLeaderTemplate.htm");
                        body = body.Replace("{TokenCommentLeader}", txt_addComment.Text);
                        mail.Body = body;
                        var smtp = new SmtpClient();
                        smtp.Send(mail);
                    }
                }
                cmd2.Connection.Close();
                cmd2.Connection.Dispose();


                // Send Receipt to User

                // Instantiate SQL String
                string SelectEmailPersonSQL;
                // Select SQL            
                SelectEmailPersonSQL =
                    "SELECT UserEmail, optOut, optOutComment FROM tbl_UsersMaster WHERE UserID = @UserID";

                // Use SQL Statement to Select Records from DB    
                var sqlConn3 =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd3 = new SqlCommand(SelectEmailPersonSQL, sqlConn3);
                cmd3.Parameters.Add("@UserID", SqlDbType.VarChar, 255).Value = Session["UserID"].ToString();

                cmd3.Connection.Open();
                SqlDataReader rdr2;
                rdr2 = cmd3.ExecuteReader();
                while (rdr2.Read())
                {
                    // Check if User has Opted Out of Email
                    string soptOut = rdr2[1].ToString();
                    string soptOutComment = rdr2[2].ToString();

                    if (soptOut == "False" && soptOutComment == "False")
                        // Send User a Receipt
                    {
                        var mail = new MailMessage();
                        mail.To.Add(rdr2[0].ToString());
                        mail.From = new MailAddress("info@feedbacksafe.com");
                        mail.Subject = "Feedback Safe: You Added a Comment";
                        mail.IsBodyHtml = true;
                        mail.BodyEncoding = Encoding.UTF8;
                        string body = FetchTemplate.ReadFile("/Templates/commentPersonTemplate.htm");
                        body = body.Replace("{TokenCommentPerson}", txt_addComment.Text);
                        mail.Body = body;
                        var smtp = new SmtpClient();
                        smtp.Send(mail);
                    }
                }
                cmd3.Connection.Close();
                cmd3.Connection.Dispose();

                // Clear Textbox
                txt_addComment.Text = "";

                // Rebind Datalist
                BindConversation();
            }
        }

        //// GET IP ADDRESS ////

        private string IpAddress()
        {
            string sIpAddress;
            sIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (sIpAddress == null)
                sIpAddress = Request.ServerVariables["REMOTE_ADDR"];
            return sIpAddress;
        }


        //// POPULATE LABELS ////

        public void PopulateLabels()
        {
            string sAskedLeaderID = lbl_LeaderID.Text;

            // Select Lists from DB
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
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