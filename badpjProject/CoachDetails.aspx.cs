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
            string coachId = Request.QueryString["id"];
            Response.Redirect("Chat.aspx?coachId=" + coachId);
        }
    }
}