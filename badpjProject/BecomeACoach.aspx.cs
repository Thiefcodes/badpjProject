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

        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            // Debugging: Check if any required fields are missing
            if (string.IsNullOrWhiteSpace(tb_Name.Text) ||
                string.IsNullOrWhiteSpace(tb_Email.Text) ||
                string.IsNullOrWhiteSpace(tb_Hp.Text) ||
                string.IsNullOrWhiteSpace(tb_AboutYou.Text) ||
                string.IsNullOrWhiteSpace(ddl_Qualification.SelectedValue) ||
                !fu_Coach.HasFile)
            {
                Response.Write("<script>alert('All fields are required');</script>");
                return;
            }

            string[] allowedExtensions = { ".mp4", ".avi", ".mov", ".wmv" };
            string fileExtension = Path.GetExtension(fu_Coach.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                Response.Write("<script>alert('Please upload a valid video file (e.g., .mp4, .avi, .mov, .wmv)');</script>");
                return;
            }

            int result = 0;
            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

            // Debugging: Generate coach ID and output to the alert
            string generatedId = GenerateCoachId();

            string status = "Pending";

            Coaches coach = new Coaches(generatedId, tb_Name.Text, tb_Email.Text,
                                        int.Parse(tb_Hp.Text), tb_AboutYou.Text, ddl_Qualification.Text, uniqueFileName, status);

            result = coach.CoachesInsert();

            // Debugging: Check if the insertion was successful
            if (result > 0)
            {
                string saveVideoPath = Server.MapPath("~/Uploads/") + uniqueFileName;

                fu_Coach.SaveAs(saveVideoPath);
                Response.Write("<script>alert('Submission successful');</script>");
            }
            else
            {
                // Debugging: In case the insertion failed
                Response.Write("<script>alert('Submission NOT successful');</script>");
            }
        }
    }
}
