using System;
using System.Web.UI;

namespace FeedbackSafe.Leader
{
    public partial class Leader : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Set OrgName Label to Session Variable
            if (Session["OrgName"] != null)
            {
                lbl_masterOrg.Text = Session["OrgName"].ToString();
            }

            // If OrgEnabled on Postback is False
            if (IsPostBack)
            {
                if (lbl_orgEnabled.Text == "false")
                {
                    Response.Redirect("/Disabled.aspx");

                }
            }

        }
    }
}