using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class CoachEditProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Ensure the coach is logged in
                if (Session["UserId"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }
                int userId = int.Parse(Session["UserId"].ToString());
                Coaches coach = new Coaches().GetCoachByUserId(userId);
                if (coach != null)
                {
                    tb_Name.Text = coach.Coach_Name;
                    tb_Email.Text = coach.Coach_Email;
                    tb_Hp.Text = coach.Coach_Hp.ToString();
                    tb_AboutYou.Text = coach.Coach_Desc;

                    // Populate Qualification dropdown with current value and disable editing
                    ddl_Qualification.Items.Clear();
                    ddl_Qualification.Items.Add(new ListItem(coach.Coach_Qualification, coach.Coach_Qualification));
                    ddl_Qualification.Enabled = false;

                    // Populate Area of Expertise dropdown with options and select current value
                    ddl_AreaOfExpertise.Items.Clear();
                    ddl_AreaOfExpertise.Items.Add(new ListItem("Strength Training", "Strength Training"));
                    ddl_AreaOfExpertise.Items.Add(new ListItem("Cardiovascular Training", "Cardiovascular Training"));
                    ddl_AreaOfExpertise.Items.Add(new ListItem("HIIT", "HIIT"));
                    ddl_AreaOfExpertise.Items.Add(new ListItem("Bodybuilding", "Bodybuilding"));
                    ddl_AreaOfExpertise.Items.Add(new ListItem("Powerlifting", "Powerlifting"));
                    ddl_AreaOfExpertise.Items.Add(new ListItem("CrossFit", "CrossFit"));
                    ddl_AreaOfExpertise.Items.Add(new ListItem("Functional Training", "Functional Training"));
                    ddl_AreaOfExpertise.Items.Add(new ListItem("Endurance Training", "Endurance Training"));
                    ddl_AreaOfExpertise.Items.Add(new ListItem("Flexibility & Mobility Training", "Flexibility & Mobility Training"));
                    ddl_AreaOfExpertise.Items.Add(new ListItem("Sports Performance Training", "Sports Performance Training"));
                    ddl_AreaOfExpertise.Items.Add(new ListItem("Weight Loss & Fat Reduction", "Weight Loss & Fat Reduction"));
                    ddl_AreaOfExpertise.Items.Add(new ListItem("Injury Prevention & Rehabilitation", "Injury Prevention & Rehabilitation"));
                    ddl_AreaOfExpertise.Items.Add(new ListItem("Core Training", "Core Training"));
                    
                    if (!string.IsNullOrEmpty(coach.Coach_AreaOfExpertise))
                    {
                        ddl_AreaOfExpertise.SelectedValue = coach.Coach_AreaOfExpertise;
                    }

                    // Set the profile image, using default if not set
                    imgProfile.ImageUrl = string.IsNullOrEmpty(coach.Coach_ProfileImage)
                        ? "~/Uploads/default-image.png"
                        : "~/Uploads/" + coach.Coach_ProfileImage;
                }
                else
                {
                    // Optionally handle case when no coach record exists
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("CoachProfile.aspx");
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            ClearErrorStyles();
            if (!Page.IsValid)
            {
                ApplyErrorStyles();
                return;
            }
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            int userId = int.Parse(Session["UserId"].ToString());
            Coaches coach = new Coaches().GetCoachByUserId(userId);
            if (coach != null)
            {
                // Update fields from the form
                coach.Coach_Name = tb_Name.Text.Trim();
                coach.Coach_Email = tb_Email.Text.Trim();
                coach.Coach_Hp = int.Parse(tb_Hp.Text.Trim());
                coach.Coach_Desc = tb_AboutYou.Text.Trim();
                coach.Coach_AreaOfExpertise = ddl_AreaOfExpertise.SelectedValue;

                // If a new profile picture is uploaded, delete old image and update with new one
                if (fu_ProfilePic.HasFile)
                {
                    string[] allowedImageExtensions = { ".jpg", ".jpeg", ".png" };
                    string imageExtension = Path.GetExtension(fu_ProfilePic.FileName).ToLower();
                    if (allowedImageExtensions.Contains(imageExtension))
                    {
                        // Delete old image if it exists and is not the default image
                        if (!string.IsNullOrEmpty(coach.Coach_ProfileImage) &&
                            !coach.Coach_ProfileImage.ToLower().Contains("default-image"))
                        {
                            string oldImagePath = Server.MapPath("~/Uploads/") + coach.Coach_ProfileImage;
                            if (File.Exists(oldImagePath))
                            {
                                File.Delete(oldImagePath);
                            }
                        }
                        // Save the new image
                        string newProfilePicFileName = Guid.NewGuid().ToString() + imageExtension;
                        string newProfilePicSavePath = Server.MapPath("~/Uploads/") + newProfilePicFileName;
                        fu_ProfilePic.SaveAs(newProfilePicSavePath);
                        coach.Coach_ProfileImage = newProfilePicFileName;
                    }
                }

                // Update the coach profile in the database
                bool success = coach.UpdateCoachProfile();
                if (success)
                {
                    Response.Redirect("CoachProfile.aspx");
                }
                else
                {
                    lblStatus.Text = "Error updating profile. Please try again later.";
                    lblStatus.Visible = true;
                }
            }
        }

        private void ClearErrorStyles()
        {
            tb_Name.CssClass = tb_Name.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            tb_Email.CssClass = tb_Email.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            tb_Hp.CssClass = tb_Hp.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            tb_AboutYou.CssClass = tb_AboutYou.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            ddl_Qualification.CssClass = ddl_Qualification.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            ddl_AreaOfExpertise.CssClass = ddl_AreaOfExpertise.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            fu_ProfilePic.CssClass = fu_ProfilePic.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
        }

        private void ApplyErrorStyles()
        {
            if (!rfv_Name.IsValid)
            {
                tb_Name.CssClass += " input-validation-error";
            }
            if (!rfv_Email.IsValid)
            {
                tb_Email.CssClass += " input-validation-error";
            }
            if (!rev_Email.IsValid)
            {
                tb_Email.CssClass += " input-validation-error";
            }
            if (!rfv_Hp.IsValid)
            {
                tb_Hp.CssClass += " input-validation-error";
            }
            if (!rev_Hp.IsValid)
            {
                tb_Hp.CssClass += " input-validation-error";
            }
            if (!rfv_AboutYou.IsValid)
            {
                tb_AboutYou.CssClass += " input-validation-error";
            }
            if (!rfv_Qualification.IsValid)
            {
                ddl_Qualification.CssClass += " input-validation-error";
            }
            if (!rfv_AreaOfExpertise.IsValid)
            {
                ddl_AreaOfExpertise.CssClass += " input-validation-error";
            }
        }
    }
}
