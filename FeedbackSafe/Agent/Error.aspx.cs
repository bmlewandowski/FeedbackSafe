using System;
using System.Web.UI;

namespace FeedbackSafe.Agent
{
    public partial class Error : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If Error Code Provided in Querystring, write to Label
            if (Request.QueryString["ErrorCode"] != null)
            {
                lbl_errorMessage.Text = Request.QueryString["ErrorCode"];
            }
        }
    }
}