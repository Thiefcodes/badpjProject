﻿using System;
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
                // Check if the user is logged in.
                if (Session["UserID"] != null)
                {
                    int userId = int.Parse(Session["UserID"].ToString());

                    // Check if user already has a pending submission.
                    if (RankingVideoSubmission.HasPendingSubmission(userId))
                    {
                        // Hide input fields and submit button
                        pnlVideoForm.Visible = false;
                        btnSubmitVideo.Visible = false;

                        // Display a Bootstrap-styled warning message in the literal
                        litVideoStatus.Visible = true;
                        litVideoStatus.Text = @"
                            <div class='alert alert-warning text-center' role='alert'>
                                You already have a pending submission under review. 
                                Please wait until it's approved or rejected before submitting another video.
                            </div>";
                    }
                    else
                    {
                        // Show input fields and submit button
                        pnlVideoForm.Visible = true;
                        btnSubmitVideo.Visible = true;

                        // Hide the literal message (clear any previous text)
                        litVideoStatus.Visible = false;
                        litVideoStatus.Text = "";
                    }
                }
                else
                {
                    // If not logged in, you may redirect to login or handle accordingly
                    Response.Redirect("~/Login.aspx");
                }

                // Optionally, load or refresh the leaderboard here
                LoadLeaderboard();
            }
        }

        private void LoadLeaderboard()
        {
            List<Ranking> rankings = Ranking.GetAllRankings();

            // Filter out unranked users if needed
            rankings = rankings.Where(r => r.TotalPoints >= 100).ToList();

            if (rankings.Count > 0)
            {
                rptLeaderboard.DataSource = rankings;
                rptLeaderboard.DataBind();
                rptLeaderboard.Visible = true;

                // Clear any previous message
                litMessage.Text = "";
            }
            else
            {
                rptLeaderboard.Visible = false;
                // Insert a Bootstrap-styled alert using HTML
                litMessage.Text = @"
                    <div class='alert alert-warning text-center mx-auto w-50' role='alert'>
                        <strong>Oops!</strong> No rankings available at this time. Please check back later.
                    </div>";
            }
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

            // 1. Check if the user already has a pending submission.
            if (RankingVideoSubmission.HasPendingSubmission(userId))
            {
                litVideoStatus.Visible = true;
                litVideoStatus.Text = @"
                    <div class='alert alert-warning text-center' role='alert'>
                        You already have a pending submission under review. 
                        Please wait until it's approved or rejected before submitting another video.
                    </div>";
                return;
            }

            // 2. Validate file upload.
            if (!fu_Video.HasFile)
            {
                litVideoStatus.Visible = true;
                litVideoStatus.Text = @"
                    <div class='alert alert-danger text-center' role='alert'>
                        Please select a video file to upload.
                    </div>";
                return;
            }

            // 3. Validate allowed video extensions.
            string[] allowedVideoExtensions = { ".mp4", ".avi", ".mov", ".wmv" };
            string videoExtension = Path.GetExtension(fu_Video.FileName).ToLower();
            if (!allowedVideoExtensions.Contains(videoExtension))
            {
                litVideoStatus.Visible = true;
                litVideoStatus.Text = @"
                    <div class='alert alert-danger text-center' role='alert'>
                        Invalid video file format.
                    </div>";
                return;
            }

            // 4. Generate a unique filename and save the video.
            string uniqueVideoFileName = Guid.NewGuid().ToString() + videoExtension;
            string saveVideoPath = Server.MapPath("~/Uploads/") + uniqueVideoFileName;
            try
            {
                fu_Video.SaveAs(saveVideoPath);
            }
            catch (Exception ex)
            {
                litVideoStatus.Visible = true;
                litVideoStatus.Text = @"
                    <div class='alert alert-danger text-center' role='alert'>
                        Error saving file: " + ex.Message + @"
                    </div>";
                return;
            }

            // 5. Retrieve the optional comment.
            string comment = tb_Comment.Text.Trim();

            // 6. Create a new video submission record (Status defaults to "Pending").
            RankingVideoSubmission submission = new RankingVideoSubmission
            {
                UserID = userId,
                VideoFile = uniqueVideoFileName,
                Comment = comment
            };

            try
            {
                int result = submission.InsertSubmission();
                if (result > 0)
                {
                    // Show success in the literal
                    litVideoStatus.Visible = true;
                    litVideoStatus.Text = @"
                        <div class='alert alert-success text-center' role='alert'>
                            Video submitted successfully!
                        </div>";

                    // Optionally refresh the leaderboard
                    LoadLeaderboard();

                    // Close the modal
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", "$('#videoModal').modal('hide');", true);
                }
                else
                {
                    litVideoStatus.Visible = true;
                    litVideoStatus.Text = @"
                        <div class='alert alert-danger text-center' role='alert'>
                            Error: Failed to submit your video. Please try again.
                        </div>";
                }
            }
            catch (Exception ex)
            {
                litVideoStatus.Visible = true;
                litVideoStatus.Text = @"
                    <div class='alert alert-danger text-center' role='alert'>
                        An unexpected error occurred: " + ex.Message + @"
                    </div>";
            }
        }
    }
}
