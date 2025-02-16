using badpjProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class ViewCoaches : System.Web.UI.Page
    {
        Coaches coachManager = new Coaches();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCoaches();
            }
        }

        /// <summary>
        /// Binds the coach data based on the selected filter and sort criteria.
        /// </summary>
        private void BindCoaches()
        {
            string statusFilter = ddlStatusFilter.SelectedValue;
            string sortBy = ddlSort.SelectedValue;
            List<Coaches> filteredCoaches = new List<Coaches>();

            // For Pending coaches.
            if (statusFilter.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                filteredCoaches = coachManager.GetPendingCoaches();
                rptPendingCoaches.Visible = true;
                rptApprovedCoaches.Visible = false;

                if (filteredCoaches.Count == 0)
                {
                    litNoPendingCoaches.Text = "<div class='alert alert-warning text-center' role='alert'>" +
                                                "<strong>Oops!</strong> There are currently no pending coaches to review. Please check back later." +
                                                "</div>";
                }
                else
                {
                    litNoPendingCoaches.Text = "";
                }
            }
            // For Approved coaches.
            else if (statusFilter.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                filteredCoaches = coachManager.GetApprovedCoaches();
                rptApprovedCoaches.Visible = true;
                rptPendingCoaches.Visible = false;

                if (filteredCoaches.Count == 0)
                {
                    litNoApprovedCoaches.Text = "<div class='alert alert-warning text-center' role='alert'>" +
                                                 "<strong>Oops!</strong> There are currently no approved coaches. Please check back later." +
                                                 "</div>";
                }
                else
                {
                    litNoApprovedCoaches.Text = "";
                }
            }

            // Apply sorting.
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                filteredCoaches = filteredCoaches.OrderBy(c => c.Coach_Name).ToList();
            }
            else if (sortBy.Equals("Email", StringComparison.OrdinalIgnoreCase))
            {
                filteredCoaches = filteredCoaches.OrderBy(c => c.Coach_Email).ToList();
            }

            // Bind the appropriate repeater.
            if (statusFilter.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                rptPendingCoaches.DataSource = filteredCoaches;
                rptPendingCoaches.DataBind();
            }
            else
            {
                rptApprovedCoaches.DataSource = filteredCoaches;
                rptApprovedCoaches.DataBind();
            }
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCoaches();
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCoaches();
        }

        protected void rptPendingCoaches_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string coachID = e.CommandArgument.ToString();

            if (e.CommandName == "Approve")
            {
                // Approve coach.
                bool coachApproved = coachManager.ApproveCoach(coachID);
                if (coachApproved)
                {
                    // Update CoachStatus to set isCoach = true.
                    CoachStatus cs = new CoachStatus();
                    bool statusUpdated = cs.UpdateIsCoachStatus(coachID);
                    if (statusUpdated)
                        ShowMessage("Coach approved successfully!", "success");
                    else
                        ShowMessage("Coach approved, but failed to update coach status record.", "error");
                }
                else
                {
                    ShowMessage("Error approving coach.", "error");
                }
            }
            else if (e.CommandName == "Reject")
            {
                // Reject coach (client confirmation handled in markup).
                bool coachDeleted = coachManager.RejectCoach(coachID);
                bool coachStatusDeleted = false;
                if (coachDeleted)
                {
                    CoachStatus cs = new CoachStatus();
                    coachStatusDeleted = cs.DeleteCoachStatus(coachID);
                }
                ShowMessage(
                    (coachDeleted && coachStatusDeleted) ? "Coach rejected successfully!" : "Error rejecting coach.",
                    (coachDeleted && coachStatusDeleted) ? "success" : "error"
                );
            }
            else if (e.CommandName == "ViewDetails")
            {
                Response.Redirect($"SignUpCoachesDetails.aspx?id={coachID}");
            }

            BindCoaches();
        }

        protected void rptApprovedCoaches_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string coachID = e.CommandArgument.ToString();

            if (e.CommandName == "ViewDetails")
            {
                Response.Redirect($"SignUpCoachesDetails.aspx?id={coachID}");
            }
            else if (e.CommandName == "Remove")
            {
                // Remove coach (client confirmation handled in markup).
                bool coachDeleted = coachManager.RejectCoach(coachID);
                bool coachStatusDeleted = false;
                if (coachDeleted)
                {
                    CoachStatus cs = new CoachStatus();
                    coachStatusDeleted = cs.DeleteCoachStatus(coachID);
                }
                ShowMessage(
                    (coachDeleted && coachStatusDeleted) ? "Coach removed successfully!" : "Error removing coach.",
                    (coachDeleted && coachStatusDeleted) ? "success" : "error"
                );
                BindCoaches();
            }
        }

        /// <summary>
        /// Displays a message in lblMessage using Bootstrap alert classes.
        /// </summary>
        private void ShowMessage(string message, string type)
        {
            // Determine alert class based on message type.
            string alertClass = type.Equals("success", StringComparison.OrdinalIgnoreCase) ? "alert-success" : "alert-danger";
            lblMessage.Text = message;
            lblMessage.CssClass = "alert " + alertClass + " text-center";
            lblMessage.Visible = true;
        }
    }
}
