using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeedbackSafe.Sales
{
    public partial class ManageLeads : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // First Load Events
            if (!IsPostBack)
            {
                BindLeadsList();
            }
        }


        //// BEGIN LISTVIEW DATABIND ////

        protected void BindLeadsList()
        {
            //  string sOrgID = this.lbl_OrgID.Text;
            //  SqlDataSource_Leads.SelectParameters["OrgID"].DefaultValue = sOrgID;
            Leads_ListView.DataBind();
        }

        //// BEGIN DATALIST EVENTS ////

        protected void Leads_ListView_OnItemDataBound(Object sender, ListViewItemEventArgs e)
        {
            var rec = (DataRowView) e.Item.DataItem;

            var chk_SalesClaim = (CheckBox) e.Item.FindControl("chk_SalesClaim");

            // Make sure that you have the data
            if (rec != null)
            {
                // If SalesID is not 0 and SalesClaim Checkbox is found
                if (rec["SalesID"].ToString() != "0" && chk_SalesClaim != null)
                {
                    chk_SalesClaim.Checked = true;
                }
            }
        }

        protected void Leads_ListView_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                // Find Controls in ListView

                var lbl_RowLeadID = (Label) e.Item.FindControl("lbl_RowLeadID");
                var ddl_LeadStatus = (DropDownList) e.Item.FindControl("ddl_LeadStatus");
                var txt_Comment = (TextBox) e.Item.FindControl("txt_Comment");
                var chk_SalesClaim = (CheckBox) e.Item.FindControl("chk_SalesClaim");
                var chk_markFollowup = (CheckBox) e.Item.FindControl("chk_markFollowup");
                var cal_Followup = (Calendar) e.Item.FindControl("cal_Followup");

                var rec = (DataRowView) e.Item.DataItem;

                // Set Update Parameters
                SqlDataSource_Leads.UpdateParameters["LeadStatus"].DefaultValue = ddl_LeadStatus.SelectedValue;
                SqlDataSource_Leads.UpdateParameters["Comment"].DefaultValue = txt_Comment.Text;
                SqlDataSource_Leads.UpdateParameters["LeadID"].DefaultValue = lbl_RowLeadID.Text;


                // Check if Checked Claimed
                if (chk_SalesClaim.Checked)
                {
                    SqlDataSource_Leads.UpdateParameters["SalesID"].DefaultValue = Session["SalesID"].ToString();
                }
                else
                {
                    SqlDataSource_Leads.UpdateParameters["SalesID"].DefaultValue = "0";
                }

                // Check if Checked Claimed
                if (chk_markFollowup.Checked)
                {
                    SqlDataSource_Leads.UpdateParameters["FollowUpDate"].DefaultValue =
                        cal_Followup.SelectedDate.ToString();
                }
            }
        }
    }
}