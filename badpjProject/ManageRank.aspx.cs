using badpjProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class ManageRank : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSubmissions();
            }
        }

        private void LoadSubmissions()
        {
            string statusFilter = ddlStatusFilter.SelectedValue;
            string sortField = ddlSort.SelectedValue;

            // Retrieve submissions filtered by status and sorted by the selected field.
            List<RankingVideoSubmission> submissions = RankingVideoSubmission.GetSubmissionsByStatus(statusFilter, sortField);

            if (submissions.Count > 0)
            {
                rptSubmissions.DataSource = submissions;
                rptSubmissions.DataBind();
                rptSubmissions.Visible = true;
                litNoSubmissions.Text = "";
            }
            else
            {
                rptSubmissions.Visible = false;
                litNoSubmissions.Text = "<div class='alert alert-warning text-center' role='alert'>" +
                                        $"<strong>Oops!</strong> There are currently no {statusFilter.ToLower()} submissions. Please check back later." +
                                        "</div>";
            }
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSubmissions();
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSubmissions();
        }

        private bool HideActionsColumn
        {
            get
            {
                string filter = ddlStatusFilter.SelectedValue;
                return filter.Equals("Approved", StringComparison.OrdinalIgnoreCase) ||
                       filter.Equals("Rejected", StringComparison.OrdinalIgnoreCase);
            }
        }

        protected void rptSubmissions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Cast the data item to your model.
                RankingVideoSubmission submission = (RankingVideoSubmission)e.Item.DataItem;

                // Find the controls using the new panel names.
                Panel pnlPendingActions = (Panel)e.Item.FindControl("pnlPendingActions");
                Panel pnlDeleteActions = (Panel)e.Item.FindControl("pnlDeleteAction");
                TextBox tbPoints = (TextBox)e.Item.FindControl("tbPoints");

                // Set the points textbox based on status.
                if (submission.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    tbPoints.Enabled = false;
                    tbPoints.Text = submission.AssignedPoints.ToString();
                }
                else if (submission.Status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                {
                    tbPoints.Enabled = false;
                    tbPoints.Text = "0";
                }

                // Toggle panels:
                if (submission.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                {
                    pnlPendingActions.Visible = true;
                    pnlDeleteActions.Visible = false;
                }
                else // Approved, Rejected, or Removed.
                {
                    pnlPendingActions.Visible = false;
                    pnlDeleteActions.Visible = true;
                }
            }
        }

        protected void rptSubmissions_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string submissionIdStr = e.CommandArgument.ToString();
            Guid submissionId = Guid.Parse(submissionIdStr);

            // Retrieve the submission record.
            RankingVideoSubmission submission = RankingVideoSubmission.GetSubmissionById(submissionId);
            if (submission == null)
            {
                lblMessage.Text = "Submission not found.";
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
                return;
            }

            // Locate the points textbox within the repeater item.
            TextBox tbPoints = (TextBox)e.Item.FindControl("tbPoints");
            int points = 0;
            if (e.CommandName == "Approve")
            {
                // Check if the assign point field is empty.
                if (string.IsNullOrWhiteSpace(tbPoints.Text))
                {
                    lblMessage.Text = "Please assign points before approving.";
                    lblMessage.CssClass = "alert alert-warning text-center";
                    lblMessage.Visible = true;
                    return;
                }

                int.TryParse(tbPoints.Text, out points);

                // If already approved, do not allow re-approval.
                if (submission.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    lblMessage.Text = "This submission has already been approved.";
                    lblMessage.CssClass = "alert alert-warning";
                    lblMessage.Visible = true;
                    return;
                }

                // Update submission record: set status to Approved and store the assigned points.
                int updateResult = submission.ApproveSubmission(points);
                if (updateResult > 0)
                {
                    // Update or create the user's ranking record.
                    Ranking ranking = Ranking.GetRankingByUserId(submission.UserID);
                    if (ranking != null)
                    {
                        ranking.UpdateRanking(points);
                    }
                    else
                    {
                        ranking = new Ranking
                        {
                            UserID = submission.UserID,
                            TotalPoints = points,
                            Rank = "Beginner" // This value will be recalculated in UpdateRanking.
                        };
                        ranking.InsertRanking();
                    }

                    lblMessage.Text = "Submission approved and points assigned.";
                    lblMessage.CssClass = "alert alert-success";
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.Text = "Error updating submission.";
                    lblMessage.CssClass = "alert alert-danger";
                    lblMessage.Visible = true;
                }
            }
            else if (e.CommandName == "Reject")
            {
                int updateResult = submission.UpdateSubmissionStatus("Rejected");
                if (updateResult > 0)
                {
                    lblMessage.Text = "Submission rejected.";
                    lblMessage.CssClass = "alert alert-success";
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.Text = "Error updating submission.";
                    lblMessage.CssClass = "alert alert-danger";
                    lblMessage.Visible = true;
                }
            }
            else if (e.CommandName == "DeleteSubmission")
            {
                // Call your delete method
                int result = RankingVideoSubmission.DeleteSubmissionById(submissionId);
                if (result > 0)
                {
                    lblMessage.Text = "Submission deleted successfully.";
                    lblMessage.CssClass = "alert alert-success";
                }
                else
                {
                    lblMessage.Text = "Error deleting submission.";
                    lblMessage.CssClass = "alert alert-danger";
                }
                lblMessage.Visible = true;
            }

            // Refresh the submissions list.
            LoadSubmissions();
        }
    }
}
