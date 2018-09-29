using System;
using System.Web.UI;

namespace FeedbackSafe
{
    public partial class Catch : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // First Load Events
            if (!IsPostBack)
            {
                // IF LEADER SEND TO LEADER DASHBOARD
                if (User.IsInRole("Leader"))
                {
                    Response.Redirect("/Leader/Default.aspx");
                }

                    // ELSE IF AGENT SENT TO AGENT DASHBOARD
                else if (User.IsInRole("Agent"))
                {
                    Response.Redirect("/Agent/Default.aspx");
                }

                    // ELSE IF SALES SENT TO SALES DASHBOARD
                else if (User.IsInRole("Sales"))
                {
                    Response.Redirect("/Sales/Default.aspx");
                }

                    // ELSE SEND TO PERSON DASHBOARD
                else
                {
                    Response.Redirect("/Person.aspx");
                }
            }
        }
    }
}