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
                // Retrieve the logged-in user's ID from session
                if (Session["UserID"] == null)
                {
                    // If not logged in, redirect to login page
                    Response.Redirect("~/Login.aspx");
                    return;
                }
                int userId = int.Parse(Session["UserID"].ToString());
                CoachStatus cs = new CoachStatus();

                // Check if the logged-in user already has a coach application
                if (cs.IsUserAlreadyCoach(userId))
                {
                    // Hide the form and show the "Pending" status message
                    formDiv.Visible = false;
                    lblStatus.Text = "Your application is pending.";
                    lblStatus.Visible = true;
                }
                else
                {
                    formDiv.Visible = true;
                    lblStatus.Visible = false;
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

            // Check file extension
            string[] allowedExtensions = { ".mp4", ".avi", ".mov", ".wmv" };
            string fileExtension = Path.GetExtension(fu_Coach.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return;
            }

            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            int userId = int.Parse(Session["UserID"].ToString());

            // Generate unique coach ID and file name
            string generatedCoachId = GenerateCoachId();
            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            string saveVideoPath = Server.MapPath("~/Uploads/") + uniqueFileName;
            string status = "Pending";

            Coaches coach = new Coaches(
                generatedCoachId,
                tb_Name.Text,
                tb_Email.Text,
                int.Parse(tb_Hp.Text),
                tb_AboutYou.Text,
                ddl_Qualification.SelectedValue,
                uniqueFileName,
                status
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
                        fu_Coach.SaveAs(saveVideoPath);
                        Response.Redirect("~/CoachSubmitted.aspx");
                    }
                    else
                    {
                        lblStatus.Text = "Error: Failed to insert coach status record. Please try again later.";
                        lblStatus.Visible = true;
                    }
                }
                else
                {
                    lblStatus.Text = "Error: Failed to insert coach record. Please try again later.";
                    lblStatus.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "An unexpected error occurred: " + ex.Message;
                lblStatus.Visible = true;
            }
        }

        private void ClearErrorStyles()
        {
            // Remove the error class and reset to default (black) border
            tb_Name.CssClass = tb_Name.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            tb_Email.CssClass = tb_Email.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            tb_Hp.CssClass = tb_Hp.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            tb_AboutYou.CssClass = tb_AboutYou.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
            ddl_Qualification.CssClass = ddl_Qualification.CssClass.Replace(" input-validation-error", "").Replace(" input-validation-success", "");
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
            if (!rfv_Coach.IsValid)
            {
                fu_Coach.CssClass += " input-validation-error";
            }
        }
    }
}