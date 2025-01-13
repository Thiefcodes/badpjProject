using System;
using System.Collections.Generic;
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

            int result = 0;
            string image = "";


            if (fu_Coach.HasFile)
            {
                image = fu_Coach.FileName;
            }

            string generatedId = GenerateCoachId();
            string status = "Pending";

            Coaches coach = new Coaches(generatedId, tb_Name.Text, tb_Email.Text, 
                                        int.Parse(tb_Hp.Text), tb_AboutYou.Text, ddl_Qualification.Text, image, status);
            result = coach.CoachesInsert();

            if (result > 0)
            {
                string saveimg = Server.MapPath("~/Uploads/") + image;
                fu_Coach.SaveAs(saveimg);
                Response.Write("<script>alert('Submission successful');</script>");
            }
            else { Response.Write("<script>alert('Submission NOT successful');</script>"); }


            Response.Redirect("Coaches.aspx");
        }
    }
}