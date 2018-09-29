using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.Web.UI;

namespace FeedbackSafe
{
    public partial class Recover : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_recoverKey_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Instantiate SQL String
                string SelectGuidSQL;

                // Get Token from Session 
                string sEmail = txt_userEmail.Text;


                // Select SQL            
                SelectGuidSQL = "SELECT UserEmail, UserToken FROM tbl_UsersMaster WHERE UserEmail = '" + sEmail + "'";

                // Use SQL Statement to Select Records from DB    
                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd = new SqlCommand(SelectGuidSQL, sqlConn);

                cmd.Connection.Open();
                SqlDataReader rdr;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var mail = new MailMessage();
                    mail.To.Add(rdr[0].ToString());
                    mail.From = new MailAddress("info@feedbacksafe.com");
                    mail.Subject = "Feedback Safe Email Key";
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = Encoding.UTF8;
                    string body = FetchTemplate.ReadFile("/Templates/recoverTemplate.htm");
                    body = body.Replace("{Token1}", rdr[1].ToString());
                    body = body.Replace("{Token2}", rdr[1].ToString());
                    mail.Body = body;
                    var smtp = new SmtpClient();
                    smtp.Send(mail);
                }
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                txt_userEmail.Text = "";
                div_requestkey.Visible = false;
                div_requestkeysuccess.Visible = true;
            }
        }
    }
}