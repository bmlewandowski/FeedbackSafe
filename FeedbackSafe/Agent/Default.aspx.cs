using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeedbackSafe.Agent
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // First Load Events
            if (!IsPostBack)
            {
                BindOrgDropdown();

                if (Session["OrgID"] == null)
                {
                    Session["OrgID"] = "0";
                }
            }
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
            ddl_selectOrg.Items.Insert(0, new ListItem("[Select Org To Impersonate]", "0"));
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        //// BEGIN DROPDOWN EVENTS ////
        protected void ddl_selectOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lbl_masterOrg = (Label) Master.FindControl("lbl_masterOrg");

            string sOrgID = ddl_selectOrg.SelectedValue;
            string sOrgName = ddl_selectOrg.SelectedItem.ToString();
            Session["OrgID"] = sOrgID;
            Session["OrgName"] = sOrgName;
            lbl_masterOrg.Text = sOrgName;

            // Populate Statistics
            GetStats();
        }

        //// GET STATISTICS ////

        public void GetStats()
        {
            string SelectSQLStats = "SELECT " +
                                    "Count_Approved=(SELECT Count(ConversationID) FROM tbl_Conversations WHERE OrgID = @OrgID AND ConversationApproved = '1')," +
                                    "Count_Unread=(SELECT Count(ConversationID) FROM tbl_Conversations WHERE OrgID = @OrgID AND ConversationUnread = '1')," +
                                    "Count_Leaders=(SELECT Count(UserID) FROM tbl_UsersMaster WHERE OrgID = @OrgID AND isLeader = '1')," +
                                    "Count_Users=(SELECT Count(UserID) FROM tbl_UsersMaster WHERE OrgID = @OrgID AND isLeader = '0')," +
                                    "Count_optOut=(SELECT Count(optOut) FROM tbl_UsersMaster WHERE OrgID = @OrgID AND optOut = '1')," +
                                    "Count_optOutConversation =(SELECT Count(optOutConversation) FROM tbl_UsersMaster WHERE OrgID = @OrgID AND optOutConversation = '1')," +
                                    "Count_optOutComment=(SELECT Count(optOutComment) FROM tbl_UsersMaster WHERE OrgID = @OrgID AND optOutComment = '1')";

            // Select Lists from DB
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            var cmd = new SqlCommand(SelectSQLStats, sqlConn);
            cmd.Parameters.Add("@OrgID", SqlDbType.VarChar, 255).Value = Session["OrgID"].ToString();
            cmd.Connection.Open();

            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lbl_stat_approved.Text = rdr["Count_Approved"].ToString();
                lbl_stat_unread.Text = rdr["Count_Unread"].ToString();
                lbl_stat_leaders.Text = rdr["Count_Leaders"].ToString();
                lbl_stat_users.Text = rdr["Count_Users"].ToString();
                lbl_optout.Text = rdr["Count_optOut"].ToString();
                lbl_stat_optoutconversations.Text = rdr["Count_optOutConversation"].ToString();
                lbl_stat_optoutcomments.Text = rdr["Count_optOutComment"].ToString();
            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
    }
}