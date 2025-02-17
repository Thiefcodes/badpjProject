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
        /// Only one set (Pending OR Approved) is displayed at a time.
        /// </summary>
        private void BindCoaches()
        {
            string statusFilter = ddlStatusFilter.SelectedValue;
            string sortBy = ddlSort.SelectedValue;
            List<Coaches> coachesList = new List<Coaches>();

            // Retrieve data based on the filter.
            if (statusFilter.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                coachesList = coachManager.GetPendingCoaches();
            }
            else if (statusFilter.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                coachesList = coachManager.GetApprovedCoaches();
            }

            // Apply sorting.
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                coachesList = coachesList.OrderBy(c => c.Coach_Name).ToList();
            }
            else if (sortBy.Equals("Email", StringComparison.OrdinalIgnoreCase))
            {
                coachesList = coachesList.OrderBy(c => c.Coach_Email).ToList();
            }

            // Bind data to repeater or display "no data" message.
            if (coachesList.Count > 0)
            {
                rptCoaches.DataSource = coachesList;
                rptCoaches.DataBind();
                rptCoaches.Visible = true;
                litNoCoaches.Text = "";
            }
            else
            {
                rptCoaches.Visible = false;
                litNoCoaches.Text = $@"
                    <div class='alert alert-warning text-center' role='alert'>
                        <strong>Oops!</strong> There are currently no {statusFilter.ToLower()} coaches to display. 
                        Please check back later.
                    </div>";
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

        /// <summary>
        /// Toggle panels in ItemDataBound based on the current filter (Pending or Approved).
        /// </summary>
        protected void rptCoaches_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Find the panels in this row
                Panel pnlApproveCoach = (Panel)e.Item.FindControl("pnlApproveCoach");
                Panel pnlRemoveCoach = (Panel)e.Item.FindControl("pnlRemoveCoach");

                // Check the filter
                string currentFilter = ddlStatusFilter.SelectedValue; // e.g. "Pending" or "Approved"

                if (currentFilter.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                {
                    // Show Approve/Reject panel, hide Remove panel
                    pnlApproveCoach.Visible = true;
                    pnlRemoveCoach.Visible = false;
                }
                else if (currentFilter.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    // Hide Approve/Reject panel, show Remove panel
                    pnlApproveCoach.Visible = false;
                    pnlRemoveCoach.Visible = true;
                }
            }
        }

        protected void rptCoaches_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string coachID = e.CommandArgument.ToString();

            if (e.CommandName == "ViewDetails")
            {
                Response.Redirect($"SignUpCoachesDetails.aspx?id={coachID}");
            }
            else if (e.CommandName == "Approve")
            {
                // Approve the coach
                bool coachApproved = coachManager.ApproveCoach(coachID);
                if (coachApproved)
                {
                    // After approving, optionally update the CoachStatus record to set isCoach = true
                    CoachStatus cs = new CoachStatus();
                    bool statusUpdated = cs.UpdateIsCoachStatus(coachID);

                    if (statusUpdated)
                        ShowMessage("Coach approved successfully!", "success");
                    else
                        ShowMessage("Coach approved, but failed to update coach status.", "error");
                }
                else
                {
                    ShowMessage("Error approving coach.", "error");
                }
                BindCoaches();
            }
            else if (e.CommandName == "Remove")
            {
                // Attempt to delete the Coach record (RejectCoach in your logic).
                bool coachDeleted = coachManager.RejectCoach(coachID);
                if (coachDeleted)
                {
                    // Then remove the CoachStatus record.
                    CoachStatus cs = new CoachStatus();
                    bool coachStatusDeleted = cs.DeleteCoachStatus(coachID);

                    if (coachStatusDeleted)
                    {
                        // Both the coach record and coach status record are removed.
                        ShowMessage("Coach removed successfully!", "success");
                    }
                    else
                    {
                        // Coach record is removed, but status record failed to delete.
                        ShowMessage("Coach removed, but failed to remove coach status record.", "error");
                    }
                }
                else
                {
                    // The coach record itself failed to remove.
                    ShowMessage("Error removing coach.", "error");
                }

                // Refresh the list of coaches.
                BindCoaches();
            }
        }

        private void ShowMessage(string message, string type)
        {
            string alertClass = type.Equals("success", StringComparison.OrdinalIgnoreCase) ? "alert-success" : "alert-danger";
            lblMessage.Text = message;
            lblMessage.CssClass = "alert " + alertClass + " text-center";
            lblMessage.Visible = true;
        }
    }
}
