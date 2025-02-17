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
                    // Retrieve the coach ID from the query string.
                    string coachID = Request.QueryString["Id"];
                    if (string.IsNullOrEmpty(coachID))
                    {
                        Response.Redirect("ViewCoaches.aspx");
                        return;
                    }

                    Coaches coach = new Coaches().getCoaches(coachID);
                    if (coach == null)
                    {
                        Response.Redirect("ViewCoaches.aspx");
                        return;
                    }

                    // Set coach details into labels.
                    lbl_CoachName.Text = coach.Coach_Name;
                    lbl_CoachEmail.Text = coach.Coach_Email;
                    lbl_CoachHp.Text = coach.Coach_Hp.ToString();
                    lbl_CoachDesc.Text = coach.Coach_Desc;
                    lbl_CoachQualification.Text = coach.Coach_Qualification;
                    lbl_CoachStatus.Text = coach.Coach_Status;

                    // Setup the video player if the video file exists.
                    string videoPath = $"~/Uploads/{coach.Coach_Video}";
                    string resolvedVideoPath = Server.MapPath(videoPath);
                    if (File.Exists(resolvedVideoPath))
                    {
                        videoSource.Attributes["src"] = ResolveUrl(videoPath);
                        videoContainer.Visible = true;
                        videoPlayer.Visible = true;
                    }
                    else
                    {
                        videoContainer.Visible = false;
                    }

                    string certPath = $"~/Uploads/{coach.Coach_CertificationFile}";
                    string resolvedCertPath = Server.MapPath(certPath);
                    if (!string.IsNullOrEmpty(coach.Coach_CertificationFile) && File.Exists(resolvedCertPath))
                    {
                        pnlCertDoc.Visible = true;

                        // Set the link text and URL
                        lnkCertDoc.Text = "View Certificate";
                        lnkCertDoc.NavigateUrl = ResolveUrl(certPath);

                        // Open the certificate in a new pop-up window
                        lnkCertDoc.Attributes["onclick"] = "window.open('" + ResolveUrl(certPath) +
                            "', 'popupwindow', 'width=800,height=600,scrollbars=yes,resizable=yes'); return false;";
                    }
                    else
                    {
                        pnlCertDoc.Visible = false;
                    }
            }
        }

            protected void Btn_Back_Click(object sender, EventArgs e)
            {
                Response.Redirect("ViewCoaches.aspx");
            }
        }
    }
