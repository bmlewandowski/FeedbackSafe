using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;

namespace FeedbackSafe
{
    public partial class Initiate : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get Token from Session 
            string sLeaderName = Session["AskLeaderName"].ToString();
            lbl_leaderAskedName.Text = sLeaderName;

            string sLeaderID = Session["AskLeaderID"].ToString();
            lbl_leaderAsked.Text = sLeaderID;


            // Get Leader Login Name

            // Instantiate SQL String
            string SelectLeaderLoginSQL;

            // Select SQL            
            SelectLeaderLoginSQL = "SELECT aspnetId FROM tbl_UsersMaster WHERE UserID ='" + sLeaderID + "'";

            // Use SQL Statement to Select Records from DB    
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand(SelectLeaderLoginSQL, sqlConn);

            cmd.Connection.Open();
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                // Create strings from reader
                string sleaderAskedId = rdr["aspnetId"].ToString();
                lbl_leaderAspnetId.Text = sleaderAskedId;
            }
            cmd.Connection.Close();
            cmd.Connection.Dispose();

            // Check for image in User Links folder
            string imgDisplayPath = Server.MapPath("~/Images/Users/" + lbl_leaderAspnetId.Text + "/user_defined.jpg");
            if (File.Exists(imgDisplayPath))
            {
                // Set Image Control URL 
                img_askedLeader.ImageUrl = "~/Images/Users/" + lbl_leaderAspnetId.Text + "/user_defined.jpg";
            }
        }

        protected void btn_askQuestion_Click(object sender, ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                // Instantiate SQL String
                string InsertConvSQL;

                // Get Session Variable
                string sautoApproveConversation = Session["autoApproveConversation"].ToString();

                // Set SQL based on AutoApproveConversation Session variable
                if (sautoApproveConversation == "True")
                {
                    InsertConvSQL =
                        "INSERT INTO tbl_Conversations (OrgID, UserID, LeaderID, ConversationPrivate, ConversationApproved, ConversationUnread, IpAddress)VALUES(@OrgID, @UserID, @LeaderID, @Private, '1', '1', @IpAddress);SELECT @@IDENTITY";
                }
                else
                {
                    InsertConvSQL =
                        "INSERT INTO tbl_Conversations (OrgID, UserID, LeaderID, ConversationPrivate, ConversationApproved, ConversationUnread, IpAddress)VALUES(@OrgID, @UserID, @LeaderID, @Private, '0', '1', @IpAddress);SELECT @@IDENTITY";
                }

                // Insert new Conversation to DB and Return New ID
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd = new SqlCommand(InsertConvSQL, sqlConn);
                cmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 255).Value = Session["OrgID"].ToString();
                cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 255).Value = Session["UserID"].ToString();
                cmd.Parameters.Add("@LeaderID", SqlDbType.VarChar, 255).Value = Session["AskLeaderID"].ToString();
                cmd.Parameters.Add("@IpAddress", SqlDbType.VarChar, 255).Value = IpAddress();

                if (chk_isPrivate.Checked)
                {
                    cmd.Parameters.Add("@Private", SqlDbType.VarChar, 255).Value = "True";
                }
                else
                {
                    cmd.Parameters.Add("@Private", SqlDbType.VarChar, 255).Value = "False";
                }


                cmd.Connection.Open();
                Int32 NewConvID = Convert.ToInt32(cmd.ExecuteScalar());
                lbl_ConvID.Text = NewConvID.ToString();
                cmd.Connection.Close();
                cmd.Connection.Dispose();


                // Set SQL based on AutoApproveComment Session variable
                // If we want to force the user to Approve the first Comment of the Conversation
                // Else we just count this first Comment AS the Conversation and rely on above

                // Insert Comment to DB using New ConversationID
                var sqlConn2 =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd2 =
                    new SqlCommand(
                        "INSERT INTO tbl_Comments (ConversationID, UserID, CommentText, CommentApproved, IpAddress)VALUES(@ConversationID, @UserID, @CommentText, '1', @IpAddress)",
                        sqlConn2);
                cmd2.Parameters.Add("@ConversationID", SqlDbType.VarChar, 255).Value = lbl_ConvID.Text;
                cmd2.Parameters.Add("@UserID", SqlDbType.VarChar, 255).Value = Session["UserID"].ToString();
                cmd2.Parameters.Add("@CommentText", SqlDbType.NVarChar, -1).Value = txt_askedQuestion.Text;
                cmd2.Parameters.Add("@IpAddress", SqlDbType.VarChar, 255).Value = IpAddress();
                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
                cmd2.Connection.Dispose();

                // Send Receipt to Leader

                // Instantiate SQL String
                string SelectEmailSQL;
                // Select SQL            
                SelectEmailSQL = "SELECT UserEmail, optOut FROM tbl_UsersMaster WHERE UserID = @LeaderID";

                // Use SQL Statement to Select Records from DB    
                var sqlConn3 =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd3 = new SqlCommand(SelectEmailSQL, sqlConn3);
                cmd3.Parameters.Add("@LeaderID", SqlDbType.VarChar, 255).Value = Session["AskLeaderID"].ToString();

                cmd3.Connection.Open();
                SqlDataReader rdr;
                rdr = cmd3.ExecuteReader();
                while (rdr.Read())
                {
                    // Check if Org has disabled Conversation Emails
                    string sconversationEmail = Session["conversationEmail"].ToString();

                    // Check if Leader has Opted Out of Email
                    string soptOut = rdr[1].ToString();

                    if (sconversationEmail == "True" & soptOut == "False")
                    {
                        // Send Leader a Receipt
                        var mail = new MailMessage();
                        mail.To.Add(rdr[0].ToString());
                        mail.From = new MailAddress("info@feedbacksafe.com");
                        mail.Subject = "Feedback Safe New Conversation Added";
                        mail.IsBodyHtml = true;
                        mail.BodyEncoding = Encoding.UTF8;
                        string body = FetchTemplate.ReadFile("/Templates/conversationLeaderTemplate.htm");
                        body = body.Replace("{TokenConvLeader}", txt_askedQuestion.Text);
                        mail.Body = body;
                        var smtp = new SmtpClient();
                        smtp.Send(mail);
                    }
                }
                cmd3.Connection.Close();
                cmd3.Connection.Dispose();

                // Send Receipt to User

                // Instantiate SQL String
                string SelectEmailPersonSQL;
                // Select SQL            
                SelectEmailPersonSQL =
                    "SELECT UserEmail, optOut, optOutConversation FROM tbl_UsersMaster WHERE UserID = @UserID";

                // Use SQL Statement to Select Records from DB    
                var sqlConn4 =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd4 = new SqlCommand(SelectEmailPersonSQL, sqlConn4);
                cmd4.Parameters.Add("@UserID", SqlDbType.VarChar, 255).Value = Session["UserID"].ToString();

                cmd4.Connection.Open();
                SqlDataReader rdr2;
                rdr2 = cmd4.ExecuteReader();
                while (rdr2.Read())
                {
                    // Check if User has Opted Out of Email
                    string soptOut = rdr2[1].ToString();
                    string soptOutConversation = rdr2[2].ToString();

                    if (soptOut == "False" && soptOutConversation == "False")
                        // Send User a Receipt
                    {
                        var mail = new MailMessage();
                        mail.To.Add(rdr2[0].ToString());
                        mail.From = new MailAddress("info@feedbacksafe.com");
                        mail.Subject = "Feedback Safe: You Started a Conversation";
                        mail.IsBodyHtml = true;
                        mail.BodyEncoding = Encoding.UTF8;
                        string body = FetchTemplate.ReadFile("/Templates/conversationPersonTemplate.htm");
                        body = body.Replace("{TokenConvPerson}", txt_askedQuestion.Text);
                        mail.Body = body;
                        var smtp = new SmtpClient();
                        smtp.Send(mail);
                    }
                }
                cmd4.Connection.Close();
                cmd4.Connection.Dispose();

                // Send Back to Dashboard
                Response.Redirect("Person.aspx");
            }
        }

        // Start Checkboxes
        protected void chk_isPrivate_CheckedChanged(object sender, EventArgs e)
        {
            // Check Private Checkbox and Adjust Image 
            if (chk_isPrivate.Checked)
            {
                img_markPrivate.ImageUrl = "~/Images/Interface/private_small.png";
            }

            else
            {
                img_markPrivate.ImageUrl = "~/Images/Interface/private_small_faded.png";
            }
        }

        // End Checkboxes

        // Get IP Address
        private string IpAddress()
        {
            string sIpAddress;
            sIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (sIpAddress == null)
                sIpAddress = Request.ServerVariables["REMOTE_ADDR"];
            return sIpAddress;
        }
    }
}