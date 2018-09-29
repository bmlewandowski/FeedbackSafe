using System;
using System.Web.UI;

namespace FeedbackSafe.Agent
{
    public partial class Agent : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If User Isn't in AGENT Role


            // Set OrgName Label to Session Variable
            if (Session["OrgName"] != null)
            {
                lbl_masterOrg.Text = Session["OrgName"].ToString();
            }
        }
    }
}