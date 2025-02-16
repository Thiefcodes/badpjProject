using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        /// <summary>
        /// Loads submissions based on selected filter and sort.
        /// </summary>
        private void LoadSubmissions()
        {
            string statusFilter = ddlStatusFilter.SelectedValue;
            string sortField = ddlSort.SelectedValue;

            // Retrieve submissions filtered by status and sorted by the selected field.
            List<RankingVideoSubmission> submissions = RankingVideoSubmission.GetSubmissionsByStatus(statusFilter, sortField);
            rptSubmissions.DataSource = submissions;
            rptSubmissions.DataBind();
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
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Cast the data item to your model.
                RankingVideoSubmission submission = (RankingVideoSubmission)e.Item.DataItem;

                // Find the panels.
                Panel pnlApproveReject = (Panel)e.Item.FindControl("pnlApproveReject");
                Panel pnlDelete = (Panel)e.Item.FindControl("pnlDelete");
                TextBox tbPoints = (TextBox)e.Item.FindControl("tbPoints");

                // Set the points TextBox based on status.
                if (submission.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    tbPoints.Enabled = false;
                    tbPoints.Text = submission.AssignedPoints.ToString();
                }
                else if (submission.Status.Equals("Rejected", StringComparison.OrdinalIgnoreCase) ||
                         submission.Status.Equals("Removed", StringComparison.OrdinalIgnoreCase))
                {
                    tbPoints.Enabled = false;
                    tbPoints.Text = "0";
                }

                // Toggle panels based on status:
                if (submission.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                {
                    pnlApproveReject.Visible = true;
                    pnlDelete.Visible = false;
                }
                else // For Approved, Rejected, or Removed
                {
                    pnlApproveReject.Visible = false;
                    pnlDelete.Visible = true;
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
            if (tbPoints != null && !string.IsNullOrEmpty(tbPoints.Text))
            {
                int.TryParse(tbPoints.Text, out points);
            }

            if (e.CommandName == "Approve")
            {
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
                LoadSubmissions();
            }

            // Refresh the submissions list.
            LoadSubmissions();
        }
    }
}