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
            // Reset error label on page load
            lbl_Error.Visible = false;
            lbl_Error.Text = string.Empty;
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                lbl_Error.Visible = true;
                lbl_Error.Text = "Please fix the errors and try again.";
                return;
            }

            // Check file extension
            string[] allowedExtensions = { ".mp4", ".avi", ".mov", ".wmv" };
            string fileExtension = Path.GetExtension(fu_Coach.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                lbl_Error.Visible = true;
                lbl_Error.Text = "Please upload a valid video file (e.g., .mp4, .avi, .mov, .wmv).";
                return;
            }

            // Generate unique coach ID and file name
            string generatedId = GenerateCoachId();
            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            string saveVideoPath = Server.MapPath("~/Uploads/") + uniqueFileName;

            string status = "Pending";

            Coaches coach = new Coaches(
                generatedId,
                tb_Name.Text,
                tb_Email.Text,
                int.Parse(tb_Hp.Text),
                tb_AboutYou.Text,
                ddl_Qualification.SelectedValue,
                uniqueFileName,
                status
            );

            int result = coach.CoachesInsert();

            if (result > 0)
            {
                fu_Coach.SaveAs(saveVideoPath);
                Response.Redirect("~/CoachSubmit.aspx"); // Redirect to success page
            }
            else
            {
                lbl_Error.Visible = true;
                lbl_Error.Text = "Submission was not successful. Please try again later.";
            }
        }
    }
}