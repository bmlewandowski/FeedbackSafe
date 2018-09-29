using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeedbackSafe.Agent
{
    public partial class ManageLeaders : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // First Load Events
            if (!IsPostBack)
            {
                BindOrg();
            }
        }

        //// BIND ORGID FROM SESSION VARIABLE TO SQL PARAMETER ////

        protected void BindOrg()
        {
            if (Session["OrgID"] != null)
            {
                string sOrgID = Session["OrgID"].ToString();
                SqlDataSource_ManageUser.SelectParameters["OrgID"].DefaultValue = sOrgID;
                ManageUser_ListView.DataBind();
            }
            else // OrgID Session is lost
            {
                Response.Redirect("/Agent/Error.aspx?ErrorCode=OrgID_Session_Was_Lost");
            }
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
        }

        //// END LISTVIEW EVENTS ////


        //// BEGIN BUTTONS ////

        protected void btn_AddUser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Add New User to tbl_UsersMaster, set to current OrgID and generate new GUID

                if (Session["OrgID"] != null)
                {
                    string sOrgID = Session["OrgID"].ToString();

                    if (sOrgID != "0")
                    {
                        // Insert User to DB
                        var sqlConn =
                            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                        var cmd =
                            new SqlCommand(
                                "INSERT INTO tbl_UsersMaster (OrgID, UserEmail, UserTitle, UserFirstName, UserMiddleName, UserLastName, isLeader)VALUES(@OrgID, @UserEmail, @UserTitle, @UserFirstName, @UserMiddleName, @UserLastName, 'True');SELECT @@IDENTITY",
                                sqlConn);


                        cmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 255).Value = sOrgID;
                        cmd.Parameters.Add("@UserEmail", SqlDbType.VarChar, 255).Value = txt_AddUserEmail.Text;
                        cmd.Parameters.Add("@UserTitle", SqlDbType.VarChar, 255).Value = txt_AddUserTitle.Text;
                        cmd.Parameters.Add("@UserFirstName", SqlDbType.VarChar, 255).Value = txt_AddUserFirstName.Text;
                        cmd.Parameters.Add("@UserMiddleName", SqlDbType.VarChar, 255).Value = txt_AddUserMiddleName.Text;
                        cmd.Parameters.Add("@UserLastName", SqlDbType.VarChar, 255).Value = txt_AddUserLastName.Text;

                        cmd.Connection.Open();
                        Int32 NewConvID = Convert.ToInt32(cmd.ExecuteScalar());
                        lbl_newLeaderID.Text = NewConvID.ToString();
                        //cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                        cmd.Connection.Dispose();


                        // TO DO
                        // Create ASP.NET ID Set Role and Create User Folder

                        string sleaderUsername = txt_AddUserLogin.Text;
                        string sleaderPassword = txt_addPassword.Text;
                        string sleaderEmail = txt_AddUserEmail.Text;

                        // Create ASP.NET Membership User
                        Membership.CreateUser(sleaderUsername, sleaderPassword, sleaderEmail);

                        // Set Leader Role
                        Roles.AddUserToRole(sleaderUsername, "Leader");


                        // Get ASP.NET User ID from Membership
                        MembershipUser userObject = Membership.GetUser(sleaderUsername);
                        string sUseraspnet = userObject.ProviderUserKey.ToString();
                        string saspnetID = sUseraspnet;


                        // Create User Folder for Images
                        string UserFolderPath = Server.MapPath("~/Images/Users/" + saspnetID);
                        Directory.CreateDirectory(UserFolderPath);

                        // Move ASP.NET ID Over to tbl_UsersMaster
                        string sUserID = lbl_newLeaderID.Text;


                        string MergeAccountSQL = "UPDATE tbl_UsersMaster SET aspnetID = '" + saspnetID +
                                                 "' WHERE UserID = '" + sUserID + "'";

                        var sqlConn2 =
                            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                        var cmd2 = new SqlCommand(MergeAccountSQL, sqlConn2);
                        cmd2.Connection.Open();
                        cmd2.ExecuteNonQuery();
                        cmd2.Connection.Close();
                        cmd2.Connection.Dispose();

                        // Send Wecome Email to New User (checkbox?)
                        //MailMessage mail = new MailMessage();
                        //mail.To.Add(this.txt_AddUserEmail.Text);
                        //mail.From = new MailAddress("info@feedbacksafe.com");
                        //mail.Subject = "Welcome to Feedback Safe";
                        //mail.IsBodyHtml = true;
                        //mail.BodyEncoding = System.Text.Encoding.UTF8;
                        //string body = FetchTemplate.ReadFile("/Templates/welcomeTemplate.htm");
                        //body = body.Replace("{UserName}", this.txt_AddUserTitle.Text + " " + this.txt_AddUserFirstName.Text + " " + this.txt_AddUserMiddleName.Text + " " + this.txt_AddUserLastName.Text);
                        //body = body.Replace("{Token1}", sUserToken);
                        //body = body.Replace("{Token2}", sUserToken);
                        //mail.Body = body;

                        //SmtpClient smtp = new SmtpClient();
                        //smtp.Send(mail);


                        // Clear Textbox
                        txt_AddUserEmail.Text = "";
                        txt_AddUserTitle.Text = "";
                        txt_AddUserFirstName.Text = "";
                        txt_AddUserMiddleName.Text = "";
                        txt_AddUserLastName.Text = "";
                        txt_AddUserLogin.Text = "";
                        txt_addPassword.Text = "";
                        txt_confirmPassword.Text = "";

                        // Call ListView
                        BindOrg();

                        // Hide Div
                        addLeader.Visible = false;
                    }

                    else // OrgID Session is lost
                    {
                        Response.Redirect("/Agent/Error.aspx?ErrorCode=OrgID_Session_Was_Lost");
                    }
                }
            }

            else // OrgID Session is lost
            {
                Response.Redirect("/Agent/Error.aspx?ErrorCode=OrgID_Session_Was_Lost");
            }
        }


        protected void lbn_addLeaderDiv_Click(object sender, EventArgs e)
        {
            if (addLeader.Visible)
            {
                addLeader.Visible = false;
            }

            else
            {
                addLeader.Visible = true;
            }
        }


        protected void lbn_editLeaderDiv_Click(object sender, EventArgs e)
        {
            if (editLeaders.Visible)
            {
                editLeaders.Visible = false;
            }

            else
            {
                editLeaders.Visible = true;
            }
        }

        //// END BUTTONS ////


        //// CHECK FOR DUPLICATE ASP.NET MEMBERSHIP USERNAME ////

        protected void DuplicateNameCheck_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Create MembershipUserCollection to collate a list of duplicate email addresses
            MembershipUserCollection memCollection = Membership.FindUsersByName(args.Value);

            //If duplicate email addresses are found then error
            if (memCollection.Count > 0)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }
    }
}