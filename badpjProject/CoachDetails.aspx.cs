using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class CoachDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string coachId = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(coachId))
                {
                    // Retrieve the coach details using your Coaches class
                    Coaches coach = new Coaches().getCoaches(coachId);
                    if (coach != null)
                    {
                        // Set profile image; if missing, use the default image.
                        imgProfile.ImageUrl = string.IsNullOrEmpty(coach.Coach_ProfileImage)
                            ? "~/Uploads/default-image.png"
                            : "~/Uploads/" + coach.Coach_ProfileImage;

                        // Set other details
                        lblName.InnerText = coach.Coach_Name;
                        lblExpertise.InnerText = coach.Coach_AreaOfExpertise;
                        lblQualification.InnerText = coach.Coach_Qualification;
                        lblDescription.InnerText = coach.Coach_Desc;
                    }
                    else
                    {
                        // Optionally handle a case where no coach is found
                    }
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Coaches.aspx");
        }

        protected void btnChat_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Write("<script>alert('Session expired. Please log in again.');</script>");
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]); // Get UserId from session
            string coachId = Request.QueryString["id"]; // Get CoachId from URL

            if (!string.IsNullOrEmpty(coachId))
            {
                string redirectUrl = "Chat.aspx?userId=" + userId + "&coachId=" + coachId;
                Response.Write("<script>console.log('Redirecting to: " + redirectUrl + "');</script>");
                Response.Redirect(redirectUrl);
            }
            else
            {
                Response.Write("<script>alert('Error: No CoachId found.');</script>");
            }
        }

    }
}