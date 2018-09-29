using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeedbackSafe.Agent
{
    public partial class ManageSales : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // First Load Events
            if (!IsPostBack)
            {
                BindStates();
                BindOrg();
            }
        }

        //// BIND ORGID FROM SESSION VARIABLE TO SQL PARAMETER ////

        protected void BindOrg()
        {
            //string sOrgID = Session["OrgID"].ToString();
            //SqlDataSource_ManageUser.SelectParameters["OrgID"].DefaultValue = sOrgID;
            ManageUser_ListView.DataBind();
        }


        //// BEGIN LISTVIEW EVENTS ////

        protected void ManageUser_ListView_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                // Find Controls in ListView

                var lbl_RowUserID = (Label) e.Item.FindControl("lbl_RowUserID");
                var txt_SalesFirstName = (TextBox) e.Item.FindControl("txt_SalesFirstName");
                var txt_SalesMiddleName = (TextBox) e.Item.FindControl("txt_SalesMiddleName");
                var txt_SalesLastName = (TextBox) e.Item.FindControl("txt_SalesLastName");
                var txt_SalesAddress = (TextBox) e.Item.FindControl("txt_SalesAddress");
                var txt_SalesCity = (TextBox) e.Item.FindControl("txt_SalesCity");
                var txt_SalesState = (TextBox) e.Item.FindControl("txt_SalesState");
                var txt_SalesZip = (TextBox) e.Item.FindControl("txt_SalesZip");
                var txt_SalesPhone = (TextBox) e.Item.FindControl("txt_SalesPhone");
                var txt_SalesPhone2 = (TextBox) e.Item.FindControl("txt_SalesPhone2");
                var txt_SalesEmail = (TextBox) e.Item.FindControl("txt_SalesEmail");
                var txt_SalesComments = (TextBox) e.Item.FindControl("txt_SalesComments");


                var rec = (DataRowView) e.Item.DataItem;

                // Set Update Parameters
                SqlDataSource_ManageUser.UpdateParameters["SalesID"].DefaultValue = lbl_RowUserID.Text;
                SqlDataSource_ManageUser.UpdateParameters["SalesEmail"].DefaultValue = txt_SalesEmail.Text;
                SqlDataSource_ManageUser.UpdateParameters["SalesFirstName"].DefaultValue = txt_SalesFirstName.Text;
                SqlDataSource_ManageUser.UpdateParameters["SalesMiddleName"].DefaultValue = txt_SalesMiddleName.Text;
                SqlDataSource_ManageUser.UpdateParameters["SalesLastName"].DefaultValue = txt_SalesLastName.Text;
                SqlDataSource_ManageUser.UpdateParameters["SalesAddress"].DefaultValue = txt_SalesAddress.Text;
                SqlDataSource_ManageUser.UpdateParameters["SalesCity"].DefaultValue = txt_SalesCity.Text;
                SqlDataSource_ManageUser.UpdateParameters["SalesState"].DefaultValue = txt_SalesState.Text;
                SqlDataSource_ManageUser.UpdateParameters["SalesZip"].DefaultValue = txt_SalesZip.Text;
                SqlDataSource_ManageUser.UpdateParameters["SalesPhone"].DefaultValue = txt_SalesPhone.Text;
                SqlDataSource_ManageUser.UpdateParameters["SalesPhone2"].DefaultValue = txt_SalesPhone2.Text;
                SqlDataSource_ManageUser.UpdateParameters["SalesComments"].DefaultValue = txt_SalesComments.Text;
            }
        }

        //// END LISTVIEW EVENTS ////

        //// BEGIN BUTTONS ////

        protected void btn_AddUser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Add New User to tbl_SalesMaster

                // Insert User to DB
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd =
                    new SqlCommand(
                        "INSERT INTO tbl_SalesMaster (SalesFirstName, SalesMiddleName, SalesLastName, SalesAddress, SalesCity, SalesState, SalesZip, SalesPhone, SalesPhone2, SalesEmail, SalesComments)VALUES(@SalesFirstName, @SalesMiddleName, @SalesLastName, @SalesAddress, @SalesCity, @SalesState, @SalesZip, @SalesPhone, @SalesPhone2, @SalesEmail, @SalesComments);SELECT @@IDENTITY",
                        sqlConn);

                cmd.Parameters.Add("@SalesFirstName", SqlDbType.VarChar, 255).Value = txt_AddUserFirstName.Text;
                cmd.Parameters.Add("@SalesMiddleName", SqlDbType.VarChar, 255).Value = txt_AddUserMiddleName.Text;
                cmd.Parameters.Add("@SalesLastName", SqlDbType.VarChar, 255).Value = txt_AddUserLastName.Text;
                cmd.Parameters.Add("@SalesAddress", SqlDbType.VarChar, 255).Value = txt_AddUserAddress.Text;
                cmd.Parameters.Add("@SalesCity", SqlDbType.VarChar, 255).Value = txt_AddUserCity.Text;
                cmd.Parameters.Add("@SalesState", SqlDbType.VarChar, 255).Value =
                    ddl_AddUserState.SelectedItem.ToString();
                cmd.Parameters.Add("@SalesZip", SqlDbType.VarChar, 255).Value = txt_AddUserZip.Text;
                cmd.Parameters.Add("@SalesPhone", SqlDbType.VarChar, 255).Value = txt_AddUserPhone.Text;
                cmd.Parameters.Add("@SalesPhone2", SqlDbType.VarChar, 255).Value = txt_AddUserPhone2.Text;
                cmd.Parameters.Add("@SalesEmail", SqlDbType.VarChar, 255).Value = txt_AddUserEmail.Text;
                cmd.Parameters.Add("@SalesComments", SqlDbType.NVarChar, -1).Value = txt_AddUserSalesComments.Text;


                cmd.Connection.Open();
                Int32 NewConvID = Convert.ToInt32(cmd.ExecuteScalar());
                lbl_newSalesID.Text = NewConvID.ToString();
                //cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();


                // TO DO
                // Create ASP.NET ID Set Role and Create User Folder

                string ssalesUsername = txt_AddUserLogin.Text;
                string ssalesPassword = txt_addPassword.Text;
                string ssalesEmail = txt_AddUserEmail.Text;

                // Create ASP.NET Membership User
                Membership.CreateUser(ssalesUsername, ssalesPassword, ssalesEmail);

                // Set Sales Role
                Roles.AddUserToRole(ssalesUsername, "Sales");

                // Move ASP.NET ID Over to tbl_SalesMaster
                string sSalesID = lbl_newSalesID.Text;

                // Get ASP.NET User ID from Membership
                MembershipUser userObject = Membership.GetUser(ssalesUsername);
                string sUserID = userObject.ProviderUserKey.ToString();
                string saspnetID = sUserID;

                string MergeAccountSQL = "UPDATE tbl_SalesMaster SET aspnetID = '" + saspnetID + "' WHERE SalesID = '" +
                                         sSalesID + "'";

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
                txt_AddUserFirstName.Text = "";
                txt_AddUserMiddleName.Text = "";
                txt_AddUserLastName.Text = "";
                txt_AddUserAddress.Text = "";
                txt_AddUserCity.Text = "";
                txt_AddUserZip.Text = "";
                txt_AddUserPhone.Text = "";
                txt_AddUserPhone2.Text = "";
                txt_AddUserSalesComments.Text = "";
                txt_AddUserLogin.Text = "";
                txt_addPassword.Text = "";
                txt_confirmPassword.Text = "";

                // Call ListView
                BindOrg();

                // Hide Div
                addSales.Visible = false;
            }
        }


        protected void lbn_addSalesDiv_Click(object sender, EventArgs e)
        {
            if (addSales.Visible)
            {
                addSales.Visible = false;
            }

            else
            {
                addSales.Visible = true;
            }
        }


        protected void lbn_editSalesDiv_Click(object sender, EventArgs e)
        {
            if (editSales.Visible)
            {
                editSales.Visible = false;
            }

            else
            {
                editSales.Visible = true;
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

        // Bind State Dropdown Lists
        public void BindStates()
        {
            /*bind states to drop-down list from XML*/
            var ds = new DataSet("States");
            ds.ReadXml(MapPath("/XML/States.xml"));

            ddl_AddUserState.DataSource = ds;
            ddl_AddUserState.DataTextField = "Abbr";
            ddl_AddUserState.DataValueField = "Name";
            ddl_AddUserState.DataBind();
            ddl_AddUserState.Items.Insert(0, new ListItem("[STATE]", "default"));
        }
    }
}