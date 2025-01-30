using badpjProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease;

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

        private void BindCoaches()
        {
            // Fetch pending and approved coaches
            List<Coaches> pendingCoaches = coachManager.GetPendingCoaches();
            List<Coaches> approvedCoaches = coachManager.GetApprovedCoaches();

            // Bind data to repeaters
            rptPendingCoaches.DataSource = pendingCoaches;
            rptPendingCoaches.DataBind();

            rptApprovedCoaches.DataSource = approvedCoaches;
            rptApprovedCoaches.DataBind();

            // Check if there is no data for pending coaches
            if (pendingCoaches.Count == 0)
            {
                // Hide the table if there are no pending coaches
                rptPendingCoaches.Visible = false;

                // Display the "No pending coaches" message
                litNoPendingCoaches.Text = "<div class='alert alert-warning text-center' role='alert'>" +
                                            "<strong>Oops!</strong> There are currently no pending coaches to review. Please check back later." +
                                            "</div>";
            }
            else
            {
                // Show the table if there are pending coaches
                rptPendingCoaches.Visible = true;

                // Reset the "No pending coaches" message
                litNoPendingCoaches.Text = "";
            }

            // Handle Approved Coaches (No data case)
            if (approvedCoaches.Count == 0)
            {
                rptApprovedCoaches.Visible = false;
                litNoApprovedCoaches.Text = "<div class='alert alert-warning text-center' role='alert'>" +
                                            "<strong>Oops!</strong> There are currently no approved coaches. Please check back later." +
                                            "</div>";
            }
            else
            {
                rptApprovedCoaches.Visible = true;
                litNoApprovedCoaches.Text = "";
            }
        }

        protected void rptPendingCoaches_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string coachID = e.CommandArgument.ToString();

            if (e.CommandName == "Approve")
            {
                bool success = coachManager.ApproveCoach(coachID);
                ShowAlert(success ? "Coach approved successfully" : "Error approving coach");
            }
            else if (e.CommandName == "Reject")
            {
                bool success = coachManager.RejectCoach(coachID);
                ShowAlert(success ? "Coach rejected successfully" : "Error rejecting coach");
            }
            else if (e.CommandName == "ViewDetails")
            {
                Response.Redirect($"SignUpCoachesDetails.aspx?id={coachID}");
            }

            BindCoaches(); // Refresh list after action
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
                bool success = coachManager.RejectCoach(coachID); // Treating "Remove" as "Reject"
                ShowAlert(success ? "Coach removed successfully" : "Error removing coach");
                BindCoaches();
            }
        }

        private void ShowAlert(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{message}');", true);
        }
    }
}