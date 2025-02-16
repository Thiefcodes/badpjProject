using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

        private string GetCoachIdFromDatabase()
        {
            if (Session["UserId"] == null)
            {
                Response.Write("<script>alert('Session UserId is NULL.');</script>");
                return null;
            }

            int userId = Convert.ToInt32(Session["UserId"]); // Get UserId from session
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            string coachId = null;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // SQL query: Ensure isCoach is correctly checked
                string query = @"
            SELECT TOP 1 c.Id 
            FROM Coach c
            INNER JOIN CoachStatus cs ON c.Id = cs.coach_id
            WHERE cs.user_id = @UserId AND cs.isCoach = 1";  // Only check for 1 (INT)

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    coachId = result.ToString();
                }
            }

            // Debugging output
            if (coachId == null)
            {
                Response.Write("<script>alert('CoachId is NULL for UserId: " + userId + "');</script>");
            }
            else
            {
                Response.Write("<script>alert('CoachId from DB: " + coachId + "');</script>");
            }

            return coachId;
        }

        protected void btnChatUsers_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Write("<script>alert('Session expired. Please log in again.');</script>");
                return;
            }

            string coachId = GetCoachIdFromDatabase();

            if (!string.IsNullOrEmpty(coachId))
            {
                Response.Redirect("ChatList.aspx?coachId=" + coachId);
            }
            else
            {
                Response.Write("<script>alert('Error: No CoachId found for UserId: " + Session["UserId"].ToString() + ". Please check database.');</script>");
            }
        }

    }
}