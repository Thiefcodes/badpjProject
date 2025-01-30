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
                Response.Redirect("~/CoachSubmitted.aspx");
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