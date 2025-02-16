using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class Leaderboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLeaderboard();
            }
        }

        private void LoadLeaderboard()
        {
            List<Ranking> rankings = Ranking.GetAllRankings();
            rptLeaderboard.DataSource = rankings;
            rptLeaderboard.DataBind();
        }


        protected void btnSubmitVideo_Click(object sender, EventArgs e)
        {
            // Ensure the user is logged in.
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            int userId = int.Parse(Session["UserID"].ToString());

            // Validate file upload.
            if (!fu_Video.HasFile)
            {
                lblVideoStatus.Text = "Please select a video file to upload.";
                lblVideoStatus.Visible = true;
                return;
            }

            // Validate allowed video extensions.
            string[] allowedVideoExtensions = { ".mp4", ".avi", ".mov", ".wmv" };
            string videoExtension = Path.GetExtension(fu_Video.FileName).ToLower();
            if (!allowedVideoExtensions.Contains(videoExtension))
            {
                lblVideoStatus.Text = "Invalid video file format.";
                lblVideoStatus.Visible = true;
                return;
            }

            // Generate a unique filename and save the video.
            string uniqueVideoFileName = Guid.NewGuid().ToString() + videoExtension;
            string saveVideoPath = Server.MapPath("~/Uploads/") + uniqueVideoFileName;
            try
            {
                fu_Video.SaveAs(saveVideoPath);
            }
            catch (Exception ex)
            {
                lblVideoStatus.Text = "Error saving file: " + ex.Message;
                lblVideoStatus.Visible = true;
                return;
            }

            // Retrieve the optional comment.
            string comment = tb_Comment.Text.Trim();

            // Create a new video submission record.
            RankingVideoSubmission submission = new RankingVideoSubmission
            {
                UserID = userId,
                VideoFile = uniqueVideoFileName,
                Comment = comment
                // Status defaults to "Pending" and SubmissionDate is set automatically.
            };

            try
            {
                int result = submission.InsertSubmission();
                if (result > 0)
                {
                    lblVideoStatus.CssClass = "text-success";
                    lblVideoStatus.Text = "Video submitted successfully!";
                    lblVideoStatus.Visible = true;

                    // Optionally refresh the leaderboard.
                    LoadLeaderboard();

                    // Close the modal using a client script.
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", "$('#videoModal').modal('hide');", true);
                }
                else
                {
                    lblVideoStatus.Text = "Error: Failed to submit your video. Please try again.";
                    lblVideoStatus.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblVideoStatus.Text = "An unexpected error occurred: " + ex.Message;
                lblVideoStatus.Visible = true;
            }
        }
    }
}