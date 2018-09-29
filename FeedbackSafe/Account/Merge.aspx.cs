using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace FeedbackSafe.Account
{
    public partial class Merge : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get UserID from Session
            string sUserID = Session["UserID"].ToString();
            // Get ASP.NET User ID from Membership
            string saspnetID = FetchUser.UserID();
            string MergeAccountSQL = "UPDATE tbl_UsersMaster SET aspnetID = '" + saspnetID + "' WHERE UserID = '" +
                                     sUserID + "'";

            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand(MergeAccountSQL, sqlConn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();

            Response.Redirect("/Catch.aspx");
        }
    }
}