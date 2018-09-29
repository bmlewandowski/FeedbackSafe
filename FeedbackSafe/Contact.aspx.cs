using System;
using System.Net.Mail;
using System.Web.UI;

namespace FeedbackSafe
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_submit_feedback_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string emailFrom = txt_fb_name.Text;
                    string emailMail = txt_fb_email.Text;
                    string emailBody = txt_fb_comments.Text;
                    var message = new MailMessage("info@feedbacksafe.com", "info@feedbacksafe.com",
                                                  "Feedback Safe Contact Form",
                                                  "Name: " + emailFrom + "<br />Email: " + emailMail + "<br />" +
                                                  emailBody);
                    message.IsBodyHtml = true;
                    var emailClient = new SmtpClient();
                    emailClient.Send(message);
                    div_fb_form.Visible = false;
                    div_fb_success.Visible = true;
                }
                catch (Exception ex)
                {
                    div_fb_form.Visible = false;
                    div_fb_success.Visible = true;
                }
            }
        }
    }
}