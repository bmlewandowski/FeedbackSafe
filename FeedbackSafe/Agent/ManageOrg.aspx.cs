using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeedbackSafe.Agent
{
    public partial class ManageOrg : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // First Load Events
            if (!IsPostBack)
            {
                BindStates();
                BindOrgDropdown();
            }

            //  bindOrg();
        }

        //// BEGIN DROPDOWN BIND ////

        public void BindOrgDropdown()
        {
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand("SELECT * FROM tbl_OrgsMaster", sqlConn);
            cmd.Connection.Open();

            SqlDataReader ddlValues;
            ddlValues = cmd.ExecuteReader();
            ddl_selectOrg.DataSource = ddlValues;
            ddl_selectOrg.DataValueField = "OrgID";
            ddl_selectOrg.DataTextField = "OrgName";


            ddl_selectOrg.DataBind();
            ddl_selectOrg.Items.Insert(0, new ListItem("[Select Org]", "0"));
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        //// BEGIN DROPDOWN EVENTS ////
        protected void ddl_selectOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindOrg();
        }


        protected void bindOrg()
        {
            string sOrgID = ddl_selectOrg.SelectedValue;
            SqlDataSource_ManageOrg.SelectParameters["OrgID"].DefaultValue = sOrgID;
            ManageOrg_ListView.DataBind();
        }

        //// BEGIN DATALIST EVENTS ////

        protected void ManageOrg_ListView_OnItemDataBound(Object sender, ListViewItemEventArgs e)
        {
            var rec = (DataRowView)e.Item.DataItem;
            // Make sure that you have the data
            if (rec != null)
            {
                var chk_approveConversation = (CheckBox)e.Item.FindControl("chk_approveConversation");
                var chk_approveComment = (CheckBox)e.Item.FindControl("chk_approveComment");
                var chk_conversationEmail = (CheckBox)e.Item.FindControl("chk_conversationEmail");
                var chk_commentEmail = (CheckBox)e.Item.FindControl("chk_commentEmail");
                var chk_profanityFilter = (CheckBox)e.Item.FindControl("chk_profanityFilter");
                var chk_disableOrg = (CheckBox)e.Item.FindControl("chk_disableOrg");
                var chk_toggleapproveConversation = (CheckBox)e.Item.FindControl("chk_toggleapproveConversation");
                var chk_toggleapproveComment = (CheckBox)e.Item.FindControl("chk_toggleapproveComment");
                var chk_toggleconversationEmail = (CheckBox)e.Item.FindControl("chk_toggleconversationEmail");
                var chk_togglecommentEmail = (CheckBox)e.Item.FindControl("chk_togglecommentEmail");
                var chk_toggleprofanityFilter = (CheckBox)e.Item.FindControl("chk_toggleprofanityFilter");
                var chk_toggledisableOrg = (CheckBox)e.Item.FindControl("chk_toggledisableOrg");

                if (chk_approveConversation != null)
                {
                    if (rec["autoApproveConversation"].ToString() == "True")
                    {
                        chk_approveConversation.Checked = true;
                    }
                    else
                    {
                        chk_approveConversation.Checked = false;
                    }
                }

                if (chk_approveComment != null)
                {
                    if (rec["autoApproveComment"].ToString() == "True")
                    {
                        chk_approveComment.Checked = true;
                    }
                    else
                    {
                        chk_approveComment.Checked = false;
                    }
                }

                if (chk_conversationEmail != null)
                {
                    if (rec["conversationEmail"].ToString() == "True")
                    {
                        chk_conversationEmail.Checked = true;
                    }
                    else
                    {
                        chk_conversationEmail.Checked = false;
                    }
                }


                if (chk_commentEmail != null)
                {
                    if (rec["commentEmail"].ToString() == "True")
                    {
                        chk_commentEmail.Checked = true;
                    }
                    else
                    {
                        chk_commentEmail.Checked = false;
                    }
                }

                if (chk_profanityFilter != null)
                {
                    if (rec["profanityFilter"].ToString() == "True")
                    {
                        chk_profanityFilter.Checked = true;
                    }
                    else
                    {
                        chk_profanityFilter.Checked = false;
                    }
                }

                if (chk_disableOrg != null)
                {
                    if (rec["OrgEnabled"].ToString() == "True")
                    {
                        chk_disableOrg.Checked = false;
                    }
                    else
                    {
                        chk_disableOrg.Checked = true;
                    }
                }


                if (chk_toggleapproveConversation != null)
                {
                    if (rec["autoApproveConversation"].ToString() == "True")
                    {
                        chk_toggleapproveConversation.Checked = true;
                    }
                    else
                    {
                        chk_toggleapproveConversation.Checked = false;
                    }
                }

                if (chk_toggleapproveComment != null)
                {
                    if (rec["autoApproveComment"].ToString() == "True")
                    {
                        chk_toggleapproveComment.Checked = true;
                    }
                    else
                    {
                        chk_toggleapproveComment.Checked = false;
                    }
                }

                if (chk_toggleconversationEmail != null)
                {
                    if (rec["conversationEmail"].ToString() == "True")
                    {
                        chk_toggleconversationEmail.Checked = true;
                    }
                    else
                    {
                        chk_toggleconversationEmail.Checked = false;
                    }
                }


                if (chk_togglecommentEmail != null)
                {
                    if (rec["commentEmail"].ToString() == "True")
                    {
                        chk_togglecommentEmail.Checked = true;
                    }
                    else
                    {
                        chk_togglecommentEmail.Checked = false;
                    }
                }

                if (chk_toggleprofanityFilter != null)
                {
                    if (rec["profanityFilter"].ToString() == "True")
                    {
                        chk_toggleprofanityFilter.Checked = true;
                    }
                    else
                    {
                        chk_toggleprofanityFilter.Checked = false;
                    }
                }

                if (chk_toggledisableOrg != null)
                {
                    if (rec["OrgEnabled"].ToString() == "True")
                    {
                        chk_toggledisableOrg.Checked = false;
                    }
                    else
                    {
                        chk_toggledisableOrg.Checked = true;
                    }
                }
            }
        }

        protected void ManageOrg_ListView_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                // Find Controls in ListView
                var txt_OrgName = (TextBox)e.Item.FindControl("txt_OrgName");
                var txt_OrgAddress = (TextBox)e.Item.FindControl("txt_OrgAddress");
                var txt_OrgCity = (TextBox)e.Item.FindControl("txt_OrgCity");
                var txt_OrgState = (TextBox)e.Item.FindControl("txt_OrgState");
                var txt_OrgZip = (TextBox)e.Item.FindControl("txt_OrgZip");
                var txt_OrgPhone = (TextBox)e.Item.FindControl("txt_OrgPhone");
                var txt_OrgPhone2 = (TextBox)e.Item.FindControl("txt_OrgPhone2");
                var txt_OrgFax = (TextBox)e.Item.FindControl("txt_OrgFax");
                var txt_OrgEmail = (TextBox)e.Item.FindControl("txt_OrgEmail");
                var txt_OrgContact = (TextBox)e.Item.FindControl("txt_OrgContact");
                var txt_OrgDescription = (TextBox)e.Item.FindControl("txt_OrgDescription");
                var chk_toggleapproveConversation = (CheckBox)e.Item.FindControl("chk_toggleapproveConversation");
                var chk_toggleapproveComment = (CheckBox)e.Item.FindControl("chk_toggleapproveComment");
                var chk_toggleconversationEmail = (CheckBox)e.Item.FindControl("chk_toggleconversationEmail");
                var chk_togglecommentEmail = (CheckBox)e.Item.FindControl("chk_togglecommentEmail");
                var chk_toggleprofanityFilter = (CheckBox)e.Item.FindControl("chk_toggleprofanityFilter");
                var chk_toggledisableOrg = (CheckBox)e.Item.FindControl("chk_toggledisableOrg");

                // Set Update Parameters
                SqlDataSource_ManageOrg.UpdateParameters["OrgID"].DefaultValue = ddl_selectOrg.SelectedValue;
                SqlDataSource_ManageOrg.UpdateParameters["OrgName"].DefaultValue = txt_OrgName.Text;
                SqlDataSource_ManageOrg.UpdateParameters["OrgAddress"].DefaultValue = txt_OrgAddress.Text;
                SqlDataSource_ManageOrg.UpdateParameters["OrgCity"].DefaultValue = txt_OrgCity.Text;
                SqlDataSource_ManageOrg.UpdateParameters["OrgState"].DefaultValue = txt_OrgState.Text;
                SqlDataSource_ManageOrg.UpdateParameters["OrgZip"].DefaultValue = txt_OrgZip.Text;
                SqlDataSource_ManageOrg.UpdateParameters["OrgPhone"].DefaultValue = txt_OrgPhone.Text;
                SqlDataSource_ManageOrg.UpdateParameters["OrgPhone2"].DefaultValue = txt_OrgPhone2.Text;
                SqlDataSource_ManageOrg.UpdateParameters["OrgFax"].DefaultValue = txt_OrgFax.Text;
                SqlDataSource_ManageOrg.UpdateParameters["OrgEmail"].DefaultValue = txt_OrgEmail.Text;
                SqlDataSource_ManageOrg.UpdateParameters["OrgContact"].DefaultValue = txt_OrgContact.Text;
                SqlDataSource_ManageOrg.UpdateParameters["OrgDescription"].DefaultValue = txt_OrgDescription.Text;

                if (chk_toggleapproveConversation.Checked)
                {
                    SqlDataSource_ManageOrg.UpdateParameters["autoApproveConversation"].DefaultValue = "1";
                }

                if (chk_toggleapproveComment.Checked)
                {
                    SqlDataSource_ManageOrg.UpdateParameters["autoApproveComment"].DefaultValue = "1";
                }

                if (chk_toggleconversationEmail.Checked)
                {
                    SqlDataSource_ManageOrg.UpdateParameters["conversationEmail"].DefaultValue = "1";
                }

                if (chk_togglecommentEmail.Checked)
                {
                    SqlDataSource_ManageOrg.UpdateParameters["commentEmail"].DefaultValue = "1";
                }

                if (chk_toggleprofanityFilter.Checked)
                {
                    SqlDataSource_ManageOrg.UpdateParameters["profanityFilter"].DefaultValue = "1";
                }

                if (chk_toggledisableOrg.Checked)
                {
                    SqlDataSource_ManageOrg.UpdateParameters["OrgEnabled"].DefaultValue = "0";
                }
            }
        }


        //// END  DATALIST EVENTS ////


        //// BEGIN BUTTONS ////

        protected void lbn_addOrgDiv_Click(object sender, EventArgs e)

        {
            if (addNewOrg.Visible)
            {
                addNewOrg.Visible = false;
            }

            else
            {
                addNewOrg.Visible = true;
            }
        }

        protected void lbn_editOrgDiv_Click(object sender, EventArgs e)
        {
            if (editOrg.Visible)
            {
                editOrg.Visible = false;
            }

            else
            {
                editOrg.Visible = true;
            }
        }


        protected void btn_addNewOrg_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Add New Org to tbl_OrgsMaster

                var sqlConn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                var cmd =
                    new SqlCommand(
                        "INSERT INTO tbl_OrgsMaster (OrgName, OrgAddress, OrgCity, OrgState, OrgZip, OrgPhone, OrgPhone2, OrgFax, OrgEmail, OrgContact, OrgDescription, OrgStatus, OrgSubscription)VALUES(@OrgName, @OrgAddress, @OrgCity, @OrgState, @OrgZip, @OrgPhone, @OrgPhone2, @OrgFax, @OrgEmail, @OrgContact, @OrgDescription, @OrgStatus, @OrgSubscription)",
                        sqlConn);


                cmd.Parameters.Add("@OrgName", SqlDbType.VarChar, 255).Value = txt_addOrgName.Text;
                cmd.Parameters.Add("@OrgAddress", SqlDbType.VarChar, 255).Value = txt_addOrgAddress.Text;
                cmd.Parameters.Add("@OrgCity", SqlDbType.VarChar, 255).Value = txt_addOrgCity.Text;
                cmd.Parameters.Add("@OrgState", SqlDbType.VarChar, 255).Value = ddl_addOrgState.SelectedItem.ToString();
                cmd.Parameters.Add("@OrgZip", SqlDbType.VarChar, 255).Value = txt_addOrgZip.Text;
                cmd.Parameters.Add("@OrgPhone", SqlDbType.VarChar, 255).Value = txt_addOrgPhone.Text;
                cmd.Parameters.Add("@OrgPhone2", SqlDbType.VarChar, 255).Value = txt_addOrgPhone2.Text;
                cmd.Parameters.Add("@OrgFax", SqlDbType.VarChar, 255).Value = txt_addOrgFax.Text;
                cmd.Parameters.Add("@OrgEmail", SqlDbType.VarChar, 255).Value = txt_addOrgEmail.Text;
                cmd.Parameters.Add("@OrgContact", SqlDbType.VarChar, 255).Value = txt_addOrgContact.Text;
                cmd.Parameters.Add("@OrgDescription", SqlDbType.NVarChar, -1).Value = txt_addOrgDescription.Text;
                cmd.Parameters.Add("@OrgStatus", SqlDbType.VarChar, 255).Value =
                    ddl_addOrgStatus.SelectedItem.ToString();
                cmd.Parameters.Add("@OrgSubscription", SqlDbType.VarChar, 255).Value =
                    ddl_addOrgSubscription.SelectedItem.ToString();

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                // Clear Boxes
                txt_addOrgName.Text = "";
                txt_addOrgAddress.Text = "";
                txt_addOrgCity.Text = "";
                txt_addOrgZip.Text = "";
                txt_addOrgPhone.Text = "";
                txt_addOrgPhone2.Text = "";
                txt_addOrgFax.Text = "";
                txt_addOrgEmail.Text = "";
                txt_addOrgContact.Text = "";
                txt_addOrgDescription.Text = "";

                // Hide Div
                addNewOrg.Visible = false;

                // Rebind OrgDropdown
                BindOrgDropdown();
            }
        }

        //// END BUTTONS ////

        // Bind State Dropdown Lists
        public void BindStates()
        {
            /*bind states to drop-down list from XML*/
            var ds = new DataSet("States");
            ds.ReadXml(MapPath("/XML/States.xml"));

            ddl_addOrgState.DataSource = ds;
            ddl_addOrgState.DataTextField = "Abbr";
            ddl_addOrgState.DataValueField = "Name";
            ddl_addOrgState.DataBind();
            ddl_addOrgState.Items.Insert(0, new ListItem("[STATE]", "default"));
        }
    }
}