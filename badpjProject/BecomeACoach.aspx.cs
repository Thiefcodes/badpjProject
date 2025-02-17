using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class BecomeACoach : System.Web.UI.Page
    {
        public string GenerateCoachId()
        {
            return Guid.NewGuid().ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if the logged-in user's ID exists in Session.
                if (Session["UserID"] == null)
                {
                    // Not logged in, redirect to login page.
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                int userId = int.Parse(Session["UserID"].ToString());
                CoachStatus cs = new CoachStatus();

                // Check if the logged-in user already has a coach application.
                if (cs.IsUserAlreadySignUp(userId))
                {
                    // Hide the form and show the pending status message (using the Bootstrap alert div).
                    formDiv.Visible = false;
                    litPendingStatus.Text = "Your application is pending approval.";
                    divPendingStatus.Style["display"] = "block";
                }
                else
                {
                    // Show the form and hide the pending status alert.
                    formDiv.Visible = true;
                    divPendingStatus.Style["display"] = "none";
                }
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            ClearErrorStyles();

            if (!IsValid)
            {
                ApplyErrorStyles();
                return;
            }

            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            int userId = int.Parse(Session["UserID"].ToString());
            string generatedCoachId = GenerateCoachId();

            if (ddl_Qualification.SelectedValue == "Rank")
            {
                int userPoints = GetUserTotalPoints(userId);
                if (userPoints < 3000)
                {
                    return;
                }
            }
            // Validate the coach video file upload.
            string[] allowedVideoExtensions = { ".mp4", ".avi", ".mov", ".wmv" };
            string videoExtension = Path.GetExtension(fu_Coach.FileName).ToLower();
            if (!allowedVideoExtensions.Contains(videoExtension))
            {
                return;
            }
            string uniqueVideoFileName = Guid.NewGuid().ToString() + videoExtension;
            string saveVideoPath = Server.MapPath("~/Uploads/") + uniqueVideoFileName;

            // Since Profile Picture is removed, we won't handle fu_ProfilePic logic here.

            string status = "Pending";

            // Create the coach object with an empty profilePicFileName (or remove the parameter if not needed).
            Coaches coach = new Coaches(
                generatedCoachId,
                tb_Name.Text.Trim(),
                tb_Email.Text.Trim(),
                int.Parse(tb_Hp.Text.Trim()),
                tb_AboutYou.Text.Trim(),
                ddl_Qualification.SelectedValue,
                uniqueVideoFileName,
                status,
                "",
                ddl_AreaOfExpertise.SelectedValue
            );

            try
            {
                int result = coach.CoachesInsert();
                if (result > 0)
                {
                    CoachStatus cs = new CoachStatus();
                    bool insertStatus = cs.InsertCoachStatus(userId, generatedCoachId);
                    if (insertStatus)
                    {
                        // Save the coach video file
                        fu_Coach.SaveAs(saveVideoPath);

                        // After successful submission, redirect to a confirmation page.
                        Response.Redirect("~/CoachSubmitted.aspx");
                    }
                    else
                    {
                        litPendingStatus.Text = "Error: Failed to insert coach status record. Please try again later.";
                        divPendingStatus.CssClass = "alert alert-danger text-center";
                        divPendingStatus.Style["display"] = "block";
                    }
                }
                else
                {
                    litPendingStatus.Text = "Error: Failed to insert coach record. Please try again later.";
                    divPendingStatus.CssClass = "alert alert-danger text-center";
                    divPendingStatus.Style["display"] = "block";
                }
            }
            catch (Exception ex)
            {
                litPendingStatus.Text = "An unexpected error occurred: " + ex.Message;
                divPendingStatus.CssClass = "alert alert-danger text-center";
                divPendingStatus.Style["display"] = "block";
            }
        }

        private int GetUserTotalPoints(int userId)
        {
            Ranking userRank = Ranking.GetRankingByUserId(userId);
            if (userRank != null)
                return userRank.TotalPoints;
            return 0;
        }

        protected void cvRank_ServerValidate(object source, ServerValidateEventArgs e)
        {
            // If user didn't select "Placeholder Rank", pass validation.
            if (ddl_Qualification.SelectedValue != "Rank")
            {
                e.IsValid = true;
                return;
            }

            // If user *did* select "Placeholder Rank", check their points
            if (Session["UserID"] == null)
            {
                // Not logged in => can't validate rank properly. Mark invalid or handle differently.
                e.IsValid = false;
                return;
            }

            int userId = int.Parse(Session["UserID"].ToString());
            int userPoints = GetUserTotalPoints(userId); // Implement this method as needed

            if (userPoints < 3000)
            {
                // Not enough points
                e.IsValid = false;
            }
            else
            {
                e.IsValid = true;
            }
        }


        private void ClearErrorStyles()
        {
            // Remove error classes from controls
            tb_Name.CssClass = tb_Name.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            tb_Email.CssClass = tb_Email.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            tb_Hp.CssClass = tb_Hp.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            tb_AboutYou.CssClass = tb_AboutYou.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            ddl_Qualification.CssClass = ddl_Qualification.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            ddl_AreaOfExpertise.CssClass = ddl_AreaOfExpertise.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            fu_Coach.CssClass = fu_Coach.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
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
            if (!rfv_Coach.IsValid)
            {
                fu_Coach.CssClass += " input-validation-error";
            }
            if (!cvRank.IsValid)
            {
                ddl_Qualification.CssClass += " input-validation-error";
            }
        }
    }
}
