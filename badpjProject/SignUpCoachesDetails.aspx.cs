using System;
using System.IO;
using System.Web.UI;

namespace badpjProject
{
    public partial class SignUpCoachesDetails : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string coachID = Request.QueryString["Id"];
                if (!string.IsNullOrEmpty(coachID))
                {
                    
                    Coaches coach = new Coaches().getCoaches(coachID);
                    if (coach != null)
                    {
                        lbl_CoachName.Text = coach.Coach_Name;
                        lbl_CoachEmail.Text = coach.Coach_Email;
                        lbl_CoachHp.Text = coach.Coach_Hp.ToString();
                        lbl_CoachDesc.Text = coach.Coach_Desc;
                        lbl_CoachQualification.Text = coach.Coach_Qualification;
                        lbl_CoachStatus.Text = coach.Coach_Status;

                        string videoPath = $"~/Uploads/{coach.Coach_Video}";

                        // Debugging: Show the resolved video path in an alert
                        string resolvedVideoPath = Server.MapPath(videoPath);
                        if (File.Exists(resolvedVideoPath))
                        {
                            videoSource.Attributes["src"] = ResolveUrl(videoPath);
                            videoContainer.Visible = true;
                            videoPlayer.Visible = true;
                            videoPlayer.DataBind();
                        }
                        else
                        {
                            videoContainer.Visible = false;
                        }
                    }
                    else
                    {
                      Response.Redirect("ViewCoaches.aspx");
                    }
                }
                else
                {
                   Response.Redirect("ViewCoaches.aspx");
                }
            }
        }

        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewCoaches.aspx");
        }
    }
}
