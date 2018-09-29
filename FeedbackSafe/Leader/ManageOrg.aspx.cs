using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using PS.OnlineImageOptimizer;

namespace FeedbackSafe.Leader
{
    public partial class ManageOrg : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // First Load Events
            if (!IsPostBack)
            {
                // Failsafe for losing OrgID
                if (Session["OrgID"] == null)
                {
                    Response.Redirect("Default.aspx");
                }

                BindOrg();
            }


            // Check for image in User Links folder
            string imgDisplayPath = Server.MapPath("~/Images/Users/" + FetchUser.UserID() + "/user_defined.jpg");
            if (File.Exists(imgDisplayPath))
            {
                // Set Image Control URL 
                img_user.ImageUrl = "~/Images/Users/" + FetchUser.UserID() + "/user_defined.jpg";
            }
        }

        //// LISTVIEW DATABIND ////
        protected void BindOrg()
        {
            string sOrgID = Session["OrgID"].ToString();
            SqlDataSource_ManageOrg.SelectParameters["OrgID"].DefaultValue = sOrgID;
            ManageOrg_ListView.DataBind();
        }


        //// BEGIN DATALIST EVENTS ////

        protected void ManageOrg_ListView_OnItemDataBound(Object sender, ListViewItemEventArgs e)
        {
            var rec = (DataRowView) e.Item.DataItem;
            // Make sure that you have the data
            if (rec != null)
            {
                var chk_approveConversation = (CheckBox) e.Item.FindControl("chk_approveConversation");
                var chk_approveComment = (CheckBox) e.Item.FindControl("chk_approveComment");
                var chk_conversationEmail = (CheckBox) e.Item.FindControl("chk_conversationEmail");
                var chk_commentEmail = (CheckBox) e.Item.FindControl("chk_commentEmail");
                var chk_profanityFilter = (CheckBox) e.Item.FindControl("chk_profanityFilter");
                var chk_toggleapproveConversation = (CheckBox) e.Item.FindControl("chk_toggleapproveConversation");
                var chk_toggleapproveComment = (CheckBox) e.Item.FindControl("chk_toggleapproveComment");
                var chk_toggleconversationEmail = (CheckBox) e.Item.FindControl("chk_toggleconversationEmail");
                var chk_togglecommentEmail = (CheckBox) e.Item.FindControl("chk_togglecommentEmail");
                var chk_toggleprofanityFilter = (CheckBox) e.Item.FindControl("chk_toggleprofanityFilter");

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
            }
        }

        protected void ManageOrg_ListView_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                // Find Controls in ListView
                var txt_OrgName = (TextBox) e.Item.FindControl("txt_OrgName");
                var txt_OrgAddress = (TextBox) e.Item.FindControl("txt_OrgAddress");
                var txt_OrgCity = (TextBox) e.Item.FindControl("txt_OrgCity");
                var txt_OrgState = (TextBox) e.Item.FindControl("txt_OrgState");
                var txt_OrgZip = (TextBox) e.Item.FindControl("txt_OrgZip");
                var txt_OrgPhone = (TextBox) e.Item.FindControl("txt_OrgPhone");
                var txt_OrgPhone2 = (TextBox) e.Item.FindControl("txt_OrgPhone2");
                var txt_OrgFax = (TextBox) e.Item.FindControl("txt_OrgFax");
                var txt_OrgEmail = (TextBox) e.Item.FindControl("txt_OrgEmail");
                var txt_OrgContact = (TextBox) e.Item.FindControl("txt_OrgContact");
                var txt_OrgDescription = (TextBox) e.Item.FindControl("txt_OrgDescription");
                var chk_toggleapproveConversation = (CheckBox) e.Item.FindControl("chk_toggleapproveConversation");
                var chk_toggleapproveComment = (CheckBox) e.Item.FindControl("chk_toggleapproveComment");
                var chk_toggleconversationEmail = (CheckBox) e.Item.FindControl("chk_toggleconversationEmail");
                var chk_togglecommentEmail = (CheckBox) e.Item.FindControl("chk_togglecommentEmail");
                var chk_toggleprofanityFilter = (CheckBox) e.Item.FindControl("chk_toggleprofanityFilter");

                // Set Update Parameters
                SqlDataSource_ManageOrg.UpdateParameters["OrgID"].DefaultValue = Session["OrgID"].ToString();
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
            }
        }

        //// END  DATALIST EVENTS ////


        //// BEGIN BUTTONS ////

        protected void btn_UserImage_Click(object sender, EventArgs e)
        {
            if (upl_userimage.HasFile)
            {
                var op = new ImageOptimizer();
                op.ImgQuality = 80;
                op.MaxHeight = 150;
                op.MaxWidth = 150;

                Bitmap bmp = ResizeImage(upl_userimage.PostedFile.InputStream, 150, 150);

                bmp.Save(Server.MapPath("~/Images/Users/" + FetchUser.UserID() + "/user_defined.jpg"), ImageFormat.Jpeg);
                op.Optimize(Server.MapPath("~/Images/Users/" + FetchUser.UserID() + "/user_defined.jpg"));

                // Reload Page
                Response.Redirect("ManageOrg.aspx");
            }
        }

        //// END  BUTTONS ////


        //// BEGIN IMAGE FUNCTION ////

        // Resize Uploaded Image
        private Bitmap ResizeImage(Stream streamImage, int maxWidth, int maxHeight)
        {
            var originalImage = new Bitmap(streamImage);
            int newWidth = originalImage.Width;
            int newHeight = originalImage.Height;
            double aspectRatio = originalImage.Width/(double) originalImage.Height;

            if (aspectRatio <= 1 && originalImage.Width > maxWidth)
            {
                newWidth = maxWidth;
                newHeight = (int) Math.Round(newWidth/aspectRatio);
            }
            else if (aspectRatio > 1 && originalImage.Height > maxHeight)
            {
                newHeight = maxHeight;
                newWidth = (int) Math.Round(newHeight*aspectRatio);
            }

            return new Bitmap(originalImage, newWidth, newHeight);
        }

        //// END  IMAGE FUNCTION ////
    }
}