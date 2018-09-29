using System;
using System.Web.UI;

namespace FeedbackSafe.Sales
{
    public partial class Sales : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Set SalesID Label to Session Variable
            if (Session["SalesID"] != null)
            {
                lbl_masterSalesID.Text = Session["SalesID"].ToString();
            }

            // Set OrgName Label to Session Variable
            if (Session["OrgName"] != null)
            {
                lbl_masterOrg.Text = Session["OrgName"].ToString();
            }
        }
    }
}