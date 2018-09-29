using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;

namespace FeedbackSafe
{
    public partial class Profile : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // First Load Events
            if (!IsPostBack)
            {
                CheckPreferences();
            }
        }

        protected void btn_savePreferences_Click(object sender, EventArgs e)
        {
            // Save Preferences to UsersMaster and switch divs

            // Instantiate SQL String
            string SetProfileSQL;

            // Get UserID from Session
            string sUserID = Session["UserID"].ToString();

            // Select SQL            
            SetProfileSQL =
                "UPDATE tbl_UsersMaster SET optOut = @optOut, optOutConversation = @optOutConversation, optOutComment = @optOutComment WHERE UserID ='" +
                sUserID + "'";

            // Use SQL Statement to Select Records from DB    
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand(SetProfileSQL, sqlConn);

            if (chk_optOut.Checked)
            {
                cmd.Parameters.Add("@optOut", SqlDbType.VarChar, 1).Value = "1";
            }
            else
            {
                cmd.Parameters.Add("@optOut", SqlDbType.VarChar, 1).Value = "0";
            }

            if (chk_optOutConversation.Checked)
            {
                cmd.Parameters.Add("@optOutConversation", SqlDbType.VarChar, 1).Value = "1";
            }
            else
            {
                cmd.Parameters.Add("@optOutConversation", SqlDbType.VarChar, 1).Value = "0";
            }

            if (chk_optOutComment.Checked)
            {
                cmd.Parameters.Add("@optOutComment", SqlDbType.VarChar, 1).Value = "1";
            }
            else
            {
                cmd.Parameters.Add("@optOutComment", SqlDbType.VarChar, 1).Value = "0";
            }

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();

            changePreferences.Visible = false;
            savedPreferences.Visible = true;
        }


        public void CheckPreferences()
        {
            // Get UserID from Session
            string sUserID = Session["UserID"].ToString();

            // Select Lists from DB
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd =
                new SqlCommand(
                    "SELECT UserEmail, optOut, optOutConversation, optOutComment, aspnetId FROM tbl_UsersMaster WHERE UserID ='" +
                    sUserID + "'", sqlConn);
            cmd.Connection.Open();

            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string soptOut = rdr["optOut"].ToString();
                string soptOutConversation = rdr["optOutConversation"].ToString();
                string soptOutComment = rdr["optOutComment"].ToString();
                string saspnetId = rdr["aspnetId"].ToString();

                lbl_userEmail.Text = rdr["UserEmail"].ToString();

                if (rdr["aspnetId"] != DBNull.Value && saspnetId != "")
                {
                    // Hide Add Password div
                    addPassword.Visible = false;
                }

                if (soptOut == "True")
                {
                    chk_optOut.Checked = true;
                }

                if (soptOutConversation == "True")
                {
                    chk_optOutConversation.Checked = true;
                }

                if (soptOutComment == "True")
                {
                    chk_optOutComment.Checked = true;
                }
            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        protected void btn_addPassword_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (Session["UserID"] != null)
                {
                    // Create ASPNET User Account and move ASPNET ID to Users Master for cross reference

                    string suserEmail = lbl_userEmail.Text;
                    string suserPassword = txt_addPassword.Text;

                    if (Membership.GetUser(suserEmail) == null)
                    {
                        Membership.CreateUser(suserEmail, suserPassword, suserEmail);
                        FormsAuthentication.SetAuthCookie(suserEmail, false /* createPersistentCookie */);
                        Response.Redirect("/Account/Merge.aspx");
                    }
                    else // Email Already Exists
                    {
                        Response.Redirect("/Error.aspx?ErrorCode=Email_Already_Exists_As_User");
                    }
                }

                else // UserID Session is lost
                {
                    Response.Redirect("/Error.aspx?ErrorCode=UserID_Session_Was_Lost");
                }
            }
        }
    }
}