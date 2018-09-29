using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeedbackSafe.Leader
{
    public partial class SendWelcome : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_sendMail_Click(object sender, EventArgs e)
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
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand(SelectUsersSQL, sqlConn);

            cmd.Connection.Open();
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                // Get Master Label
                var lbl_masterOrg = (Label) Master.FindControl("lbl_masterOrg");
                string sOrgName = lbl_masterOrg.Text;

                var mail = new MailMessage();
                mail.To.Add(rdr[0].ToString());
                mail.From = new MailAddress("info@feedbacksafe.com");
                mail.Subject = "Welcome to Feedback Safe with " + sOrgName;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                string body = FetchTemplate.ReadFile("/Templates/welcomeTemplate.htm");
                body = body.Replace("{UserName}", rdr[1] + " " + rdr[2] + " " + rdr[3] + " " + rdr[4]);
                body = body.Replace("{Organization}", sOrgName);
                body = body.Replace("{Token1}", rdr[5].ToString());
                body = body.Replace("{Token2}", rdr[5].ToString());
                mail.Body = body;
                var smtp = new SmtpClient();
                smtp.Send(mail);
            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
            // Send to Dashboard
            Response.Redirect("/Leader/Default.aspx");
        }
    }
}