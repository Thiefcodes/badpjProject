using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class CoachProfile : System.Web.UI.Page
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

                // Retrieve the coach's details by userId
                Coaches coach = new Coaches().GetCoachByUserId(userId);
                if (coach != null)
                {
                    lblName.InnerText = coach.Coach_Name;
                    lblEmail.InnerText = coach.Coach_Email;
                    lblHp.InnerText = coach.Coach_Hp.ToString();
                    lblDesc.InnerText = coach.Coach_Desc;
                    lblQualification.InnerText = coach.Coach_Qualification;
                    lblExpertise.InnerText = coach.Coach_AreaOfExpertise;

                    // Set the profile image, using default if none provided
                    imgProfile.ImageUrl = string.IsNullOrEmpty(coach.Coach_ProfileImage)
                        ? "~/Uploads/default-image.png"
                        : "~/Uploads/" + coach.Coach_ProfileImage;
                }
                else
                {
                    // Optionally, handle the case when no coach record exists
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            // Redirect to the CoachEditProfile page
            Response.Redirect("CoachEditProfile.aspx");
        }
    }
}