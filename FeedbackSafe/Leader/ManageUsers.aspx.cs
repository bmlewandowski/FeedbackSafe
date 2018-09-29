using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace FeedbackSafe.Leader
{
    public partial class ManageUsers : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // First Load Events
            if (!IsPostBack)
            {
                BindOrg();
            }
        }

        //// LISTVIEW DATABIND ////

        protected void BindOrg()
        {
            string sOrgID = Session["OrgID"].ToString();
            SqlDataSource_ManageUser.SelectParameters["OrgID"].DefaultValue = sOrgID;
            ManageUser_ListView.DataBind();
        }

        //// BEGIN LISTVIEW EVENTS ////

        protected void ManageUser_ListView_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                // Find Controls in ListView

                var lbl_RowUserID = (Label) e.Item.FindControl("lbl_RowUserID");
                var txt_OrgName = (TextBox) e.Item.FindControl("txt_OrgName");
                var txt_UserEmail = (TextBox) e.Item.FindControl("txt_UserEmail");
                var txt_UserTitle = (TextBox) e.Item.FindControl("txt_UserTitle");
                var txt_UserFirstName = (TextBox) e.Item.FindControl("txt_UserFirstName");
                var txt_UserMiddleName = (TextBox) e.Item.FindControl("txt_UserMiddleName");
                var txt_UserLastName = (TextBox) e.Item.FindControl("txt_UserLastName");

                var rec = (DataRowView) e.Item.DataItem;

                // Set Update Parameters
                SqlDataSource_ManageUser.UpdateParameters["UserID"].DefaultValue = lbl_RowUserID.Text;
                SqlDataSource_ManageUser.UpdateParameters["UserEmail"].DefaultValue = txt_UserEmail.Text;
                SqlDataSource_ManageUser.UpdateParameters["UserTitle"].DefaultValue = txt_UserTitle.Text;
                SqlDataSource_ManageUser.UpdateParameters["UserFirstName"].DefaultValue = txt_UserFirstName.Text;
                SqlDataSource_ManageUser.UpdateParameters["UserMiddleName"].DefaultValue = txt_UserMiddleName.Text;
                SqlDataSource_ManageUser.UpdateParameters["UserLastName"].DefaultValue = txt_UserLastName.Text;
            }

            if (e.CommandName == "Delete")
            {
                // Find Controls in ListView

                var lbl_RowUserID = (Label) e.Item.FindControl("lbl_RowUserID");


                var rec = (DataRowView) e.Item.DataItem;

                // Set Update Parameters
                SqlDataSource_ManageUser.DeleteParameters["UserID"].DefaultValue = lbl_RowUserID.Text;

                // Delete User from ASP.NET Membership
                // Membership.DeleteUser();
            }
        }

        //// END LISTVIEW EVENTS ////

        //// BEGIN BUTTONS ////

        protected void btn_AddUser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Add New User to tbl_UsersMaster, set to current OrgID and generate new GUID

                string sOrgID = Session["OrgID"].ToString();
                string sUserToken = Guid.NewGuid().ToString();
                // Insert User to DB
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd =
                    new SqlCommand(
                        "INSERT INTO tbl_UsersMaster (OrgID, UserEmail, UserTitle, UserFirstName, UserMiddleName, UserLastName, UserToken)VALUES(@OrgID, @UserEmail, @UserTitle, @UserFirstName, @UserMiddleName, @UserLastName, @UserToken)",
                        sqlConn);


                cmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 255).Value = sOrgID;
                cmd.Parameters.Add("@UserEmail", SqlDbType.VarChar, 255).Value = txt_AddUserEmail.Text;
                cmd.Parameters.Add("@UserTitle", SqlDbType.VarChar, 255).Value = txt_AddUserTitle.Text;
                cmd.Parameters.Add("@UserFirstName", SqlDbType.VarChar, 255).Value = txt_AddUserFirstName.Text;
                cmd.Parameters.Add("@UserMiddleName", SqlDbType.VarChar, 255).Value = txt_AddUserMiddleName.Text;
                cmd.Parameters.Add("@UserLastName", SqlDbType.VarChar, 255).Value = txt_AddUserLastName.Text;
                cmd.Parameters.Add("@UserToken", SqlDbType.VarChar, 255).Value = sUserToken;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Check ASP.NET for Duplicate email
                // STORED PROCEDURE            


                // Send Wecome Email to New User (checkbox?)
                var mail = new MailMessage();
                mail.To.Add(txt_AddUserEmail.Text);
                mail.From = new MailAddress("info@feedbacksafe.com");
                mail.Subject = "Welcome to Feedback Safe";
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                string body = FetchTemplate.ReadFile("/Templates/welcomeTemplate.htm");
                body = body.Replace("{UserName}",
                                    txt_AddUserTitle.Text + " " + txt_AddUserFirstName.Text + " " +
                                    txt_AddUserMiddleName.Text + " " + txt_AddUserLastName.Text);
                body = body.Replace("{Token1}", sUserToken);
                body = body.Replace("{Token2}", sUserToken);
                mail.Body = body;

                var smtp = new SmtpClient();
                smtp.Send(mail);


                // Clear Textbox
                txt_AddUserEmail.Text = "";
                txt_AddUserTitle.Text = "";
                txt_AddUserFirstName.Text = "";
                txt_AddUserMiddleName.Text = "";
                txt_AddUserLastName.Text = "";

                // Call ListView Databind
                BindOrg();
            }
        }

        protected void lbn_addUserDiv_Click(object sender, EventArgs e)
        {
            if (addUser.Visible)
            {
                addUser.Visible = false;
            }

            else
            {
                addUser.Visible = true;
            }
        }

        protected void lbn_editUserDiv_Click(object sender, EventArgs e)
        {
            if (editUsers.Visible)
            {
                editUsers.Visible = false;
            }

            else
            {
                editUsers.Visible = true;
            }
        }

        //// END BUTTONS ////
    }
}